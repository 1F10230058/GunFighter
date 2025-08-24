using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private AudioSource bgmSource;
    private AudioSource sfxSource;

    private const string BGM_VOLUME_KEY = "BGMVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";

    // --- ここから追加 ---
    [Header("最大音量設定")]
    [Range(0f, 1f)] // Inspectorで0から1のスライダーになる
    public float maxBgmVolume = 0.5f; // BGMの最大音量を50%に設定
    // --- ここまで追加 ---

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
            sfxSource = gameObject.AddComponent<AudioSource>();

            // 保存されたスライダーの位置をロード（なければ初期値1）
            float bgmSliderValue = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 1f);
            float sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);
            // スライダーの位置と最大音量を掛け合わせて適用
            SetBGMVolume(bgmSliderValue); 
            SetSFXVolume(sfxVolume);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource.clip == clip) return;
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    // BGMの音量を設定する関数
    public void SetBGMVolume(float sliderValue)
    {
        // --- ここを変更 ---
        // スライダーの値(0~1)と、設定した最大音量を掛け合わせる
        bgmSource.volume = sliderValue * maxBgmVolume;
        // --- ここまで変更 ---

        // スライダーの位置だけを保存
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, sliderValue);
        PlayerPrefs.Save();
    }

    // SFXの音量を設定する関数
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
        PlayerPrefs.Save();
    }
}