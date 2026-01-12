using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{
    public string SceneName;
    public int CodingType;

    public void SceneChange()
    {
        SceneManager.LoadScene(SceneName);
    }

    public void BlockCreate()
    {
        if (LevelManager.Instance.IsPlaying()) return;
        LevelManager.Instance.AddCommand(CodingType);
    }

    public void BlockRemove()
    {
        if (LevelManager.Instance.IsPlaying()) return;

        LevelManager lm = LevelManager.Instance;

        // 블록이 하나도 없으면 종료
        if (lm.CodingList.Count == 0) return;

        // 선택 안 했으면 맨 뒤 삭제
        int removeIndex = lm.selectedIndex == -1
            ? lm.CodingList.Count - 1
            : lm.selectedIndex;

        lm.RemoveCommand(removeIndex);
    }


    public void PlayCode()
    {
        LevelManager.Instance.PlayCode();
    }

    public void StopCode()
    {
        LevelManager.Instance.StopCode();
    }
}
