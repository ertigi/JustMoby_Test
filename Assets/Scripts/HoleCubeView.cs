using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public sealed class HoleCubeView : MonoBehaviour
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private Image _image;
    [SerializeField] private AnimationCurve _animationCurveY;
    [SerializeField] private AnimationCurve _animationCurveX;
    [SerializeField] private float _curveYForce = 300;
    [SerializeField] private float _animationDuration = .5f;

    public Subject<Unit> OnAnimationCompleted { get; } = new();
    private Vector2 _startPosition;

    public void Init(HoleCubeSpawnData data)
    {
        _image.sprite = data.Descriptor.Sprite;
        _rect.sizeDelta = data.Size;
        _rect.anchoredPosition = data.Position;
        _startPosition = data.Position;

        PlayAnimation();
    }

    public void ResetState()
    {
        _rect.localScale = Vector3.one;
    }

    private void PlayAnimation()
    {
        _rect.localScale = Vector3.one;

        _rect.DORotate(new(0, 0, 360), _animationDuration, RotateMode.FastBeyond360);

        var tween = DOVirtual.Float(0f, 1f, _animationDuration, t =>
        {
            var x = Mathf.Lerp(_startPosition.x, 0f, _animationCurveX.Evaluate(t));
            var y = _curveYForce * _animationCurveY.Evaluate(t);

            _rect.anchoredPosition = new(x, y);
        })
        .OnComplete(() =>
        {
            OnAnimationCompleted.OnNext(Unit.Default);
        });
    }
}