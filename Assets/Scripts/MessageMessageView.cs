using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

public sealed class MessageMessageView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private CanvasGroup _group;

    private CompositeDisposable _cd = new();
    private MessageViewModel _vm;
    private float _fadeDuration;

    [Inject]
    public void Construct(MessageViewModel vm, GameConfigSO gameConfigSO)
    {
        _vm = vm;
        _fadeDuration = gameConfigSO.MessageFadeDuration;
    }

    private void Awake()
    {
        _group.alpha = 0f;
        _text.text = string.Empty;
    }

    private void OnEnable()
    {
        _vm.Text
            .Subscribe(msg => 
            { 
                _text.text = msg; 
            })
            .AddTo(_cd);

        _vm.Visible
            .DistinctUntilChanged()
            .Subscribe(visible =>
            {
                _group.DOKill();
                _group.DOFade(visible ? 1f : 0f, _fadeDuration);
            })
            .AddTo(_cd);
    }

    private void OnDisable() => _cd.Clear();
}