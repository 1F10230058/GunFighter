using UnityEngine;
using UnityEngine.SceneManagement; // シーンの切り替えに必要

public class TitleManager : MonoBehaviour
{
    // スタートボタンが押された時に呼ばれる関数
    public void StartGame()
    {
        // "Field"という名前のシーンをロードする
        SceneManager.LoadScene("Field");
    }
}