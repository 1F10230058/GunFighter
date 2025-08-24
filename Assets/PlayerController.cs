using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // 移動速度
    public float moveSpeed = 5f;
    // ダッシュ時の速度倍率
    public float dashMultiplier = 1.5f;

    // 物理演算コンポーネントを入れておく箱
    private Rigidbody2D rb;
    // 入力方向を覚えておく変数
    private Vector2 movement;
    private FieldManager fieldManager;

    // ゲーム開始時に一度だけ呼ばれる
    void Start()
    {
        // シーン内にあるFieldManagerを探して、変数に保存しておく
        fieldManager = FindObjectOfType<FieldManager>();
            // もし戦闘から戻ってきたなら
        if (GameData.returnedFromBattle)
        {
            // 保存しておいた位置に戻す
            transform.position = GameData.playerLastPosition;
            // フラグをリセットして、次回シーンロード時には実行されないようにする
            GameData.returnedFromBattle = false;
        }

        rb = GetComponent<Rigidbody2D>();
        GameData.currentPlayerSprite = GetComponent<SpriteRenderer>().sprite;
    }

    // 毎フレーム呼ばれる
    void Update()
    {
        // キーボードの上下左右の入力を受け取る
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    // 物理演算のタイミングで呼ばれる
    void FixedUpdate()
    {
        // 現在の速度を計算する
        float currentSpeed = moveSpeed;

        // もし左Shiftキーが押されていたら
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // 現在の速度をダッシュ時の速度にする
            currentSpeed = moveSpeed * dashMultiplier;
        }

        // 1. 次に移動する先の座標を計算する
        Vector2 nextPosition = rb.position + movement.normalized * currentSpeed * Time.fixedDeltaTime;

        // 2. X座標とY座標を、境界線の範囲内に制限（クランプ）する
        if (fieldManager != null)
        {
            nextPosition.x = Mathf.Clamp(nextPosition.x, fieldManager.topLeftBoundary.position.x, fieldManager.bottomRightBoundary.position.x);
            nextPosition.y = Mathf.Clamp(nextPosition.y, fieldManager.bottomRightBoundary.position.y, fieldManager.topLeftBoundary.position.y);
        }

        // 3. 制限された座標に向かって移動する
        rb.MovePosition(nextPosition);
    }

    // 他のコライダーと衝突した時に呼ばれる
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ぶつかった相手のタグ
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Boss")
        {
            // 自分の位置を保存
        GameData.playerLastPosition = transform.position;
        GameData.returnedFromBattle = true;

        // 接触した敵のIDを取得して保存
        EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
        if (enemy != null && enemy.enemyType != null)
        {
            GameData.currentEnemyId = enemy.enemyId;
            // enemyTypeを通して、正しいデータにアクセスする
            GameData.currentEnemySprite = enemy.enemyType.sprite;
            GameData.currentEnemyDropAmount = enemy.enemyType.moneyDropAmount;
            GameData.currentEnemyUsesFeint = enemy.enemyType.usesFeint;
            GameData.currentEnemyReactionTime = enemy.enemyType.reactionTime;
            GameData.currentEnemyRequiredWins = enemy.enemyType.requiredWins;
            GameData.currentEnemyBattleScaleMultiplier = enemy.enemyType.battleScaleMultiplier;
            Debug.Log("【戦闘開始】このIDの敵と戦います: " + GameData.currentEnemyId);
        }

        // 敵のスプライトを保存
        SpriteRenderer enemySpriteRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
        if (enemySpriteRenderer != null)
        {
            GameData.currentEnemySprite = enemySpriteRenderer.sprite;
        }

        // 戦闘シーンをロード
        SceneManager.LoadScene("Battle");

        }
    }
}