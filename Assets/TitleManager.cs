using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // コルーチンを使うために必要

public class TitleManager : MonoBehaviour
{
    // ボタンから呼ばれるのはこちらの関数
    public void StartGame()
    {
        // 実際の処理はコルーチンに任せる
        StartCoroutine(StartGameWithSound());
    }

    // 音を鳴らして、少し待ってからシーンを切り替えるコルーチン
    private IEnumerator StartGameWithSound()
    {
        // 新しいゲームを始める前に、前回のゲームの記録をリセットする
        GameData.returnedFromBattle = false;
        GameData.defeatedEnemyIds.Clear();

        // クリック音の再生
        if (AudioManager.Instance != null && AudioManager.Instance.clickSound != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.clickSound);
        }

        // 音が鳴るのを少し待つ
        yield return new WaitForSeconds(0.1f);

        // "Field"シーンをロードする
        SceneManager.LoadScene("Field");
    }
}