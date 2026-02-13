using UniRx;

public sealed class TowerViewModel
{
    public ReactiveCollection<TowerCubeViewModel> Cubes { get; } = new();
    public ReactiveProperty<bool> HeightLocked { get; } = new(false);
}
