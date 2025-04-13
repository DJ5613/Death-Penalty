using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Management;

public class StartGame : MonoBehaviour
{
    public void ChangeScene()
    {        
        SceneManager.LoadSceneAsync("Location");
    }
}
