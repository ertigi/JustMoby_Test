using System;
using UnityEngine;

[Serializable]
public class CubeColorConfig
{
    [field: SerializeField] public string CubeId { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
}