using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public sealed class TowerStackContainerView : MonoBehaviour
{
    [SerializeField] private RectTransform _container;
    [SerializeField] private TowerCubeView _cubePrefab;
    private TowerViewModel _towerViewModel;
    private DragDropController _dragDropController;
    private List<TowerCubeView> _views = new();
    private Stack<TowerCubeView> _pool = new();
    private CompositeDisposable _cd = new();


    [Inject]
    public void Construct(TowerViewModel towerViewModel, DragDropController dragDropController)
    {
        _dragDropController = dragDropController;
        _towerViewModel = towerViewModel;
    }

    private void Awake()
    {
        _towerViewModel.Cubes.ObserveReset()
            .Subscribe(_ => SyncAll())
            .AddTo(_cd);

        _towerViewModel.Cubes.ObserveAdd()
            .Subscribe(e =>
            {
                InsertView(e.Index, e.Value);
                RefreshSiblingOrderFrom(e.Index);
            })
            .AddTo(_cd);

        _towerViewModel.Cubes.ObserveRemove()
            .Subscribe(e =>
            {
                RemoveViewAt(e.Index);
                RefreshSiblingOrderFrom(e.Index);
            })
            .AddTo(_cd);
    }

    private void OnDisable()
    {
        _cd.Clear();
    }

    private void SyncAll()
    {
        for (int i = _views.Count - 1; i >= 0; i--)
            Despawn(_views[i]);
        _views.Clear();

        for (int i = 0; i < _towerViewModel.Cubes.Count; i++)
            InsertView(i, _towerViewModel.Cubes[i]);

        RefreshSiblingOrderFrom(0);
    }

    private void InsertView(int index, TowerCubeViewModel cubeVm)
    {
        var view = Spawn();
        view.transform.SetSiblingIndex(index);

        view.Bind(_dragDropController, cubeVm);
        _views.Insert(index, view);
    }

    private void RemoveViewAt(int index)
    {
        if (index < 0 || index >= _views.Count)
            return;

        var view = _views[index];
        _views.RemoveAt(index);

        Despawn(view);
    }

    private void RefreshSiblingOrderFrom(int startIndex)
    {
        for (int i = startIndex; i < _views.Count; i++)
            _views[i].transform.SetSiblingIndex(i);
    }

    private TowerCubeView Spawn()
    {
        TowerCubeView towerCubeView;
        if (_pool.Count > 0)
        {
            towerCubeView = _pool.Pop();
            towerCubeView.gameObject.SetActive(true);
            return towerCubeView;
        }
        else
        {
            towerCubeView = Instantiate(_cubePrefab, _container);
        }

        return towerCubeView;
    }

    private void Despawn(TowerCubeView view)
    {
        view.Unbind();
        view.gameObject.SetActive(false);
        view.transform.SetParent(transform, false);
        _pool.Push(view);
    }
}