using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int levelIndex;

    [Header("Sprites")]
    public Sprite lockedSprite;    // 기본 잠금 이미지
    public Sprite unlockedSprite;  // 해금 이미지

    [Header("UI")]
    public GameObject txt;         // 하위 텍스트
    public Button button;

    Image img;

    void Start()
    {
        img = GetComponent<Image>();
        UpdateState();
    }

    public void UpdateState()
    {
        int cleared = LevelManager.Instance.currentClearLevel;

        // 이전 레벨을 클리어했으면 해금
        if (levelIndex == cleared + 1)
        {
            SetUnlocked();
        }
        else if (levelIndex <= cleared)
        {
            // 이미 플레이했던 레벨 (이미지는 그대로 두거나 unlocked 유지)
            SetUnlocked();
        }
        else
        {
            SetLocked();
        }
    }

    void SetLocked()
    {
        img.sprite = lockedSprite;
        button.interactable = false;
        if (txt != null) txt.SetActive(false);
    }

    void SetUnlocked()
    {
        img.sprite = unlockedSprite;
        button.interactable = true;
        if (txt != null) txt.SetActive(true);
    }
}
