using UnityEngine;

[System.Serializable]
public class PlayerSkin
{
    public Sprite front;
    public Sprite right;
    public Sprite left;
    public Sprite back;

    public int price = 10;
    public bool isPurchased;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentClearLevel = 0;
    public int currentPlayingLevel;

    public int s_Stars;

    public int currentSkinIndex;
    public PlayerSkin defaultSkin;

    // 상점에 있는 모든 스킨들 연결
    public PlayerSkin[] allSkins;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else Destroy(gameObject);
    }

    public bool IsPurchased(int index)
    {
        return PlayerPrefs.GetInt("Skin_" + index, index == 0 ? 1 : 0) == 1;
    }

    public void SetPurchased(int index)
    {
        PlayerPrefs.SetInt("Skin_" + index, 1);
        PlayerPrefs.Save();
    }

    public void SaveCurrentSkin(int index)
    {
        currentSkinIndex = index;
        PlayerPrefs.SetInt("CurrentSkin", index);
        PlayerPrefs.Save();
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("Stars", s_Stars);
        PlayerPrefs.SetInt("ClearLevel", currentClearLevel);

        // 스킨 구매 저장
        for (int i = 0; i < allSkins.Length; i++)
        {
            PlayerPrefs.SetInt("Skin_" + i, allSkins[i].isPurchased ? 1 : 0);
            Debug.Log("저장됨: " + allSkins[i].isPurchased);
        }

        // 현재 선택 스킨 저장
        PlayerPrefs.SetInt("CurrentSkin", currentSkinIndex);

        PlayerPrefs.Save();
    }

    void LoadData()
    {
        s_Stars = PlayerPrefs.GetInt("Stars", 0);
        currentSkinIndex = PlayerPrefs.GetInt("CurrentSkin", 0);
        currentClearLevel = PlayerPrefs.GetInt("ClearLevel", 0);
    }

    public void SaveStars()
    {
        PlayerPrefs.SetInt("Stars", s_Stars);
    }
}
