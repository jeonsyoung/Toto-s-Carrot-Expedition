using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinButtonUI : MonoBehaviour
{
    public int skinIndex;

    PlayerSkin skin;

    public TextMeshProUGUI buttonText;
    public Button button;

    void Awake()
    {
        if (GameManager.Instance == null) return;

        if (skinIndex < 0 || skinIndex >= GameManager.Instance.allSkins.Length)
            return;

        skin = GameManager.Instance.allSkins[skinIndex];
    }

    void Start()
    {
        Refresh();
    }

    public void OnClick()
    {
        if (skin == null)
        {
            Debug.LogError("Skin null! index: " + skinIndex);
            return;
        }

        if (skin.isPurchased)
            Select();
        else
            Buy();
    }

    void Buy()
    {
        int price = GameManager.Instance.allSkins[skinIndex].price;

        if (GameManager.Instance.s_Stars < price)
            return;

        GameManager.Instance.s_Stars -= price;
        GameManager.Instance.SaveStars();

        GameManager.Instance.SetPurchased(skinIndex);

        Select();
    }

    void Select()
    {
        GameManager.Instance.SaveCurrentSkin(skinIndex);

        Shop.Instance.curSkin.sprite =
            GameManager.Instance.allSkins[skinIndex].front;

        RefreshAll();
    }

    public void Refresh()
    {
        bool purchased = GameManager.Instance.IsPurchased(skinIndex);

        if (!purchased)
            buttonText.text = "구매\n별: " + GameManager.Instance.allSkins[skinIndex].price;
        else if (skinIndex == GameManager.Instance.currentSkinIndex)
            buttonText.text = "선택됨";
        else
            buttonText.text = "선택";
    }

    void RefreshAll()
    {
        foreach (var ui in FindObjectsByType<SkinButtonUI>(FindObjectsSortMode.None))
            ui.Refresh();
    }
}
