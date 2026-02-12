using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public sealed class TowerCubeView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private Image _image;
    [SerializeField] private CanvasGroup _canvasGroup;
    private DragDropController _dragDrop;
    private TowerCubeData _data;
    private Vector2 _startPos;
    private int _index;
    private bool _isDestroyed;

    public void Bind(DragDropController dragDrop, int index, TowerCubeData data)
    {
        _dragDrop = dragDrop;
        _index = index;
        _data = data;

        _image.sprite = data.Descriptor.Sprite;

        _rect.sizeDelta = data.Descriptor.Size;
        _rect.anchoredPosition = data.AnchoredPosition;
    }

    public void PlayPlaceAnimation()
    {
        if (_rect == null)
            return;

        _rect.DOKill();

        var target = _rect.anchoredPosition;
        _rect.anchoredPosition = target + Vector2.down * 15f;
        _rect.DOAnchorPos(target, 0.25f).SetEase(Ease.OutBack);
    }

    public void Destroy()
    {
        _isDestroyed = true;
        Destroy(gameObject);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 0;
        _startPos = _rect.anchoredPosition;
        _dragDrop.BeginDragFromTower(_data.Descriptor, _index);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _dragDrop.EndDrag(eventData.position);

        if (_isDestroyed)
            return;

        _canvasGroup.alpha = 1;
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rect.parent as RectTransform, eventData.position, eventData.pressEventCamera, out var local);

        _rect.DOKill();
        _rect.DOAnchorPos(_startPos, 0.15f).From(local).SetEase(Ease.OutCubic);

    }

    public void OnDrag(PointerEventData eventData) { }
}