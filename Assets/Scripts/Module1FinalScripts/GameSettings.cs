using UnityEngine;

public static class GameSettings
{
    public static float MouseSensitivity { get; private set; } = PlayerPrefs.GetFloat("MouseSensitivity", 0.1f);
    public static float MasterVolume { get; private set; } = PlayerPrefs.GetFloat("MasterVolume", 1f);

    public static void SetMouseSensitivity(float value)
    {
        MouseSensitivity = Mathf.Clamp(value, 0.01f, 0.5f);
        PlayerPrefs.SetFloat("MouseSensitivity", MouseSensitivity);
    }

    public static void SetMasterVolume(float value)
    {
        MasterVolume = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat("MasterVolume", MasterVolume);
    }

    public static void Save()
    {
        PlayerPrefs.Save();
    }
}