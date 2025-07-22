using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // publicにするとUnityエディタのインスペクターから調整できる
    public float moveSpeed = 5f;
    public float dashMultiplier = 2f;

    private Rigidbody2D rb;
    private Vector2 movement;

    // ゲームが始まった時に一度だけ呼ばれる
    void Start()
    {
        // Playerが持っているRigidbody 2Dコンポーネントを取得しておく
        rb = GetComponent<Rigidbody2D>();
    }

    // フレームごとに毎回呼ばれる
    void Update()
    {
        // キーボードの水平（左右、A/D）と垂直（上下、W/S）の入力を受け取る
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    // 一定間隔で物理演算の前に呼ばれる
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

    // このオブジェクトが他のコライダーと衝突した時に呼ばれる関数
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ぶつかった相手のゲームオブジェクトのタグが "Enemy" だったら
        if (collision.gameObject.tag == "Enemy")
        {
            // "Battle" という名前のシーンをロードする
            SceneManager.LoadScene("Battle");
        }
    }
}