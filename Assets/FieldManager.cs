using UnityEngine;

public class FieldManager : MonoBehaviour
{
    // このシーンが始まった時に呼ばれる
    void Start()
    {
        // 討伐済みの敵を探して消す
        DestroyDefeatedEnemies();
    }

    void DestroyDefeatedEnemies()
    {
        // もし討伐済みリストに何もなければ何もしない
        if (GameData.defeatedEnemyIds.Count == 0) return;

        // シーンにいる全てのEnemyControllerを探す
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();
        Debug.Log("フィールドの敵をチェックします。討伐済みリストの数: " + GameData.defeatedEnemyIds.Count);

        // 全ての敵をループでチェック
        foreach (EnemyController enemy in enemies)
        {
            Debug.Log("【フィールド確認】このIDの敵を見つけました: " + enemy.enemyId);
            // もし、その敵のIDが討伐済みリストに含まれていたら
            if (GameData.defeatedEnemyIds.Contains(enemy.enemyId))
            {
                // その敵のゲームオブジェクトを破壊する
                Destroy(enemy.gameObject);
            }
        }
    }
}