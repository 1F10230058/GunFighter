using UnityEngine;
using UnityEngine.UI; // Buttonを扱うために必要
using TMPro;

public class UIManager : MonoBehaviour
{
    public AudioClip clickSound;
    [Header("所持金表示")]
    public TextMeshProUGUI moneyText;

    [Header("ショップ設定")]
    public GameObject shopPanel; // ショップのパネル全体
    public Button buyJewelButton; // 宝玉を買うボタン

    private const string JEWEL_KEY = "HasJewel"; // 宝玉を買ったかどうかのセーブデータ名
    private const int JEWEL_PRICE = 300; // 宝玉の値段

    void Start()
    {
        // ゲーム開始時はショップを閉じておく
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
        }
        // 購入状況をチェックしてボタンの表示を更新
        CheckJewelPurchaseStatus();
    }

    void Update()
    {
        if (moneyText != null)
        {
            moneyText.text = "G: " + PlayerWallet.CurrentMoney;
        }
    }

    // --- ここからショップ用の関数 ---

    public void OpenShop()
    {
        shopPanel.SetActive(true);
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
    }

    public void BuyJewel()
    {
        // 300Gを支払おうとして、成功したら
        if (PlayerWallet.TrySpendMoney(JEWEL_PRICE))
        {
            // 「宝玉を買った」という記録をセーブデータに保存
            PlayerPrefs.SetInt(JEWEL_KEY, 1); // 1 = trueの意味
            PlayerPrefs.Save();

            Debug.Log("宝玉を購入した！");

            // ボタンを「売り切れ」にする
            CheckJewelPurchaseStatus();
        }
    }

    // 宝玉の購入状況をチェックしてボタンの状態を変える
    private void CheckJewelPurchaseStatus()
    {
        // もしセーブデータに「宝玉を買った」記録があれば
        if (PlayerPrefs.GetInt(JEWEL_KEY, 0) == 1)
        {
            // ボタンを押せなくして、テキストを「売り切れ」にする
            buyJewelButton.interactable = false;
            buyJewelButton.GetComponentInChildren<TextMeshProUGUI>().text = "Sold out!";
        }
    }
    public void PlayClickSound() // <<< 新しい関数を追加
    {
        AudioManager.Instance.PlaySFX(clickSound);
    }
}