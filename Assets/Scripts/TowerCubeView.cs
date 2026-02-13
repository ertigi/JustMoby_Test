using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public sealed class TowerCubeView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private Image _image;
    [SerializeField] private CanvasGroup _canvasGroup;

    private CompositeDisposable _cd = new();
    private TowerCubeViewModel _towerCubeViewModel;
    private DragDropController _dragDrop;
    private Vector2 _startPos;
    private bool _isUnbind;
    private bool _isInitialized;

    public void Bind(DragDropController dragDrop, TowerCubeViewModel towerCubeViewModel)
    {
        _canvasGroup.alpha = 1;
        _towerCubeViewModel = towerCubeViewModel;
        _dragDrop = dragDrop;

        _image.sprite = _towerCubeViewModel.Descriptor.Sprite;

        _rect.sizeDelta = _towerCubeViewModel.Descriptor.Size;

        _isInitialized = false;

        _towerCubeViewModel.Position
            .DistinctUntilChanged()
            .Subscribe(pos =>
            {
                PlayPlaceAnimation(pos);
            })
            .AddTo(_cd);

        _rect.anchoredPosition = _towerCubeViewModel.Position.Value;
    }

    public void Unbind()
    {
        _cd.Clear();
        _towerCubeViewModel = null;
        _isUnbind = true;
        _rect.DOKill();
    }

    public void PlayPlaceAnimation(Vector2 position)
    {
        if (!_isInitialized)
        {
            _rect.anchoredPosition = position;
            _isInitialized = true;
            return;
        }

        _rect.DOKill();
        _rect.DOAnchorPos(position, 0.25f).SetEase(Ease.OutCubic);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 0;
        _startPos = _rect.anchoredPosition;
        _dragDrop.BeginDragFromTower(_towerCubeViewModel.Descriptor, _towerCubeViewModel.Index);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 1;
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