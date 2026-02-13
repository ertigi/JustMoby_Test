using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public sealed class PaletteCubeItemView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private RectTransform _rect;
    private DragDropController _dragDrop;
    private CubeDescriptor _descriptor;

    public void Bind(DragDropController dragDrop, CubeDescriptor descriptor)
    {
        _dragDrop = dragDrop;
        _descriptor = descriptor;

        _image.sprite = descriptor.Sprite;
        _rect.sizeDelta = descriptor.Size;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _dragDrop.BeginDragFromPalette(_descriptor);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _dragDrop.EndDrag(eventData.position);
    }

    public void OnDrag(PointerEventData eventData) { }
}