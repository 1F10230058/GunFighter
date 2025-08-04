using UnityEngine;
using System.Collections.Generic; // Listを使うために必要

public class FieldManager : MonoBehaviour
{
    // --- ここから追加 ---
    [Header("敵のスポーン設定")]
    public GameObject enemyPrefab; // 敵のプレハブをセットする
    public int numberOfEnemies = 5; // スポーンさせたい敵の数
    public Transform topLeftBoundary; // スポーン範囲の左上
    public Transform bottomRightBoundary; // スポーン範囲の右下
    // --- ここまで追加 ---


    void Start()
    {
        // --- ここから追加 ---
        SpawnEnemies();
        // --- ここまで追加 ---

        // 討伐済みの敵を探して消す処理（これは残しておく）
        DestroyDefeatedEnemies();
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

    // --- ここから追加 ---
    void SpawnEnemies()
    {
        // 現在の敵の数を数える
        int currentEnemyCount = FindObjectsOfType<EnemyController>().Length;
        // 倒した敵の分だけ補充する
        int enemiesToSpawn = numberOfEnemies - currentEnemyCount;

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // 設定した範囲内でランダムな座標を計算
            float spawnX = Random.Range(topLeftBoundary.position.x, bottomRightBoundary.position.x);
            float spawnY = Random.Range(bottomRightBoundary.position.y, topLeftBoundary.position.y); // Y座標はminとmaxが逆

            Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);

            // 敵プレハブを、計算した座標に生成する
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
    // --- ここまで追加 ---
}