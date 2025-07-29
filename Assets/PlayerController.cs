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
        // このオブジェクトに付いている Rigidbody 2D を取得して rb に保存
        rb = GetComponent<Rigidbody2D>();

        // このオブジェクトに付いている Sprite Renderer から画像を取得し、GameDataに保存
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
            // 接触した敵からSpriteRendererコンポーネントを取得
            SpriteRenderer enemySpriteRenderer = collision.gameObject.GetComponent<SpriteRenderer>();

            // 取得したスプライトを、データの保管庫に保存
            if (enemySpriteRenderer != null)
            {
                GameData.currentEnemySprite = enemySpriteRenderer.sprite;
            }

            // 戦闘シーンをロードする
            SceneManager.LoadScene("Battle");
        }
    }
}