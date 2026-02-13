using UniRx;

public sealed class HoleViewModel
{
    public Subject<HoleCubeSpawnData> OnCubeSpawnRequested { get; } = new();
}
