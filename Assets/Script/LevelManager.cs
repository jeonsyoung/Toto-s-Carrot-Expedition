using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public int currentClearLevel = 0; // 0이면 1레벨만 열림
    public int maxLevel = 15;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ClearLevel()
    {
        if (currentClearLevel < maxLevel)
            currentClearLevel++;
    }
}

