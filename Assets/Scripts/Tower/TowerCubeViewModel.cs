using System;
using UniRx;
using UnityEngine;

public sealed class TowerCubeViewModel
{
    public PaletteCubeDescriptor Descriptor { get; }
    public Vector2 Size { get; }
    public ReactiveProperty<Vector2> Position { get; }
    public int Index => _index;
    private int _index;

    public TowerCubeViewModel(PaletteCubeDescriptor descriptor, Vector2 size, Vector2 startPosition, int index)
    {
        Descriptor = descriptor;
        Size = size;
        Position = new ReactiveProperty<Vector2>(startPosition);
        _index = index;
    }

    public void SetNewIndex(int i)
    {
        _index = i;
    }
}