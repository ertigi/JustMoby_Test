using UnityEngine;

public sealed class TowerCubeData
{
    public CubeDescriptor Descriptor { get; }
    public Vector2 AnchoredPosition { get; set; }
    public float Width { get; }
    public float Height { get; }

    public TowerCubeData(CubeDescriptor descriptor, Vector2 anchoredPosition, float width, float height)
    {
        Descriptor = descriptor;
        AnchoredPosition = anchoredPosition;
        Width = width;
        Height = height;
    }
}
