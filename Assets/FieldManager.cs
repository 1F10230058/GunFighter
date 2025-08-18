using UnityEngine;
using System.Collections.Generic; // Listを使うために必要

public class FieldManager : MonoBehaviour
{
    [Header("敵のスポーン設定")]
    // --- ここを変更 ---
    public List<GameObject> enemyPrefabs; // 複数のプレハブをセットできるリストに変更
    // --- 変更ここまで ---
    public int numberOfEnemies = 5;
    public Transform topLeftBoundary;
    public Transform bottomRightBoundary;


    void Start()
    {
        // 1. まず、討伐済みの敵をシーンから消す
        DestroyDefeatedEnemies();

        // 2. その後で、足りない分の敵を新しくスポーンさせる
        SpawnEnemies();
    }

    void DestroyDefeatedEnemies()
    {
        if (GameData.defeatedEnemyIds.Count == 0) return;

        EnemyController[] enemies = FindObjectsOfType<EnemyController>();

        foreach (EnemyController enemy in enemies)
        {
            if (GameData.defeatedEnemyIds.Contains(enemy.enemyId))
            {
                Destroy(enemy.gameObject);
            }
        }
    }

    void SpawnEnemies()
    {
        int currentEnemyCount = FindObjectsOfType<EnemyController>().Length;
        int enemiesToSpawn = numberOfEnemies - currentEnemyCount;

        // --- enemyPrefabsリストが空でないことを確認 ---
        if (enemyPrefabs.Count == 0)
        {
            Debug.LogError("Enemy Prefabsのリストが空です！");
            return;
        }
        // --- 確認ここまで ---

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            float spawnX = Random.Range(topLeftBoundary.position.x, bottomRightBoundary.position.x);
            float spawnY = Random.Range(bottomRightBoundary.position.y, topLeftBoundary.position.y);

            Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);

            // --- ここを変更 ---
            // リストの中からランダムに1つプレハブを選ぶ
            GameObject prefabToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
            // 選んだプレハブを生成する
            Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            // --- 変更ここまで ---
        }
    }
}