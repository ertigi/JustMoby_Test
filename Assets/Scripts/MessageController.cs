using System.Collections.Generic;
using UniRx;

public sealed class MessageController
{
    private MessageViewModel _vm;
    private LocalizationService _localizationService;
    private Queue<string> _queue = new();
    private CompositeDisposable _cd = new();
    private float _durationSeconds;
    private bool _isShowing;

    public MessageController(MessageViewModel vm, LocalizationService localizationService, GameConfigSO gameConfigSO)
    {
        _vm = vm;
        _localizationService = localizationService;
        _durationSeconds = gameConfigSO.MessageDuration;
    }

    public void Enqueue(LocalizationMessageKey key)
    {
        var text = _localizationService.Get(key);

        if (string.IsNullOrWhiteSpace(text))
            return;

        _queue.Enqueue(text);
        TryShowNext();
    }

    private void TryShowNext()
    {
        if (_isShowing)
            return;
        if (_queue.Count == 0)
            return;

        _isShowing = true;

        var msg = _queue.Dequeue();
        _vm.Text.Value = msg;
        _vm.Visible.Value = true;

        Observable.Timer(System.TimeSpan.FromSeconds(_durationSeconds))
            .Subscribe(_ =>
            {
                _vm.Visible.Value = false;
                _isShowing = false;
                TryShowNext();
            })
            .AddTo(_cd);
    }
}
