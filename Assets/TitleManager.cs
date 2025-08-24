using UnityEngine;
using UnityEngine.SceneManagement; // シーンの切り替えに必要

public class TitleManager : MonoBehaviour
{
    // スタートボタンが押された時に呼ばれる関数
    public void StartGame()
    {
        // 新しいゲームを始める前に、前回のゲームの記録をリセットする
        GameData.returnedFromBattle = false;
        GameData.defeatedEnemyIds.Clear(); // 討伐した敵のリストも空にする
        // "Field"という名前のシーンをロードする
        SceneManager.LoadScene("Field");
    }
}