using UnityEngine;

public sealed class CubeDescriptor
{
    public int Id { get; }
    public Sprite Sprite { get; }
    public Vector2 Size { get; }

    public CubeDescriptor(int id, Sprite sprite, Vector2 size)
    {
        Id = id;
        Sprite = sprite;
        Size = size;
    }
}