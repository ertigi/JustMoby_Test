using DG.Tweening;
using UniRx;
using UnityEngine;

public sealed class DragDropController
{
    private ReactiveProperty<bool> _isDragging = new(false);
    public IReadOnlyReactiveProperty<bool> IsDragging => _isDragging;
    private DragVisualView _dragVisual;
    private TowerController _towerController;
    private HoleController _holeController;
    private MessageController _messageController;
    private DragSession _currentDragSession;

    public DragDropController(TowerController towerController, HoleController holeController, MessageController messageController)
    {
        _towerController = towerController;
        _holeController = holeController;
        _messageController = messageController;
    }

    public void RegisterDragVisual(DragVisualView dragVisual)
    {
        _dragVisual = dragVisual;
    }

    public void UnregisterDragVisual(DragVisualView dragVisual)
    {
        if (_dragVisual == dragVisual)
            _dragVisual = null;
    }

    public void BeginDragFromPalette(CubeDescriptor descriptor)
    {
        if (descriptor == null)
            return;

        _currentDragSession = new(DragSourceType.Palette, descriptor, null);
        _isDragging.Value = true;

        _dragVisual.SetSprite(descriptor.Sprite);
        _dragVisual.SetSize(descriptor.Size);
    }

    public void BeginDragFromTower(CubeDescriptor descriptor, int towerIndex)
    {
        if (descriptor == null)
            return;

        _currentDragSession = new(DragSourceType.Tower, descriptor, towerIndex);
        _isDragging.Value = true;

        if (_dragVisual != null)
        {
            _dragVisual.SetSprite(descriptor.Sprite);
            _dragVisual.SetSize(descriptor.Size);
        }
    }

    public void EndDrag(Vector2 screenPoint)
    {
        if (!_isDragging.Value)
            return;

        bool success = false;

        if (_currentDragSession.SourceType == DragSourceType.Palette)
        {
            if (_towerController.TryPlaceFromPalette(_currentDragSession.Descriptor, screenPoint))
                success = true;
            else if (_towerController.TryStackFromPalette(_currentDragSession.Descriptor, screenPoint))
                success = true;

            if (success)
            {   
                _dragVisual.Hide();
            }
            else
            {
                _dragVisual.PlayMissAndHide();
                _messageController.Enqueue(LocalizationMessageKey.CubeMissed);
            }
        }
        else
        {
            if (_holeController.IsDroppedIntoHole(screenPoint))
            {
                _towerController.RemoveAt(_currentDragSession.TowerIndex.Value);
            }

            _dragVisual.Hide();
        }

        _currentDragSession = null;
        _isDragging.Value = false;
    }
}
