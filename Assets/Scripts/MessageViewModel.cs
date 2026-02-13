using UniRx;

public sealed class MessageViewModel
{
    public ReactiveProperty<string> Text { get; } = new(string.Empty);
    public ReactiveProperty<bool> Visible { get; } = new(false);
}
