using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    public static Shop Instance;

    public TextMeshProUGUI starCount;
    public SpriteRenderer curSkin;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        curSkin.sprite =
            GameManager.Instance.allSkins[GameManager.Instance.currentSkinIndex].front;

        var allButtons = FindObjectsByType<SkinButtonUI>(FindObjectsSortMode.None);
        foreach (var ui in allButtons)
            ui.Refresh();
    }



    void Update()
    {
        starCount.text = GameManager.Instance.s_Stars.ToString();
    }

    public void RefreshSkin()
    {
        curSkin.sprite =
            GameManager.Instance.allSkins[
                GameManager.Instance.currentSkinIndex
            ].front;
    }
}
