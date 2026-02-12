using UnityEngine;
using UnityEngine.UI;

public sealed class DragVisualView : MonoBehaviour
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private Image _image;
    [SerializeField] private CanvasGroup _canvasGroup;


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
        Hide();
    }

    public void Hide() => _canvasGroup.alpha = 0f;
    public void Show() => _canvasGroup.alpha = 1f;
}
