using UniRx;
using UnityEngine;
using Zenject;

public sealed class DragVisualHostView : MonoBehaviour
{
    [SerializeField] private DragVisualView _dragVisual;
    private DragDropController _dragDrop;
    private CompositeDisposable _cd = new();

    [Inject]
    public void Construct(DragDropController dragDrop)
    {
        _dragDrop = dragDrop;
    }

    private void Awake()
    {
        _dragDrop.RegisterDragVisual(_dragVisual);
    }

    private void Update()
    {
        if (!_dragDrop.IsDragging.Value)
            return;

        _dragVisual.FollowScreenPoint(Input.mousePosition);
    }

    private void OnDestroy()
    {
        _cd.Clear();
        if (_dragVisual != null)
            _dragDrop.UnregisterDragVisual(_dragVisual);
    }
}