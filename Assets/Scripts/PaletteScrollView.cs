using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public sealed class PaletteScrollView : MonoBehaviour
{
    [SerializeField] private RectTransform _content;
    [SerializeField] private PaletteCubeItemView _itemPrefab;
    private List<PaletteCubeItemView> _cubeItemViews = new();
    private CompositeDisposable _cd = new();
    private DragDropController _dragDropController;
    private PaletteViewModel _vm;


    [Inject]
    public void Construct(DragDropController dragDropController, PaletteViewModel vm)
    {
        _dragDropController = dragDropController;
        _vm = vm;
    }

    private void Awake()
    {
        _vm.Cubes.ObserveCountChanged()
            .Subscribe(_ => Rebuild())
            .AddTo(_cd);

        Rebuild();
    }

    private void OnDisable() => _cd.Clear();

    private void Rebuild()
    {
        for (int i = 0; i < _cubeItemViews.Count; i++)
            Destroy(_cubeItemViews[i].gameObject);

        _cubeItemViews.Clear();

        for (int i = 0; i < _vm.Cubes.Count; i++)
        {
            var view = Instantiate(_itemPrefab, _content);
            view.Bind(_dragDropController, i, _vm.Cubes[i]);
            _cubeItemViews.Add(view);
        }
    }
}
