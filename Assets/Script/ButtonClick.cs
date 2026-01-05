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
        LevelManager.Instance.AddCommand(CodingType);
    }

    public void BlockRemove()
    {
        LevelManager.Instance.RemoveCommand(LevelManager.Instance.selectedIndex);
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
