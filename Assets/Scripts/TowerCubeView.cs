using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public sealed class TowerCubeView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private Image _image;

    private CompositeDisposable _cd = new();
    private TowerCubeViewModel _towerCubeViewModel;
    private DragDropController _dragDrop;
    private Vector2 _startPos;
    private bool _isUnbind;

    public void Bind(DragDropController dragDrop, TowerCubeViewModel towerCubeViewModel)
    {
        _towerCubeViewModel = towerCubeViewModel;
        _dragDrop = dragDrop;

        _image.sprite = _towerCubeViewModel.Descriptor.Sprite;

        _rect.sizeDelta = _towerCubeViewModel.Descriptor.Size;

        _towerCubeViewModel.Position
            .DistinctUntilChanged()
            .Subscribe(pos =>
            {
                if (_rect == null)
                    return;

                _rect.DOKill();
                _rect.DOAnchorPos(pos, 0.25f).SetEase(Ease.OutCubic);
            })
            .AddTo(_cd);

        _rect.anchoredPosition = _towerCubeViewModel.Position.Value;
    }

    public void Unbind()
    {
        _cd.Clear();
        _towerCubeViewModel = null;
        _isUnbind = true;

        if (_rect != null)
            _rect.DOKill();
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        _startPos = _rect.anchoredPosition;
        _dragDrop.BeginDragFromTower(_towerCubeViewModel.Descriptor, _towerCubeViewModel.Index);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _dragDrop.EndDrag(eventData.position);

        if (_isUnbind)
            return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rect.parent as RectTransform, eventData.position, eventData.pressEventCamera, out var local);

        _rect.DOKill();
        _rect.DOAnchorPos(_startPos, 0.15f).From(local).SetEase(Ease.OutCubic);

    }

    public void OnDrag(PointerEventData eventData) { }
}