using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public int currentClearLevel = 0; // 0이면 1레벨만 열림
    public int maxLevel = 15;
    public int blockCount;

    public List<int> CodingList = new List<int>();
    public int selectedIndex = -1;

    Coroutine playCoroutine;
    bool isPlaying = false;

    public IngameCoding ingame;


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

    public void AddCommand(int type)
    {
        if (selectedIndex == -1)
            CodingList.Add(type);
        else
            CodingList.Insert(selectedIndex + 1, type);

        Debug.Log("추가됨: " + type);
    }

    public void RemoveCommand(int index)
    {
        if (index < 0 || index >= CodingList.Count) return;

        CodingList.RemoveAt(index);
        selectedIndex = -1;
    }

    public void PlayCode()
    {
        Debug.Log("코드 실행");
        if (isPlaying) return;

        isPlaying = true;
        playCoroutine = StartCoroutine(PlayRoutine());
    }

    IEnumerator PlayRoutine()
    {
        for (int i = 0; i < CodingList.Count; i++)
        {
            if (!isPlaying)
                yield break;

            yield return ExecuteCommand(CodingList[i]);
        }

        isPlaying = false;
    }

    IEnumerator ExecuteCommand(int cmd)
    {
        switch (cmd)
        {
            case 0: // 앞으로
                yield return ingame.MoveForward();
                break;
            case 1:
                yield return ingame.TurnRight();
                break;
            case 2:
                yield return ingame.TurnLeft();
                break;
            case 3:
                yield return ingame.Jump();
                break;
            case 4:
                yield return ingame.PushButton();
                break;
            case 5:
                yield return ingame.CarrotGet();
                break;

        }
    }
    public void StopCode()
    {
        if (!isPlaying) return;

        isPlaying = false;

        if (playCoroutine != null)
        {
            StopCoroutine(playCoroutine);
            playCoroutine = null;
        }
    }

}

