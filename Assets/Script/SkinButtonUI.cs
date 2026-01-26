using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinButtonUI : MonoBehaviour
{
    public PlayerSkin skin;
    public TextMeshProUGUI buttonText;
    public Button button;

    void Start()
    {
        Refresh();
    }

    public void OnClick()
    {
        // 이미 구매한 스킨
        if (skin.isPurchased)
        {
            Select();
        }
        else
        {
            Buy();
        }
    }

    void Buy()
    {
        if (GameManager.Instance.s_Stars < skin.price)
        {
            Debug.Log("별 부족");
            return;
        }

        GameManager.Instance.s_Stars -= skin.price;
        skin.isPurchased = true;

        Select();
    }

    void Select()
    {
        GameManager.Instance.currentSkin = skin;

        // 인게임에 있으면만 바로 반영
        if (LevelManager.Instance != null && LevelManager.Instance.ingame != null)
            LevelManager.Instance.ingame.ApplySkin();

        RefreshAll();

        Shop.Instance.curSkin.sprite = skin.front;

    }


    public void Refresh()
    {
        if (!skin.isPurchased)
        {
            buttonText.text = "구매\n별: 10";
        }
        else if (GameManager.Instance.currentSkin == skin)
        {
            buttonText.text = "선택됨";
        }
        else
        {
            buttonText.text = "선택";
        }
    }

    void RefreshAll()
    {
        foreach (SkinButtonUI ui in FindObjectsOfType<SkinButtonUI>())
            ui.Refresh();
    }
}
