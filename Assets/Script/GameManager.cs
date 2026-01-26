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
    public int s_Stars = 0;

    public PlayerSkin currentSkin;
    public PlayerSkin defaultSkin;   // 인스펙터에 Skin1 넣기

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 기본 스킨 강제 세팅
            if (currentSkin == null)
            {
                currentSkin = defaultSkin;
                defaultSkin.isPurchased = true;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

}

