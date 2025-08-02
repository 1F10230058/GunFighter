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

    // ゲーム開始時に一度だけ呼ばれる
    void Start()
    {
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

        // 計算された速度で移動する
        rb.MovePosition(rb.position + movement.normalized * currentSpeed * Time.fixedDeltaTime);
    }

    // 他のコライダーと衝突した時に呼ばれる
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ぶつかった相手のタグが "Enemy" だったら
        if (collision.gameObject.tag == "Enemy")
        {
            // 自分の位置を保存
        GameData.playerLastPosition = transform.position;
        GameData.returnedFromBattle = true;

        // 接触した敵のIDを取得して保存
        EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
        if (enemy != null)
        {
            GameData.currentEnemyId = enemy.enemyId;
            Debug.Log("戦闘開始！ 相手のID: " + GameData.currentEnemyId);
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