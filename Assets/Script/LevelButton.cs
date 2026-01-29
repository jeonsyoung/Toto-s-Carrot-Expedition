using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    public int Levelnum;

    [Header("Sprites")]
    public Sprite lockedSprite;
    public Sprite unlockedSprite;

    [Header("UI")]
    public GameObject txt;
    public Button button;

    Image img;

    void Start()
    {
        img = GetComponent<Image>();
        UpdateState();
    }

    public void OnClickLevel()
    {
        // 여기서 바로 넘김
        GameManager.Instance.currentPlayingLevel = Levelnum;

        SceneManager.LoadScene("Level"+ Levelnum);
    }

    public void UpdateState()
    {
        int cleared = GameManager.Instance.currentClearLevel;

        if (Levelnum <= cleared + 1)
            SetUnlocked();
        else
            SetLocked();
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
