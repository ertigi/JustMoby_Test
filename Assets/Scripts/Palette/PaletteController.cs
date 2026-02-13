
using Unity.VisualScripting;

public sealed class PaletteController
{
    private readonly GameConfigSO _config;
    private readonly PaletteViewModel _vm;

    public PaletteController(GameConfigSO config, PaletteViewModel vm)
    {
        _config = config;
        _vm = vm;
    }

    public void BuildPalette()
    {
        PaletteCubeDescriptor[] descriptors = new PaletteCubeDescriptor[_config.PaletteColors.Count];

        for (int i = 0; i < _config.PaletteColors.Count; i++)
        {
            var descriptor = new PaletteCubeDescriptor(_config.PaletteColors[i].Sprite, _config.CubeSize);
            descriptors[i] = descriptor;
        }

        _vm.Cubes.AddRange(descriptors);
    }
}
