using UnityEngine;
using System.Collections; // コルーチンを使うために必要

public class EnemyController : MonoBehaviour
{
    [Header("ドロップ設定")]
    public int moneyDropAmount = 10; // この敵を倒した時にもらえるお金

    [Header("AI設定")]
    public string enemyId;
    public float moveSpeed = 1f; // 移動スピード
    public float minWaitTime = 1f; // 待機時間の最小値
    public float maxWaitTime = 3f; // 待機時間の最大値
    public float moveDistance = 1f; // 一度に移動する距離

    private Rigidbody2D rb;
    private Vector2 targetPosition; // 次の目的地
    private bool isMoving = false; // 現在移動中かどうかのフラグ
    private FieldManager fieldManager;


    void Awake()
    {
        if (string.IsNullOrEmpty(enemyId))
        {
            enemyId = System.Guid.NewGuid().ToString();
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // シーン内にあるFieldManagerを探して、変数に保存しておく
        fieldManager = FindObjectOfType<FieldManager>();
        // AIの行動を開始する
        StartCoroutine(WanderAI());
    }

    void FixedUpdate()
    {
        // もし移動中なら、目的地に向かって進む
        if (isMoving)
        {
            Vector2 direction = (targetPosition - rb.position).normalized;
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

            // 目的地に十分近づいたら、移動を停止する
            if (Vector2.Distance(rb.position, targetPosition) < 0.1f)
            {
                isMoving = false;
            }
        }
    }

    // 敵の行動パターンを制御するコルーチン
    IEnumerator WanderAI()
    {
        // このループを無限に繰り返す
        while (true)
        {
            // 1. 次の目的地の候補を計算する
            float randomX = Random.Range(-moveDistance, moveDistance);
            float randomY = Random.Range(-moveDistance, moveDistance);
            Vector2 potentialPosition = rb.position + new Vector2(randomX, randomY);

            // 2. もしFieldManagerが見つかっていれば、座標を境界線の範囲内に制限する
            if (fieldManager != null)
            {
                potentialPosition.x = Mathf.Clamp(potentialPosition.x, fieldManager.topLeftBoundary.position.x, fieldManager.bottomRightBoundary.position.x);
                potentialPosition.y = Mathf.Clamp(potentialPosition.y, fieldManager.bottomRightBoundary.position.y, fieldManager.topLeftBoundary.position.y);
            }

            // 3. 制限された座標を最終的な目的地として設定する
            targetPosition = potentialPosition;
            isMoving = true;

            // 2. 目的地に到着するまで待つ（isMovingがfalseになるまで）
            yield return new WaitUntil(() => !isMoving);

            // 3. ランダムな時間だけ待機する
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);
        }
    }
}