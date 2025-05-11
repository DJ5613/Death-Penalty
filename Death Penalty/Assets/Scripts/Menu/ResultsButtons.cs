using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsButtons : MonoBehaviour
{
    public void BackToMenu(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
