using System;
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

        _vm.Cubes.Add(new(descriptor, _config.CubeSize, anchoredPos, 0));

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

        if (!_placement.IsPointOverExistingStackForPaletteCube(TowerArea, _vm.Cubes[^1].Position.Value, screenPoint, descriptor.Size))
            return false;

        if (!_heightLimit.IsStackedAboveLast(TowerArea, _vm.Cubes[^1].Position.Value, screenPoint, descriptor.Size))
        {
            _messageController.Enqueue(LocalizationMessageKey.CubeMissed);
            return false;
        }

        if (!_heightLimit.IsWithinHeightLimit(TowerArea, _vm.Cubes[^1].Position.Value, descriptor.Size))
        {
            LockHeight();
            return false;
        }

        var anchoredPos = _placement.GetStackPlacementForPalette(TowerArea, _vm.Cubes[^1].Position.Value, screenPoint, descriptor.Size);

        _vm.Cubes.Add(new(descriptor, _config.CubeSize, anchoredPos, _vm.Cubes.Count));

        _messageController.Enqueue(LocalizationMessageKey.CubePlaced);

        return true;
    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= _vm.Cubes.Count)
            return;

        _vm.Cubes.RemoveAt(index);
        _messageController.Enqueue(LocalizationMessageKey.CubeTrashed);

        if (index != 0 && index < _vm.Cubes.Count)
            ReflowFrom(index);

        SetNewIndexes();

        if (_vm.HeightLocked.Value)
            _vm.HeightLocked.Value = false;
    }

    private void ReflowFrom(int startIndex)
    {
        if (_vm.Cubes.Count == 0)
            return;

        Vector2 size = _config.CubeSize;

        int i = startIndex;
        while (i < _vm.Cubes.Count)
        {
            var below = _vm.Cubes[i - 1];
            var current = _vm.Cubes[i];

            float targetY = below.Position.Value.y + size.y;

            float supportX = below.Position.Value.x;
            float targetX = current.Position.Value.x;

            float maxOffset = size.x * _config.MaxHorizontalOffsetNormalized;
            float minSupportX = supportX - maxOffset;
            float maxSupportX = supportX + maxOffset;

            if (targetX < minSupportX || targetX > maxSupportX)
            {
                _vm.Cubes.RemoveAt(i);
                continue;
            }
            
            current.Position.Value = new Vector2(targetX, targetY);
            i++;
        }
    }
    
    private void SetNewIndexes()
    {
        for (int i = 0; i < _vm.Cubes.Count; i++)
        {
            _vm.Cubes[i].SetNewIndex(i);
        }
    }

    private void LockHeight()
    {
        _messageController.Enqueue(LocalizationMessageKey.HeightLimitReached);
        _vm.HeightLocked.Value = true;
    }
}
