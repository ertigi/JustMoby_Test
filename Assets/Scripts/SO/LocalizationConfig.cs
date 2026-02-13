using UnityEngine;

[CreateAssetMenu(menuName = "Game/LocalizationConfig", fileName = "LocalizationConfig")]
public class LocalizationConfig : ScriptableObject
{
    [field: SerializeField] public string DefaultLang { get; private set; } = "ru";
    [field: SerializeField] public string RuPath { get; private set; } = "Localization/messages_ru";
    [field: SerializeField] public string EnPath { get; private set; } = "Localization/messages_en";
}