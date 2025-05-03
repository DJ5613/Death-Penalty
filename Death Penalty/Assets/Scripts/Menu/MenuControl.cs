using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    [SerializeField] private GameObject basePanel;
    [SerializeField] private GameObject rulesPanel;


    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void RulesButt()
    {
        basePanel.SetActive(false);
        rulesPanel.SetActive(true);
    }

    public void BaseButt()
    {
        basePanel.SetActive(true);
        rulesPanel.SetActive(false);
    }

}