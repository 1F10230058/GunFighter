using UnityEngine;

public class SceneBGM : MonoBehaviour
{
    // InspectorからBGMのオーディオクリップを設定
    public AudioClip bgmClip;

    void Start()
    {
        // AudioManagerに、このシーンのBGMを再生するようお願いする
        AudioManager.Instance.PlayBGM(bgmClip);
    }
}