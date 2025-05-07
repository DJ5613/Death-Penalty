using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject hidePanel;
    [SerializeField] private GameObject showPanel;

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SwitchPanel()
    {
        hidePanel.SetActive(false);
        showPanel.SetActive(true);
    }
}