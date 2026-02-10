using UnityEngine;

public sealed class CubeDescriptor
    {
        public int Id { get; }
        public Sprite Sprite { get; }

        public CubeDescriptor(int id, Sprite sprite)
        {
            Id = id;
            Sprite = sprite;
        }
    }