using UnityEngine;
using System.Collections; // コルーチンを使うために必要

public class EnemyController : MonoBehaviour
{
    public string enemyId;

    // --- ここからAI用の設定を追加 ---
    [Header("AI設定")]
    public float moveSpeed = 1f; // 移動スピード
    public float minWaitTime = 1f; // 待機時間の最小値
    public float maxWaitTime = 3f; // 待機時間の最大値
    public float moveDistance = 1f; // 一度に移動する距離

    private Rigidbody2D rb;
    private Vector2 targetPosition; // 次の目的地
    private bool isMoving = false; // 現在移動中かどうかのフラグ
    // --- ここまで追加 ---


    void Awake()
    {
        if (string.IsNullOrEmpty(enemyId))
        {
            enemyId = System.Guid.NewGuid().ToString();
        }
    }

    // --- ここから新しい関数を追加 ---
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
            // 1. 新しい目的地を決める
            float randomX = Random.Range(-moveDistance, moveDistance);
            float randomY = Random.Range(-moveDistance, moveDistance);
            targetPosition = rb.position + new Vector2(randomX, randomY);
            isMoving = true;

            // 2. 目的地に到着するまで待つ（isMovingがfalseになるまで）
            yield return new WaitUntil(() => !isMoving);

            // 3. ランダムな時間だけ待機する
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);
        }
    }
    // --- ここまで追加 ---
}