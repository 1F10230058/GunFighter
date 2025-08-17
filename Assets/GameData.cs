using UnityEngine;
using System.Collections.Generic;

public static class GameData
{
    // --- 基本情報 ---
    public static Sprite currentEnemySprite;
    public static Sprite currentPlayerSprite;

    // --- 敵の戦闘情報 ---
    public static string currentEnemyId;
    public static int currentEnemyDropAmount;
    public static float currentEnemyReactionTime;
    public static bool currentEnemyUsesFeint;

    // --- プレイヤーの状態 ---
    public static Vector3 playerLastPosition;
    public static bool returnedFromBattle = false;
    
    // --- 討伐記録 ---
    public static List<string> defeatedEnemyIds = new List<string>();
}