using UnityEngine;

public sealed class SceneReferences : MonoBehaviour
{
    [field: SerializeField] public RectTransform TowerArea { get; private set; }
    [field: SerializeField] public RectTransform TowerStackContainer { get; private set; }
    [field: SerializeField] public RectTransform HoleRect { get; private set; }
    [field: SerializeField] public RectTransform HoleMaskRect { get; private set; }
}