using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class BlockUI : MonoBehaviour, IPointerClickHandler
{
    public TMP_Text label;

    public int index;
    Image background;

    Color normalColor = Color.white;
    Color selectedColor = Color.yellow;
    Color executingColor = Color.green;

    void Awake()
    {
        background = GetComponent<Image>();
    }

    public void Init(int index, int commandType)
    {
        this.index = index;
        SetNormal();

        label.text = GetCommandName(commandType);
    }
    string GetCommandName(int type)
    {
        switch (type)
        {
            case 0: return "앞으로 전진";
            case 1: return "왼쪽으로 회전";
            case 2: return "오른쪽으로 회전";
            case 3: return "점프";
            case 4: return "버튼 누르기";
            case 5: return "당근 획득";
        }
        return "알 수 없음";
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (LevelManager.Instance.IsPlaying()) return;
        LevelManager.Instance.SelectBlock(index);
    }

    public void SetSelected(bool selected)
    {
        background.color = selected ? selectedColor : normalColor;
    }

    public void SetExecuting(bool executing)
    {
        background.color = executing ? executingColor : normalColor;
    }

    public void SetNormal()
    {
        background.color = normalColor;
    }
}
