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
        if (!skin.isPurchased)
        {
            TryBuy();
            return;
        }

        Select();
    }

    void TryBuy()
    {
        if (GameManager.Instance.s_Stars < skin.price)
        {
            Debug.Log("별 부족");
            return;
        }

        GameManager.Instance.s_Stars -= skin.price;
        skin.isPurchased = true;

        GameManager.Instance.SaveData();
        RefreshAll();   // UI만 갱신
    }

    void Select()
    {
        GameManager.Instance.currentSkinIndex = skinIndex;

        if (LevelManager.Instance != null && LevelManager.Instance.ingame != null)
            LevelManager.Instance.ingame.ApplySkin();

        Shop.Instance.curSkin.sprite =
            GameManager.Instance.allSkins[skinIndex].front;

        RefreshAll();
    }

    public void Refresh()
    {
        if (skin == null) return;

        if (!skin.isPurchased)
            buttonText.text = "구매\n별: " + skin.price;
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
