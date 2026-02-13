using UniRx;

public sealed class PaletteViewModel
{
    public ReactiveCollection<PaletteCubeDescriptor> Cubes { get; } = new();
}
