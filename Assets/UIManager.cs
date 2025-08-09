using UnityEngine;
using TMPro; // TextMeshProを扱うために必要

public class UIManager : MonoBehaviour
{
    // InspectorからMoneyTextをセットするための変数
    public TextMeshProUGUI moneyText;

    // 毎フレーム呼ばれる
    void Update()
    {
        // PlayerWalletから現在の所持金を取得して、テキストを更新する
        if (moneyText != null)
        {
            moneyText.text = "G: " + PlayerWallet.CurrentMoney;
        }
    }
}