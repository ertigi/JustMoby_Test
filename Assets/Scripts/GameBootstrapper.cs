using Zenject;

public sealed class GameBootstrapper : IInitializable
{
    private readonly PaletteController _palette;

    public GameBootstrapper(PaletteController palette)
    {
        _palette = palette;
    }

    public void Initialize()
    {
        _palette.BuildPalette();
    }
}
