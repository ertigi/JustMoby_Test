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

    private void OnEnable()
    {
        if (_dragVisual != null)
            _dragDrop.RegisterDragVisual(_dragVisual);

        _dragDrop.IsDragging
            .DistinctUntilChanged()
            .Subscribe(isDragging =>
            {
                if (isDragging)
                    _dragVisual?.Show();
                else
                    _dragVisual?.Hide();
            })
            .AddTo(_cd);
    }

    private void Update()
    {
        if (!_dragDrop.IsDragging.Value)
            return;

        _dragVisual.FollowScreenPoint(Input.mousePosition);
    }

    private void OnDisable()
    {
        _cd.Clear();
        if (_dragVisual != null)
            _dragDrop.UnregisterDragVisual(_dragVisual);
    }
}