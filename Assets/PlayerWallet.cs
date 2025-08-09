using UnityEngine;

public static class PlayerWallet
{
    // データを保存するときの「キー（名前）」
    private const string MONEY_KEY = "PlayerMoney";

    // 現在の所持金
    public static int CurrentMoney { get; private set; }

    // ゲーム起動時に一度だけお金をロードする
    static PlayerWallet()
    {
        LoadMoney();
    }

    // お金を追加する関数
    public static void AddMoney(int amount)
    {
        CurrentMoney += amount;
        SaveMoney();
        // 確認用ログ
        Debug.Log(amount + "円獲得！ 現在の所持金: " + CurrentMoney + "円");
    }

    // お金を保存する関数
    private static void SaveMoney()
    {
        PlayerPrefs.SetInt(MONEY_KEY, CurrentMoney);
        PlayerPrefs.Save(); // これで端末に保存される
    }

    // お金をロードする関数
    private static void LoadMoney()
    {
        // 保存されているデータを読み込む（なければ0）
        CurrentMoney = PlayerPrefs.GetInt(MONEY_KEY, 0);
    }
}