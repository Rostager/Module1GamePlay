using UnityEngine;
using UnityEngine.UI;

public sealed class OptionsMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject previousMenu;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Slider volumeSlider;

    private void OnEnable()
    {
        sensitivitySlider.SetValueWithoutNotify(GameSettings.MouseSensitivity);
        volumeSlider.SetValueWithoutNotify(GameSettings.MasterVolume);
    }

    public void Open()
    {
        previousMenu.SetActive(false);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        GameSettings.Save();

        previousMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ChangeSensitivity(float value)
    {
        GameSettings.SetMouseSensitivity(value);
    }

    public void ChangeVolume(float value)
    {
        GameSettings.SetMasterVolume(value);
    }
}