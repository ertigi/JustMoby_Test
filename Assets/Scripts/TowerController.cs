using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public sealed class TowerController
{
    private readonly GameConfigSO _config;
    private readonly TowerViewModel _vm;
    private readonly TowerPlacementService _placement;
    private readonly TowerHeightLimitService _heightLimit;
    private readonly MessageController _messageController;

    public RectTransform TowerArea { get; }
    public RectTransform StackContainer { get; }

    public TowerController(
        GameConfigSO config,
        TowerViewModel vm,
        TowerPlacementService placement,
        TowerHeightLimitService heightLimit,
        SceneReferences sceneReferences,
        MessageController messageController)
    {
        _config = config;
        _vm = vm;
        _placement = placement;
        _heightLimit = heightLimit;
        _messageController = messageController;
        TowerArea = sceneReferences.TowerArea;
        StackContainer = sceneReferences.TowerStackContainer;
    }

    public bool TryPlaceFromPalette(CubeDescriptor descriptor, Vector2 screenPoint)
    {
        if (_vm.Cubes.Count > 0)
            return false;

        if (_vm.HeightLocked.Value)
        {
            _messageController.Enqueue(LocalizationMessageKey.HeightLimitReached);
            return false;
        }

        if (!_placement.IsPointInsideArea(TowerArea, screenPoint))
            return false;

        var anchoredPos = _placement.GetFirstPlacement(TowerArea, screenPoint);

        var data = new TowerCubeData(descriptor, anchoredPos, descriptor.Size.x, descriptor.Size.y);
        _vm.Cubes.Add(data);

        _messageController.Enqueue(LocalizationMessageKey.CubePlaced);

        return true;
    }

    public bool TryStackFromPalette(CubeDescriptor descriptor, Vector2 screenPoint)
    {
        if (_vm.HeightLocked.Value)
        {
            _messageController.Enqueue(LocalizationMessageKey.HeightLimitReached);
            return false;
        }

        if (_vm.Cubes.Count == 0)
            return false;

        if (!_placement.IsPointOverExistingStackForPaletteCube(TowerArea, _vm.Cubes[^1].AnchoredPosition, screenPoint, descriptor.Size))
            return false;

        if(!_heightLimit.IsStackedAboveLast(TowerArea, _vm.Cubes[^1].AnchoredPosition, screenPoint, descriptor.Size))
        {
            _messageController.Enqueue(LocalizationMessageKey.CubeMissed);
            return false;
        }

        if (!_heightLimit.IsWithinHeightLimit(TowerArea, _vm.Cubes[^1].AnchoredPosition, descriptor.Size))
        {
            LockHeight();
            return false;
        }

        var anchoredPos = _placement.GetStackPlacementForPalette(TowerArea, _vm.Cubes[^1].AnchoredPosition, screenPoint, descriptor.Size);

        var data = new TowerCubeData(descriptor, anchoredPos, descriptor.Size.x, descriptor.Size.y);
        _vm.Cubes.Add(data);

        _messageController.Enqueue(LocalizationMessageKey.CubePlaced);

        return true;
    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= _vm.Cubes.Count)
            return;

        _vm.Cubes.RemoveAt(index);

        _messageController.Enqueue(LocalizationMessageKey.CubeTrashed);

        if (index > 0 && index < _vm.Cubes.Count)
        {
            RebuildTower(index);
        }

        if (_vm.HeightLocked.Value)
            _vm.HeightLocked.Value = false;
    }

    private void RebuildTower(int removedIndex)
    {
        List<TowerCubeData> rebuildDatas = new();

        for (int i = 0; i < _vm.Cubes.Count; i++)
        {
            if (i < removedIndex)
            {
                rebuildDatas.Add(_vm.Cubes[i]);
                continue;
            }

            if (!_placement.IsPointOverExistingStackForTowerCube(rebuildDatas[^1].AnchoredPosition, _vm.Cubes[i].AnchoredPosition, _config.CubeSize))
            {
                _vm.Cubes.RemoveAt(i);
                i--;
            }
            else
            {
                var anchoredPos = _placement.GetStackPlacementForTower(rebuildDatas[^1].AnchoredPosition, _vm.Cubes[i].AnchoredPosition, _config.CubeSize);
                rebuildDatas.Add(new TowerCubeData(_vm.Cubes[i].Descriptor, anchoredPos, _config.CubeSize.x, _config.CubeSize.y));
            }
        }

        _vm.Cubes.Clear();
        _vm.Cubes.AddRange(rebuildDatas);
    }

    private void LockHeight()
    {
        _messageController.Enqueue(LocalizationMessageKey.HeightLimitReached);
        _vm.HeightLocked.Value = true;
    }
}
