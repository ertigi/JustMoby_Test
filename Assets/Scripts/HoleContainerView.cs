using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public sealed class HoleContainerView : MonoBehaviour
{
    [SerializeField] private RectTransform _holeRect;
    [SerializeField] private HoleCubeView _cubePrefab;
    private CompositeDisposable _cd = new();
    private Stack<HoleCubeView> _pool = new();
    private HoleViewModel _holeViewModel;

    [Inject]
    public void Construct(HoleViewModel holeViewModel)
    {
        _holeViewModel = holeViewModel;
    }

    private void Awake()
    {
        _holeViewModel.OnCubeSpawnRequested
            .Subscribe(SpawnCube)
            .AddTo(_cd);
    }

    private void OnDestroy()
    {
        _cd.Clear();
    }

    private void SpawnCube(HoleCubeSpawnData data)
    {
        var view = Spawn();

        view.Init(data);

        view.OnAnimationCompleted
            .Subscribe(_ => Despawn(view))
            .AddTo(view);
    }

    private HoleCubeView Spawn()
    {
        HoleCubeView view;
        if (_pool.Count > 0)
        {
            view = _pool.Pop();
            view.gameObject.SetActive(true);
            return view;
        }

        view = Instantiate(_cubePrefab, _holeRect);
        
        return view;
    }

    private void Despawn(HoleCubeView view)
    {
        view.ResetState();
        view.gameObject.SetActive(false);
        _pool.Push(view);
    }
}
