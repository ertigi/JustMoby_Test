public sealed class DragSession
{
    public DragSourceType SourceType { get; }
    public PaletteCubeDescriptor Descriptor { get; }
    public int? TowerIndex { get; }

    public DragSession(DragSourceType sourceType, PaletteCubeDescriptor descriptor, int? towerIndex)
    {
        SourceType = sourceType;
        Descriptor = descriptor;
        TowerIndex = towerIndex;
    }
}

public enum DragSourceType
{
    Palette,
    Tower
}