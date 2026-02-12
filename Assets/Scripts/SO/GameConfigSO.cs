using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Game/GameConfig", fileName = "GameConfig")]
public class GameConfigSO : ScriptableObject
{
    [field: SerializeField] public List<CubeColorConfig> PaletteColors { get; private set; } = new();
    [field: SerializeField] public float MaxHorizontalOffsetNormalized { get; private set; } = .5f;
    [field: SerializeField] public Vector2 CubeSize { get; private set; } = new(130, 130);
    [field: SerializeField] public float TowerAreaPadding { get; private set; } = 65;
}
