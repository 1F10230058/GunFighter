using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // シーン遷移に必要
using TMPro;

public class BattleManager : MonoBehaviour
{
    public TextMeshProUGUI signalText;
    // 敵が「！」に反応するまでの時間（秒）
    public float enemyReactionTime = 0.5f;

    // ゲームの状態を管理するための「状態変数」
    private BattleState currentState;

    // ゲームの状態を定義する（お手つき、入力受付中、終了）
    private enum BattleState
    {
        Waiting,      // 「！」が表示される前の待機状態
        InputReady,   // 「！」が表示され、入力待ちの状態
        Finished      // 勝敗が決まり、終了した状態
    }

    void Start()
    {
        StartCoroutine(StartDuel());
    }

    // Updateは毎フレーム呼ばれる
    void Update()
    {
        // もしスペースキーが押されたら
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 待機状態（「！」表示前）に押した場合
            if (currentState == BattleState.Waiting)
            {
                // お手つきで負け
                EndDuel("早すぎ！");
            }
            // 入力受付中に押した場合
            else if (currentState == BattleState.InputReady)
            {
                // 成功！勝ち
                EndDuel("勝ち！");
            }
        }
    }

    IEnumerator StartDuel()
    {
        // 1. 状態を「待機中」に設定
        currentState = BattleState.Waiting;
        signalText.text = "Ready...";

        // 2. ランダムな時間だけ待つ
        float randomWaitTime = Random.Range(2.0f, 5.0f);
        yield return new WaitForSeconds(randomWaitTime);

        // 3. 状態を「入力受付中」にして、合図を表示
        currentState = BattleState.InputReady;
        signalText.text = "！";

        // 4. 敵の反応時間後に「遅すぎ！」の判定を行う
        yield return new WaitForSeconds(enemyReactionTime);

        // 5. この時点でまだ勝敗が決まっていなければ（プレイヤーが入力していなければ）
        if (currentState == BattleState.InputReady)
        {
            // 時間切れで負け
            EndDuel("遅すぎ！");
        }
    }

    // 勝敗が決まった時の処理
    void EndDuel(string resultMessage)
    {
        // 状態を「終了」にして、それ以上キー入力が反応しないようにする
        currentState = BattleState.Finished;

        // 結果を表示
        signalText.text = resultMessage;

        // 2秒後にシーンを切り替える（勝っても負けてもとりあえずFieldに戻る）
        // ここは後で、負けたらGameOverに行くように変更します。
        Invoke("ReturnToField", 2f);
    }

    void ReturnToField()
    {
        SceneManager.LoadScene("Field");
    }
}