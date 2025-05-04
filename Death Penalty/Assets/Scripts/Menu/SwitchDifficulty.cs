using UnityEngine;
using UnityEngine.UI;

public class SwitchDifficulty : MonoBehaviour
{
    [SerializeField] Button _buttonRight;
    [SerializeField] Button _buttonLeft;
    [SerializeField] GameObject easyDifficulty;
    [SerializeField] GameObject mediumDifficulty;
    [SerializeField] GameObject hardDifficulty;
    static public int dif_num = 1;

    public void NextDifficult(bool left_button, bool right_button)
    {
        if (left_button) dif_num--;
        else dif_num++;
        if (dif_num > 3) dif_num = 1;
        if (dif_num < 1) dif_num = 3;        
        UpdateDifficulty();
    }
    private void UpdateDifficulty()
    {
        if (easyDifficulty != null) easyDifficulty.SetActive(dif_num == 1);
        if (mediumDifficulty != null) mediumDifficulty.SetActive(dif_num == 2);
        if (hardDifficulty != null) hardDifficulty.SetActive(dif_num == 3);
    }

    private void Awake()
    {
        _buttonRight.onClick.AddListener(() => NextDifficult(false, true));
        _buttonLeft.onClick.AddListener(() => NextDifficult(true, false));
    }
}
