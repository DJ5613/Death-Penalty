using TMPro;
using UnityEngine;

public class ResultsMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI kill_enemies;
    [SerializeField] private TextMeshProUGUI diff;

    static public int kill_score = 0;

    private string[] difficults = { "Лёгкая", "Средняя", "Сложная" };
    private void OnEnable()
    {
        int minutes = Mathf.FloorToInt(Timer.currentTime / 60f);
        int seconds = Mathf.FloorToInt(Timer.currentTime % 60f);
        timer.text = "Время: " + string.Format("{0:00}:{1:00}", minutes, seconds);

        kill_enemies.text = "Врагов убито: " + kill_score.ToString();

        diff.text = "Сложность: " + difficults[SwitchDifficulty.dif_num - 1];
    }
}
