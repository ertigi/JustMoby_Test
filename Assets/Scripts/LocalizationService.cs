using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class LocalizationService
{
    private const string PlayerPrefsLangKey = "lang"; // "ru" | "en"

    private Dictionary<string, string> _dict = new();
    private LocalizationConfig _config;

    public LocalizationService(LocalizationConfig config)
    {
        _config = config;
        var lang = ResolveLanguageCode();
        Load(lang);
    }

    public string Get(LocalizationMessageKey key) => Get(key.ToString());

    public string Get(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return string.Empty;

        return _dict.TryGetValue(key, out var value) ? value : key;
    }

    private void Load(string lang)
    {
        _dict.Clear();

        MergeFromJsonResource(_config.RuPath);

        if (!string.Equals(lang, "ru", StringComparison.OrdinalIgnoreCase))
        {
            if (string.Equals(lang, "en", StringComparison.OrdinalIgnoreCase))
                MergeFromJsonResource(_config.EnPath);
        }
    }

    private void MergeFromJsonResource(string resourcePath)
    {
        var asset = Resources.Load<TextAsset>(resourcePath);
        if (asset == null)
        {
            Debug.LogWarning($"[Localization] Missing resource: {resourcePath}.json");
            return;
        }

        var parsed = ParseDictionary(asset.text);
        foreach (var kv in parsed)
            _dict[kv.Key] = kv.Value;
    }

    private Dictionary<string, string> ParseDictionary(string json)
    {
        var wrapper = JsonUtility.FromJson<LocalizationDictionaryWrapper>(json);
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        if (wrapper?.items == null)
            return result;

        foreach (var item in wrapper.items)
        {
            if (string.IsNullOrWhiteSpace(item.key))
                continue;

            result[item.key] = item.value ?? string.Empty;
        }

        return result;
    }

    private string ResolveLanguageCode()
    {
        var saved = PlayerPrefs.GetString(PlayerPrefsLangKey, string.Empty);
        if (!string.IsNullOrWhiteSpace(saved))
            return saved.Trim().ToLowerInvariant();

        return _config.DefaultLang;
    }

    [Serializable]
    private class LocalizationDictionaryWrapper
    {
        public LocalizationItem[] items;
    }

    [Serializable]
    private class LocalizationItem
    {
        public string key;
        public string value;
    }
}
