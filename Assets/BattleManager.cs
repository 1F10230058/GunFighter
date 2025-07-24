using System.Collections;
using UnityEngine;
using TMPro; // TextMeshProを扱うために必要

public class BattleManager : MonoBehaviour
{
    // Unityエディタからテキストをアタッチするための変数
    public TextMeshProUGUI signalText;

    // ゲームが始まった時に一度だけ呼ばれる
    void Start()
    {
        // 決闘開始のコルーチンを呼び出す
        StartCoroutine(StartDuel());
    }

    // 時間差で処理を行うためのコルーチン
    IEnumerator StartDuel()
    {
        // 1. 最初のテキストを設定
        signalText.text = "Ready...";

        // 2. ランダムな時間だけ待つ (2秒から5秒の間)
        float randomWaitTime = Random.Range(2.0f, 5.0f);
        yield return new WaitForSeconds(randomWaitTime);

        // 3. 合図の「！」を表示する
        signalText.text = "！";

        // ここに後で「プレイヤーの入力待ち」や「敵の反応」の処理を追加していく
    }
}