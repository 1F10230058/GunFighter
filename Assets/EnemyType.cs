using UnityEngine;

// Projectウィンドウで右クリック > Create > Enemy Type で新しいデータを作れるようにする
[CreateAssetMenu(fileName = "NewEnemyType", menuName = "Enemy Type")]
public class EnemyType : ScriptableObject
{
    [Header("基本情報")]
    public string enemyName; // 敵の名前
    public Sprite sprite; // 見た目の画像

    [Header("ドロップ設定")]
    public int moneyDropAmount = 10; // 落とすお金

    [Header("AI設定")]
    public float moveSpeed = 1f; // 移動スピード

    [Header("戦闘設定")]
    public float reactionTime = 0.5f; // 反応速度
    public bool usesFeint = false; // フェイントを使うかどうか
    public int requiredWins = 1; // 勝利に必要な回数
}