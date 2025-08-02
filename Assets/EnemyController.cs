using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // 敵一体一体を識別するためのユニークID
    public string enemyId;

    // このオブジェクトが作られた時に一度だけ呼ばれる
    void Awake()
    {
        // もしIDが空なら、ユニークなIDを自動で生成する
        if (string.IsNullOrEmpty(enemyId))
        {
            enemyId = System.Guid.NewGuid().ToString();
        }
    }
}