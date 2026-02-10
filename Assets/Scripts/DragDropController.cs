using DG.Tweening;
using UniRx;

public sealed class DragDropController
{
    private ReactiveProperty<bool> _isDragging = new(false);
    public IReadOnlyReactiveProperty<bool> IsDragging => _isDragging;
    private DragVisualView _dragVisual;

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

        _isDragging.Value = true;

        _dragVisual.SetSprite(descriptor.Sprite);
        _dragVisual.SetSize(descriptor.Size);
    }

    public void EndDrag()
    {
        if (!_isDragging.Value)
            return;

        bool success = true;

        if (success)
        {
            _dragVisual?.Hide();
        }
        else
        {
            _dragVisual?.PlayMissAndHide();
        }

        _isDragging.Value = false;
    }
}
