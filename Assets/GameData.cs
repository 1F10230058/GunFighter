using UnityEngine;
using System.Collections.Generic;

public static class GameData
{
    public static Sprite currentEnemySprite;
    public static Sprite currentPlayerSprite;

    public static Vector3 playerLastPosition;
    public static bool currentEnemyUsesFeint;
    public static bool returnedFromBattle = false;

    public static string currentEnemyId;

    public static List<string> defeatedEnemyIds = new List<string>();
    public static int currentEnemyDropAmount;
}