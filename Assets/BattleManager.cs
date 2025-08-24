using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Sliderを使うために必要
using TMPro;

public class BattleManager : MonoBehaviour
{
    [Header("サウンド")]
    public AudioClip signalSound; // 「！」の音
    public AudioClip feintSound;  // 「？」の音
    public AudioClip winRoundSound; // 勝利時（銃を撃つ）の音
    public AudioClip loseRoundSound; // 敗北時（撃たれる）の音
    [Header("UI参照")]
    public TextMeshProUGUI signalText;
    public Slider enemyHpBar; // <<< HPバー用の変数を追加

    [Header("オブジェクト参照")]
    public SpriteRenderer enemyDisplay;
    public SpriteRenderer playerDisplay;

    [Header("戦闘設定")]
    public float characterScale = 2f;

    private BattleState currentState;
    private int currentWins = 0;

    private enum BattleState { Waiting, InputReady, Finished }

    void Start()
    {
        // 位置とサイズの設定
        if (playerDisplay != null)
        {
            playerDisplay.transform.position = new Vector3(-3f, 0, 0);
            playerDisplay.transform.localScale = new Vector3(characterScale, characterScale, 1f);
        }
        if (enemyDisplay != null)
        {
            enemyDisplay.transform.position = new Vector3(3f, 0, 0);
            float finalEnemyScale = characterScale * GameData.currentEnemyBattleScaleMultiplier;
            enemyDisplay.transform.localScale = new Vector3(finalEnemyScale, finalEnemyScale, 1f);
        }

        // スプライトの設定
        if (GameData.currentPlayerSprite != null) playerDisplay.sprite = GameData.currentPlayerSprite;
        if (GameData.currentEnemySprite != null) enemyDisplay.sprite = GameData.currentEnemySprite;

        // --- HPバーの初期設定を追加 ---
        if (enemyHpBar != null)
        {
            // 条件分岐をなくし、常にHPバーを表示する
            enemyHpBar.gameObject.SetActive(true);
            // Sliderの最大値を、必要な勝利数に設定
            enemyHpBar.maxValue = GameData.currentEnemyRequiredWins;
            // Sliderの現在の値を、残りHP（必要勝利数 - 現在の勝利数）に設定
            enemyHpBar.value = GameData.currentEnemyRequiredWins - currentWins;
        }
        // --- HPバー設定ここまで ---

        StartCoroutine(StartRound());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentState == BattleState.Waiting)
            {
                if (signalText.text == "?") EndDuel("Too Fast！", false);
                else EndDuel("Too Fast！", false);
            }
            else if (currentState == BattleState.InputReady)
            {
                RoundWin();
            }
        }
    }

    void RoundWin()
    {
        currentState = BattleState.Finished;
        currentWins++;

        // --- 勝利時の演出を追加 ---
        AudioManager.Instance.PlaySFX(winRoundSound);
        StartCoroutine(ShakeObject(enemyDisplay.transform, 0.2f, 0.1f)); // プレイヤーを揺らす
        if (enemyHpBar != null)
        {
            enemyHpBar.value = GameData.currentEnemyRequiredWins - currentWins; // HPバーを更新
        }
        // --- 演出ここまで ---

        signalText.text = "Good！";

        if (currentWins >= GameData.currentEnemyRequiredWins)
        {
            Invoke("TriggerFinalWin", 1f);
        }
        else
        {
            Invoke("StartNextRound", 2f);
        }
    }

    void TriggerFinalWin()
    {
        EndDuel("VICTORY！", true);
    }

    void StartNextRound()
    {
        StartCoroutine(StartRound());
    }

    IEnumerator StartRound()
    {
        currentState = BattleState.Waiting;
        signalText.text = "Ready...";
        yield return new WaitForSeconds(1f);

        if (GameData.currentEnemyUsesFeint)
        {
            if (Random.value < 0.5f)
            {
                yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
                signalText.text = "?";
                AudioManager.Instance.PlaySFX(feintSound);
                yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
            }
        }

        float randomWaitTime = Random.Range(0.5f, 2.0f);
        yield return new WaitForSeconds(randomWaitTime);
        currentState = BattleState.InputReady;
        signalText.text = "！";
        AudioManager.Instance.PlaySFX(signalSound);
        yield return new WaitForSeconds(GameData.currentEnemyReactionTime);
        if (currentState == BattleState.InputReady)
        {
            EndDuel("Too Slow！", false);
        }
    }

    void EndDuel(string resultMessage, bool isWin)
    {
        currentState = BattleState.Finished;
        StopAllCoroutines();
        CancelInvoke("StartNextRound");
        signalText.text = resultMessage;
        if (isWin)
        {
            if (!string.IsNullOrEmpty(GameData.currentEnemyId)) GameData.defeatedEnemyIds.Add(GameData.currentEnemyId);
            if (GameData.currentEnemyDropAmount > 0) PlayerWallet.AddMoney(GameData.currentEnemyDropAmount);
            Invoke("ReturnToField", 2f);
        }
        else
        {
            AudioManager.Instance.PlaySFX(loseRoundSound);
            StartCoroutine(ShakeObject(playerDisplay.transform, 0.4f, 0.2f));
            // Invokeをやめて、新しいコルーチンを呼び出す
            StartCoroutine(FadeToGameOver());
        }
    }

    // --- シェイク用のコルーチンを追加 ---
    IEnumerator ShakeObject(Transform objTransform, float duration, float magnitude)
    {
        Vector3 originalPos = objTransform.position;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            objTransform.position = new Vector3(originalPos.x + x, originalPos.y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        objTransform.position = originalPos; // 最後に元の位置に戻す
    }
    // --- シェイク処理ここまで ---

    void ReturnToField() { SceneManager.LoadScene("Field"); }
    void GoToGameOver() { SceneManager.LoadScene("GameOver"); }
    
    // フェードアウトさせてからゲームオーバーへ移行する処理
    private IEnumerator FadeToGameOver()
    {
        // FadeImageを取得してフェードアウトを開始
        GameObject fadeImageObject = GameObject.Find("FadeImage");
        if(fadeImageObject != null)
        {
            yield return StartCoroutine(FadeOut(fadeImageObject.GetComponent<CanvasGroup>()));
        }

        // フェードアウトが終わったらシーンをロード
        SceneManager.LoadScene("GameOver");
    }

    // 徐々に暗くするコルーチン
    private IEnumerator FadeOut(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
        while(canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime;
            yield return null;
        }
    }
}