using UniRx;

public sealed class TowerViewModel
{
    public ReactiveCollection<TowerCubeData> Cubes { get; } = new();
    public ReactiveProperty<bool> HeightLocked { get; } = new(false);
}
