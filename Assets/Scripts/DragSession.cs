public sealed class DragSession
{
    public DragSourceType SourceType { get; }
    public CubeDescriptor Descriptor { get; }
    public int? TowerIndex { get; }

    public DragSession(DragSourceType sourceType, CubeDescriptor descriptor, int? towerIndex)
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