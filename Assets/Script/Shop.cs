using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public static Shop Instance;

    public TextMeshProUGUI starCount;

    public PlayerSkin Skin1;
    public PlayerSkin Skin2;
    public PlayerSkin Skin3;
    public PlayerSkin Skin4;
    public PlayerSkin Skin5;
    public PlayerSkin Skin6;
    public PlayerSkin Skin7;
    public PlayerSkin Skin8;

    public SpriteRenderer curSkin;

    public PlayerSkin thisSkin;
    public void SelectSkin()
    {
        GameManager.Instance.currentSkin = thisSkin;

        if (LevelManager.Instance != null)
            LevelManager.Instance.ingame.ApplySkin();

        curSkin.sprite = GameManager.Instance.currentSkin.front;
    }

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        curSkin.sprite = GameManager.Instance.currentSkin.front;

        // 기본 스킨 무료 처리
        if (GameManager.Instance.currentSkin == null)
        {
            GameManager.Instance.currentSkin = Skin1;
            Skin1.isPurchased = true;
        }

        // 모든 스킨 버튼 UI 다시 갱신
        foreach (SkinButtonUI ui in FindObjectsOfType<SkinButtonUI>())
            ui.SendMessage("Refresh");
    }


    void Update()
    {
        starCount.text = GameManager.Instance.s_Stars.ToString();
    }
    public bool TryBuySkin(PlayerSkin skin)
    {
        if (skin.isPurchased)
            return false;

        if (GameManager.Instance.s_Stars < skin.price)
        {
            Debug.Log("별 부족!");
            return false;
        }

        GameManager.Instance.s_Stars -= skin.price;
        skin.isPurchased = true;

        return true;
    }

}
