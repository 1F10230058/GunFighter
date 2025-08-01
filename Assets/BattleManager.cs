using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // シーン遷移に必要
using TMPro;

public class BattleManager : MonoBehaviour
{
    public TextMeshProUGUI signalText;
    // 敵が「！」に反応するまでの時間（秒）
    public float enemyReactionTime = 0.5f;

    // 敵の画像を表示するためのSpriteRendererをアタッチする欄
    public SpriteRenderer enemyDisplay;

    public SpriteRenderer playerDisplay;

    public float characterScale = 2f; // <<< キャラクターのサイズ倍率

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
        // プレイヤーの表示位置とサイズを設定
        if (playerDisplay != null)
        {
            Vector3 playerPos = Vector3.zero;
            playerPos.x = -4f;
            playerDisplay.transform.position = playerPos;
            playerDisplay.transform.localScale = new Vector3(characterScale, characterScale, 1f); // <<< サイズ設定を追加
        }

        // 敵の表示位置とサイズを設定
        if (enemyDisplay != null)
        {
            Vector3 enemyPos = Vector3.zero;
            enemyPos.x = 5f;
            enemyDisplay.transform.position = enemyPos;
            enemyDisplay.transform.localScale = new Vector3(characterScale, characterScale, 1f); // <<< サイズ設定を追加
        }

        // スプライトをセット
        if (GameData.currentPlayerSprite != null)
        {
            playerDisplay.sprite = GameData.currentPlayerSprite;
        }

        if (GameData.currentEnemySprite != null)
        {
            enemyDisplay.sprite = GameData.currentEnemySprite;
        }

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
                // 負けなので false を渡す
                EndDuel("早すぎ！", false);
            }
            // 入力受付中に押した場合
            else if (currentState == BattleState.InputReady)
            {
                // 勝ちなので true を渡す
                EndDuel("勝ち！", true);
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
            // 負けなので false を渡す
            EndDuel("遅すぎ！", false);
        }
    }

    // 勝敗をboolで受け取るように変更
    void EndDuel(string resultMessage, bool isWin)
    {
        currentState = BattleState.Finished;
        signalText.text = resultMessage;

        // もし勝ちなら
        if (isWin)
        {
            // 2秒後にフィールドへ
            Invoke("ReturnToField", 2f);
        }
        // 負けなら
        else
        {
            // 2秒後にゲームオーバーへ
            Invoke("GoToGameOver", 2f);
        }
    }

    void ReturnToField()
    {
        SceneManager.LoadScene("Field");
    }

    // ゲームオーバーシーンに移動する関数を追加
    void GoToGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}