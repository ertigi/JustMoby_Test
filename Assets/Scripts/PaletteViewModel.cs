using UniRx;

public sealed class PaletteViewModel
{
    public ReactiveCollection<CubeDescriptor> Cubes { get; } = new();
}
