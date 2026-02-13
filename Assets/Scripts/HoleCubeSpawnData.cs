using UnityEngine;

public sealed class HoleCubeSpawnData
{
    public CubeDescriptor Descriptor { get; }
    public Vector2 Size { get; }
    public Vector2 Position { get; }

    public HoleCubeSpawnData(CubeDescriptor descriptor, Vector2 size, Vector2 position)
    {
        Descriptor = descriptor;
        Size = size;
        Position = position;
    }
}