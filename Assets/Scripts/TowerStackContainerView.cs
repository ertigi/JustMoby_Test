using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public sealed class TowerStackContainerView : MonoBehaviour
{
    [SerializeField] private RectTransform _container;
    [SerializeField] private TowerCubeView _cubePrefab;
    private List<TowerCubeView> _views = new();
    private DragDropController _dragDropController;
    private CompositeDisposable _cd = new();
    private TowerViewModel _vm;


    [Inject]
    public void Construct(TowerViewModel vm, DragDropController dragDropController)
    {
        _dragDropController = dragDropController;
        _vm = vm;
    }

    private void OnEnable()
    {
        _vm.Cubes.ObserveCountChanged()
            .Subscribe(_ => RebuildFromVm())
            .AddTo(_cd);

        RebuildFromVm();
    }

    private void OnDisable()
    {
        _cd.Clear();
    }

    private void RebuildFromVm()
    {
        if (_container == null || _cubePrefab == null)
            return;

        for (int i = 0; i < _views.Count; i++)
            _views[i].Destroy();

        _views.Clear();

        for (int i = 0; i < _vm.Cubes.Count; i++)
        {
            var data = _vm.Cubes[i];
            var view = Instantiate(_cubePrefab, _container);

            view.Bind(_dragDropController, i, data);
            _views.Add(view);

            view.PlayPlaceAnimation();
        }
    }
}