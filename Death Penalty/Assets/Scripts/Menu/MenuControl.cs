using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuControl : MonoBehaviour
{
    [SerializeField] private GameObject thisPanel;
    [SerializeField] private GameObject nextPanel;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SwitchPanel()
    {
        thisPanel.SetActive(false);
        nextPanel.SetActive(true);
    }
}