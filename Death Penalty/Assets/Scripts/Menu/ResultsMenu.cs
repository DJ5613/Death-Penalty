using TMPro;
using UnityEngine;

public class ResultsMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI status;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI kill_enemies;
    [SerializeField] private TextMeshProUGUI diff;

    static public int kill_score = 0;

    private string[] difficults = { "Лёгкая", "Средняя", "Сложная" };

    private void Start()
    {
        kill_score = 0;
    }

    private void OnEnable()
    {
        if (DamageDetector.playerHP <= 0)
        {
            status.text = "Вы проебали!";
        }
        else
        {
            status.text = "Вы нахуй в это играли?";
        }
        int minutes = Mathf.FloorToInt(Timer.currentTime / 60f);
        int seconds = Mathf.FloorToInt(Timer.currentTime % 60f);
        timer.text = "Время: " + string.Format("{0:00}:{1:00}", minutes, seconds);

        kill_enemies.text = "Врагов убито: " + kill_score.ToString();

        diff.text = "Сложность: " + difficults[SwitchDifficulty.dif_num - 1];

        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
}
