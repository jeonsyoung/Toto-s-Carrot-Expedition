using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClick : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene("LevelSet");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
