using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClick : MonoBehaviour
{

    public string SceneName;
    public void SceneChange()
    {
        SceneManager.LoadScene(SceneName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
