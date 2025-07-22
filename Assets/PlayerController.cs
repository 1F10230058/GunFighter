using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // publicにするとUnityエディタのインスペクターから調整できる
    public float moveSpeed = 5f;

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
        // Rigidbodyの位置を、入力された方向とスピードに合わせて更新する
        // Time.fixedDeltaTime を掛けることで、どのPCでも同じ速度で動くようになる
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}