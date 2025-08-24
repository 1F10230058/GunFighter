using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public TextMeshProUGUI signalText;
    public SpriteRenderer enemyDisplay;
    public SpriteRenderer playerDisplay;
    public float characterScale = 2f;

    private BattleState currentState;
    private int currentWins = 0; // 現在の勝利数をカウントする変数

    private enum BattleState
    {
        Waiting,
        InputReady,
        Finished
    }

    void Start()
    {
        // 位置とサイズの設定（変更なし）
        if (playerDisplay != null)
        {
            playerDisplay.transform.position = new Vector3(-3f, 0, 0);
            playerDisplay.transform.localScale = new Vector3(characterScale, characterScale, 1f);
        }
        if (enemyDisplay != null)
        {
            enemyDisplay.transform.position = new Vector3(3f, 0, 0);
                // 敵ごとのサイズ倍率を考慮した最終的な大きさを計算する
            float finalEnemyScale = characterScale * GameData.currentEnemyBattleScaleMultiplier;
            enemyDisplay.transform.localScale = new Vector3(finalEnemyScale, finalEnemyScale, 1f);
        }

        // スプライトの設定（変更なし）
        if (GameData.currentPlayerSprite != null) playerDisplay.sprite = GameData.currentPlayerSprite;
        if (GameData.currentEnemySprite != null) enemyDisplay.sprite = GameData.currentEnemySprite;

        // 最初のラウンドを開始
        StartCoroutine(StartRound());
    }

    void Update()
    {
        // Update内の処理は変更なし
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentState == BattleState.Waiting)
            {
                if (signalText.text == "?")
                {
                    EndDuel("早すぎ！", false);
                }
                else
                {
                    EndDuel("早すぎ！", false);
                }
            }
            else if (currentState == BattleState.InputReady)
            {
                // 1ラウンド勝利
                RoundWin();
            }
        }
    }

    // --- ここからが新しい処理 ---
    // 1ラウンド勝利した時の処理
    void RoundWin()
    {
        currentState = BattleState.Finished; // 入力を受け付けなくする
        currentWins++; // 勝利数を1増やす
        signalText.text = "勝ち！ (" + currentWins + "/" + GameData.currentEnemyRequiredWins + ")"; // 勝利数を表示

        // もし、勝利数が規定回数に達したら
        if (currentWins >= GameData.currentEnemyRequiredWins)
        {
            // 完全勝利として戦闘を終了する
            EndDuel("討伐成功！", true);
        }
        else
        {
            // まだ規定回数に達していなければ、2秒後に次のラウンドへ
            Invoke("StartNextRound", 2f);
        }
    }
    
    // 次のラウンドを開始するための関数
    void StartNextRound()
    {
        StartCoroutine(StartRound());
    }
    // --- ここまでが新しい処理 ---

    // StartDuelの名前をStartRoundに変更
    IEnumerator StartRound()
    {
        currentState = BattleState.Waiting;
        signalText.text = "Ready...";
        yield return new WaitForSeconds(1f);

        if (GameData.currentEnemyUsesFeint)
    {
        // 50%の確率でフェイントを実行する
        if (Random.value < 0.5f) 
        {
            // 0.5秒から1.5秒、ランダムな時間待つ
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
            // 「?」を表示する
            signalText.text = "?";
            // 0.5秒から1.5秒、ランダムな時間待つ
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        }
    }

        float randomWaitTime = Random.Range(0.5f, 2.0f);
        yield return new WaitForSeconds(randomWaitTime);

        currentState = BattleState.InputReady;
        signalText.text = "！";

        yield return new WaitForSeconds(GameData.currentEnemyReactionTime);

        if (currentState == BattleState.InputReady)
        {
            EndDuel("遅すぎ！", false);
        }
    }

    // EndDuelの処理を、最終的な戦闘終了の処理に特化させる
    void EndDuel(string resultMessage, bool isWin)
    {
        currentState = BattleState.Finished;
        // 1. 進行中のタイマー（コルーチン）を全て停止
        StopAllCoroutines();
        // 2. 「次のラウンドを開始せよ」という予約をキャンセル
        CancelInvoke("StartNextRound");

        // 負けた場合も勝った場合も、最終結果をしっかり表示
        signalText.text = resultMessage;
        if (!isWin) // 負けた場合はメッセージを上書き
        {
            signalText.text = resultMessage;
        }

        if (isWin)
        {
            if (!string.IsNullOrEmpty(GameData.currentEnemyId))
            {
                GameData.defeatedEnemyIds.Add(GameData.currentEnemyId);
            }
            if (GameData.currentEnemyDropAmount > 0)
            {
                PlayerWallet.AddMoney(GameData.currentEnemyDropAmount);
            }
            Invoke("ReturnToField", 2f);
        }
        else
        {
            Invoke("GoToGameOver", 3f);
        }
    }

    void ReturnToField()
    {
        SceneManager.LoadScene("Field");
    }

    void GoToGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}