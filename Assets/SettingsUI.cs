using UnityEngine;
using UnityEngine.UI; // Sliderを使うために必要

public class SettingsUI : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;

    void Start()
    {
        // AudioManagerから現在の音量設定を取得してスライダーに反映
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // スライダーが動かされた時に、AudioManagerの関数を呼ぶように設定
        bgmSlider.onValueChanged.AddListener(AudioManager.Instance.SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);
    }
}