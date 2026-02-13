using UnityEngine;

public sealed class PaletteCubeDescriptor
{
    public Sprite Sprite { get; }
    public Vector2 Size { get; }

    public PaletteCubeDescriptor(Sprite sprite, Vector2 size)
    {
        Sprite = sprite;
        Size = size;
    }
}