using UnityEngine;
using UnityEditor; // MenuItemを使うために必要

public class DebugTools
{
    // Unityの上部メニューに "Tools/Reset Save Data" を追加する
    [MenuItem("Tools/Reset Save Data")]
    private static void ResetSaveData()
    {
        // PlayerPrefsに保存された全てのデータを削除する
        PlayerPrefs.DeleteAll();
        // 確認用のメッセージをコンソールに表示
        Debug.Log("全てのセーブデータをリセットしました。");
    }
}