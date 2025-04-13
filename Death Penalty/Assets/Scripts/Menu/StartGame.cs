using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Management;

public class StartGame : MonoBehaviour
{
    [SerializeField] int loc;
    public void ChangeScene()
    {        
        SceneManager.LoadSceneAsync(loc);
    }
}
