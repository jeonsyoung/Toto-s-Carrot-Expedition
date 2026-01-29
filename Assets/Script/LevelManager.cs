using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public List<int> CodingList = new List<int>();
    public int selectedIndex = -1;

    bool isPlaying = false;
    Coroutine playCoroutine;

    public IngameCoding ingame;

    public GameObject blockPrefab;
    public GameObject arrowPrefab;
    public Transform content;
    public ScrollRect scrollRect;

    [Header("Play Button UI")]
    public Image playButtonImage;
    public Sprite playSprite;        // 기본
    public Sprite playingSprite;     // 실행 중

    bool isCleared = false;

    [Header("Carrot UI")] 
    public GameObject carrotIcon;

    [Header("Score")]
    public int optimalBlockCount = 5; // 이 레벨 최소 해답

    [Header("Star UI")]
    public GameObject star1;
    public GameObject star2;
    public GameObject star3;

    public GameObject ScorePanel;

    public GameObject carrotStar;

    public TextMeshProUGUI starText;

    void ShowStars(int score)
    {
        star1.SetActive(score >= 1);
        star2.SetActive(score >= 2);
        star3.SetActive(score >= 3);
    }
    void SaveScore(int score)
    {
        string key = "LevelScore_" + SceneManager.GetActiveScene().name;

        int best = PlayerPrefs.GetInt(key, 0);

        if (score > best)
        {
            PlayerPrefs.SetInt(key, score);
            PlayerPrefs.Save();
        }
    }

    int CalculateScore()
    {
        int used = CodingList.Count;

        if (used <= optimalBlockCount)
            return 3;
        else if (used <= optimalBlockCount + 3)
            return 2;
        else
            return 1;
    }

    public void ShowCarrotIcon()
    {
        if (carrotIcon != null)
            carrotIcon.SetActive(true);
    }

    public void HideCarrotIcon()
    {
        if (carrotIcon != null)
            carrotIcon.SetActive(false);
    }


    void Awake()
    {
        Instance = this;
        ScorePanel.SetActive(false);
    }

    // ================= 상태 =================
    public bool IsPlaying() => isPlaying;

    // ================= 추가 =================
    public void AddCommand(int type)
    {
        int insertIndex = selectedIndex == -1 ? CodingList.Count : selectedIndex + 1;
        CodingList.Insert(insertIndex, type);

        RebuildUI();
        ClearSelection();
    }

    // ================= 삭제 =================
    public void RemoveCommand(int index)
    {
        if (index < 0 || index >= CodingList.Count) return;

        CodingList.RemoveAt(index);
        RebuildUI();
        ClearSelection();
    }

    // ================= 실행 =================
    public void PlayCode()
    {
        if (isPlaying) return;

        isCleared = false; 
        isPlaying = true;
        UpdatePlayButtonUI(true);
        playCoroutine = StartCoroutine(PlayRoutine());
    }


    IEnumerator PlayRoutine()
    {
        for (int i = 0; i < CodingList.Count; i++)
        {
            HighlightExecuting(i);
            ScrollToBlock(i);
            yield return null;
            yield return ExecuteCommand(CodingList[i]);

            if (isCleared)
                yield break;
        }

        // 명령 다 끝났는데 포탈 못 갔으면 실패
        StopCode();
    }


    void FinishPlay()
    {
        isPlaying = false;
        UpdatePlayButtonUI(false);
    }


    IEnumerator ExecuteCommand(int cmd)
    {
        switch (cmd)
        {
            case 0: yield return ingame.MoveForward(); break;
            case 1: yield return ingame.TurnRight(); break;
            case 2: yield return ingame.TurnLeft(); break;
            case 3: yield return ingame.Jump(); break;
            case 4: yield return ingame.PushButton(); break;
            case 5: yield return ingame.CarrotGet(); break;
        }
    }

    public void StopCode()
    {
        if (!isPlaying) return;

        if (playCoroutine != null)
            StopCoroutine(playCoroutine);

        ClearHighlight();

        if (!isCleared)
        {
            ingame.ResetRabbit();

            // 당근 UI 리셋
            if (carrotIcon != null)
                carrotIcon.SetActive(false);
        }

        FinishPlay();
    }




    // ================= UI =================
    void RebuildUI()
    {
        foreach (Transform t in content) Destroy(t.gameObject);

        for (int i = 0; i < CodingList.Count; i++)
        {
            if (i > 0) Instantiate(arrowPrefab, content);

            GameObject block = Instantiate(blockPrefab, content);
            block.GetComponent<BlockUI>().Init(i, CodingList[i]);
        }
    }

    public void SelectBlock(int index)
    {
        selectedIndex = index;
        foreach (Transform t in content)
        {
            BlockUI ui = t.GetComponent<BlockUI>();
            if (ui != null) ui.SetSelected(ui.index == index);
        }
    }

    void HighlightExecuting(int index)
    {
        foreach (Transform t in content)
        {
            BlockUI ui = t.GetComponent<BlockUI>();
            if (ui != null) ui.SetExecuting(ui.index == index);
        }
    }

    void ClearHighlight()
    {
        foreach (Transform t in content)
        {
            BlockUI ui = t.GetComponent<BlockUI>();
            if (ui != null) ui.SetNormal();
        }
    }

    void ClearSelection()
    {
        selectedIndex = -1;
        ClearHighlight();
    }

    void UpdatePlayButtonUI(bool playing)
    {
        if (playButtonImage == null) return;

        playButtonImage.sprite = playing ? playingSprite : playSprite;
    }


    // ================= 스크롤 =================
    void ScrollToBlock(int index)
    {
        RectTransform contentRect = content as RectTransform;
        float viewportHeight = scrollRect.viewport.rect.height;

        float y = 0f;
        int blockCount = 0;

        foreach (Transform t in content)
        {
            BlockUI ui = t.GetComponent<BlockUI>();
            if (ui != null)
            {
                if (ui.index == index) break;
                blockCount++;
            }
            y += ((RectTransform)t).rect.height;
        }

        float normalized = 1f - Mathf.Clamp01((y - viewportHeight * 0.5f) /
                                             (contentRect.rect.height - viewportHeight));
        scrollRect.verticalNormalizedPosition = normalized;
    }

    public void OnReachPortal()
    {
        if (!isPlaying) return;

        isCleared = true;

        int score = CalculateScore();

        ShowStars(score); 
        SaveScore(score);

        ClearHighlight();
        FinishPlay();

        Debug.Log("클리어! 점수: " + score);
        carrotStar.SetActive(false);

        GameManager.Instance.s_Stars += score;
        if (carrotIcon != null && carrotIcon.activeSelf)
        {
            GameManager.Instance.s_Stars += 1;
            carrotStar.SetActive(true);
        }
        starText.text = GameManager.Instance.s_Stars.ToString();

        ScorePanel.SetActive(true);

        int clearedLevel = GameManager.Instance.currentPlayingLevel;

        if (clearedLevel == GameManager.Instance.currentClearLevel + 1)
        {
            GameManager.Instance.currentClearLevel++;
        }

        GameManager.Instance.SaveData();

    }

    public void OnCarrotCollected()
    {
        if (carrotIcon != null)
            carrotIcon.SetActive(true);

        Debug.Log("당근 획득!");
    }

}
