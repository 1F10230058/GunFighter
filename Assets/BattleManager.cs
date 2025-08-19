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

    public static bool currentEnemyUsesFeint;

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
            if (currentState == BattleState.Waiting)
        {
            // もし、画面の文字が「？」の時にキーを押してしまったら
            if (signalText.text == "?")
            {
                // フェイントに引っかかった専用のメッセージで負けにする
                EndDuel("早すぎ！", false);
            }
            else
            {
                // それ以外のタイミングなら、通常の「早すぎ！」で負けにする
                EndDuel("早すぎ！", false);
            }
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
    currentState = BattleState.Waiting;
    signalText.text = "Ready...";
    yield return new WaitForSeconds(1f); // Ready...を1秒表示

    // --- ここからフェイント処理 ---
    // もし、今の敵がフェイントを使うなら
    if (GameData.currentEnemyUsesFeint)
    {
        // 0.5秒から1.5秒、ランダムな時間待つ
        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        // 「？」を表示する
        signalText.text = "?";
        // 0.5秒から1.5秒、ランダムな時間待つ
        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
    }
    // --- ここまでフェイント処理 ---

    // 通常の「！」までの待機
    float randomWaitTime = Random.Range(0.5f, 2.0f);
    yield return new WaitForSeconds(randomWaitTime);

    currentState = BattleState.InputReady;
    signalText.text = "！";

    // 敵の反応時間も設計図から読み込むように変更
    yield return new WaitForSeconds(GameData.currentEnemyReactionTime); // ※

    if (currentState == BattleState.InputReady)
    {
        EndDuel("遅すぎ！", false);
    }
}

    // 勝敗をboolで受け取るように変更
    void EndDuel(string resultMessage, bool isWin)
    {
        currentState = BattleState.Finished;
        signalText.text = resultMessage;

        if (isWin)
        {
            // 1. 倒した敵のIDをリストに追加
        if (!string.IsNullOrEmpty(GameData.currentEnemyId))
        {
            GameData.defeatedEnemyIds.Add(GameData.currentEnemyId);
        }

        // 2. お金を追加する（GameDataに保存しておいた敵の情報を利用）
        if (GameData.currentEnemyDropAmount > 0)
        {
            PlayerWallet.AddMoney(GameData.currentEnemyDropAmount);
        }

            Invoke("ReturnToField", 2f);
        }
        else
        {
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