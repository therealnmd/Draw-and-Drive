using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    public enum SliderType { Music, SFX }
    public SliderType sliderType;
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();

        if (sliderType == SliderType.Music)
        {
            float savedValue = PlayerPrefs.GetFloat("MusicVolume", 1f);
            slider.value = savedValue;
            AudioManager.Instance.MusicVolume(savedValue);
        }
        else if (sliderType == SliderType.SFX)
        {
            float savedValue = PlayerPrefs.GetFloat("SFXVolume", 1f);
            slider.value = savedValue;
            AudioManager.Instance.SFXVolume(savedValue);
        }

        // Lắng nghe khi slider thay đổi
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        if (sliderType == SliderType.Music)
        {
            AudioManager.Instance.MusicVolume(value);
        }
        else if (sliderType == SliderType.SFX)
        {
            AudioManager.Instance.SFXVolume(value);
        }
    }
}
