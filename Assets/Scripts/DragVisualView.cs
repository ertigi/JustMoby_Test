using UnityEngine;
using UnityEngine.UI;

public sealed class DragVisualView : MonoBehaviour
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private Image _image;
    [SerializeField] private CanvasGroup _group;

    private void Awake()
    {
        Hide();
    }

    public void SetSprite(Sprite sprite)
    {
        _image.sprite = sprite;
    }

    public void SetSize(Vector2 size)
    {
        _rect.sizeDelta = size;
    }

    public void FollowScreenPoint(Vector2 screenPoint)
    {
        _rect.anchoredPosition = screenPoint;
    }

    public void PlayMissAndHide()
    {
        
    }

    public void Hide() => _group.alpha = 0f;
    public void Show() => _group.alpha = 1f;
}
