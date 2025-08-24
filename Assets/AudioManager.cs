using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // この AudioManager をどこからでも呼び出せるようにするための「印」
    public static AudioManager Instance { get; private set; }

    // BGM再生用の AudioSource
    private AudioSource bgmSource;
    // 効果音再生用の AudioSource
    private AudioSource sfxSource;

    // PlayerPrefsに保存するときのキー
    private const string BGM_VOLUME_KEY = "BGMVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";

    // ゲームが起動した時に一度だけ呼ばれる
    void Awake()
    {
        // もし他に AudioManager が存在しなければ、自分を「印」として設定
        if (Instance == null)
        {
            Instance = this;
            // シーンを切り替えてもこのオブジェクトが破壊されないようにする
            DontDestroyOnLoad(gameObject);

            // AudioSourceコンポーネントを2つ追加して、それぞれ設定
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true; // BGMはループ再生する
            sfxSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            // すでに AudioManager が存在する場合は、自分を破壊する
            Destroy(gameObject);
        }
    }

    // BGMを再生する関数
    public void PlayBGM(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    // 効果音を再生する関数
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
    // BGMの音量を設定する関数
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
        // 設定を保存
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, volume);
        PlayerPrefs.Save();
    }

    // SFXの音量を設定する関数
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
        // 設定を保存
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
        PlayerPrefs.Save();
    }
}