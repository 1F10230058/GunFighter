using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // コルーチンを使うために必要

public class GameOverManager : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup; // InspectorからFadeImageをセット

    // シーンが始まった時に一度だけ呼ばれる
    void Start()
    {
        // フェードインを開始
        StartCoroutine(FadeIn());
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene("Title");
    }

    // 徐々に明るくするコルーチン
    private IEnumerator FadeIn()
    {
        fadeCanvasGroup.alpha = 1;
        while(fadeCanvasGroup.alpha > 0)
        {
            fadeCanvasGroup.alpha -= Time.deltaTime;
            yield return null;
        }
    }
}