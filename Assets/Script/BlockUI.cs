using UnityEngine;
using UnityEngine.UI;

public class BlockUI : MonoBehaviour
{
    public int index;
    public Button button;

    public void Init(int index)
    {
        this.index = index;
        button.onClick.AddListener(Select);
    }

    void Select()
    {
        LevelManager.Instance.selectedIndex = index;
        Debug.Log("선택된 블록: " + index);
    }
}
