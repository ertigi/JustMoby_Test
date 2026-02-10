using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public sealed class PaletteScrollView : MonoBehaviour
{
    [SerializeField] private RectTransform _content;
    [SerializeField] private PaletteCubeItemView _itemPrefab;

    private List<PaletteCubeItemView> _cubeItemViews = new();
    private PaletteViewModel _vm;

    private readonly CompositeDisposable _cd = new();

    [Inject]
    public void Construct(PaletteViewModel vm)
    {
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
            view.Bind(i, _vm.Cubes[i]);
            _cubeItemViews.Add(view);
        }
    }
}
