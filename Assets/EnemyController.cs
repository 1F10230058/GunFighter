using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    // この敵がどのタイプか、Inspectorから設定する
    public EnemyType enemyType;

    // --- 内部で使う変数 ---
    public string enemyId;
    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private bool isMoving = false;
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
        fieldManager = FindObjectOfType<FieldManager>();
        // 設計図のデータに基づいて自分を設定する
        GetComponent<SpriteRenderer>().sprite = enemyType.sprite;
        StartCoroutine(WanderAI());
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            Vector2 direction = (targetPosition - rb.position).normalized;
            // AIのスピードも設計図から読み込む
            rb.MovePosition(rb.position + direction * enemyType.moveSpeed * Time.fixedDeltaTime);

            if (Vector2.Distance(rb.position, targetPosition) < 0.1f)
            {
                isMoving = false;
            }
        }
    }

    IEnumerator WanderAI()
    {
        float moveDistance = 1f; // この値は固定でも良い
        while (true)
        {
            float randomX = Random.Range(-moveDistance, moveDistance);
            float randomY = Random.Range(-moveDistance, moveDistance);
            Vector2 potentialPosition = rb.position + new Vector2(randomX, randomY);
            if (fieldManager != null)
            {
                potentialPosition.x = Mathf.Clamp(potentialPosition.x, fieldManager.topLeftBoundary.position.x, fieldManager.bottomRightBoundary.position.x);
                potentialPosition.y = Mathf.Clamp(potentialPosition.y, fieldManager.bottomRightBoundary.position.y, fieldManager.topLeftBoundary.position.y);
            }
            targetPosition = potentialPosition;
            isMoving = true;
            yield return new WaitUntil(() => !isMoving);
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }
}