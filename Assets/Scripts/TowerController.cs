using System.Collections;
using UnityEngine;

public class TowerController
{
    private readonly GameConfigSO _config;
    private readonly TowerViewModel _vm;
    private readonly TowerPlacementService _placement;
    private readonly TowerHeightLimitService _heightLimit;

    public RectTransform TowerArea { get; }
    public RectTransform StackContainer { get; }

    public TowerController(
        GameConfigSO config,
        TowerViewModel vm,
        TowerPlacementService placement,
        TowerHeightLimitService heightLimit,
        SceneReferences sceneReferences)
    {
        _config = config;
        _vm = vm;
        _placement = placement;
        _heightLimit = heightLimit;
        TowerArea = sceneReferences.TowerArea;
        StackContainer = sceneReferences.TowerStackContainer;
    }

    public bool TryPlaceFromPalette(CubeDescriptor descriptor, Vector2 screenPoint)
    {
        if (_vm.Cubes.Count > 0)
            return false;

        if (_vm.HeightLocked.Value)
            return false;

        if (!_placement.IsPointInsideArea(TowerArea, screenPoint))
            return false;

        if (!_heightLimit.CanAddNext(TowerArea, StackContainer, descriptor.Size.y))
        {
            LockHeight();
            return false;
        }

        var anchoredPos = _placement.GetFirstPlacement(TowerArea, screenPoint);

        var data = new TowerCubeData(descriptor, anchoredPos, descriptor.Size.x, descriptor.Size.y);
        _vm.Cubes.Add(data);

        return true;
    }

    public bool TryStackFromPalette(CubeDescriptor descriptor, Vector2 screenPoint)
    {
        if (_vm.HeightLocked.Value)
            return false;

        if (_vm.Cubes.Count == 0)
            return false;

        if (!_placement.IsPointOverExistingStack(TowerArea, _vm.Cubes[^1].AnchoredPosition, screenPoint, descriptor.Size))
            return false;

        if (!_heightLimit.CanAddNext(TowerArea, StackContainer, descriptor.Size.y))
        {
            LockHeight();
            return false;
        }

        var anchoredPos = _placement.GetStackPlacement(TowerArea, _vm.Cubes[^1].AnchoredPosition, screenPoint, descriptor.Size);

        var data = new TowerCubeData(descriptor, anchoredPos, descriptor.Size.x, descriptor.Size.y);
        _vm.Cubes.Add(data);

        return true;
    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= _vm.Cubes.Count)
            return;

        _vm.Cubes.RemoveAt(index);

        if (_vm.HeightLocked.Value)
            _vm.HeightLocked.Value = false;
    }

    private void LockHeight()
    {
        _vm.HeightLocked.Value = true;
    }
}
