using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void ChangeScene()
    {
        SceneManager.LoadSceneAsync("Location");
    }
}
