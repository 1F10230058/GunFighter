using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup;

    void Start()
    {
        Time.timeScale = 1f;
        StartCoroutine(FadeIn());
    }

    // ボタンに設定されているこの関数を、コルーチンを呼び出すだけの役割に変更
    public void ReturnToTitle()
    {
        StartCoroutine(ReturnToTitleWithSound());
    }

    // 音を鳴らして、少し待ってからシーンを切り替えるコルーチン
    private IEnumerator ReturnToTitleWithSound()
    {
        // AudioManagerの司令塔を呼び出して、登録されているクリック音を再生
        if (AudioManager.Instance != null && AudioManager.Instance.clickSound != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.clickSound);
        }

        // 音が鳴り終わるのを少しだけ待つ
        yield return new WaitForSeconds(0.1f);

        // Titleシーンをロード
        SceneManager.LoadScene("Title");
    }

    private IEnumerator FadeIn()
    {
        fadeCanvasGroup.alpha = 1;
        while(fadeCanvasGroup.alpha > 0)
        {
            fadeCanvasGroup.alpha -= Time.deltaTime;
            yield return null;
        }
        fadeCanvasGroup.blocksRaycasts = false;
    }
}