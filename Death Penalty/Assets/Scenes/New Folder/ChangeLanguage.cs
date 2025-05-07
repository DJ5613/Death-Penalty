using UnityEngine;
using UnityEngine.UI;

public class ChangeLanguage : MonoBehaviour
{
    [SerializeField] Button _buttonRight;
    [SerializeField] Button _buttonLeft;
    [SerializeField] GameObject firstLanguage;
    [SerializeField] GameObject secondLanguage;
    [SerializeField] GameObject thirdLanguage;
    static public int lang = 1;

    public void NextLang(bool left_button, bool right_button)
    {
        if (left_button) lang--;
        else lang++;
        if (lang > 3) lang = 1;
        if (lang < 1) lang = 3;
        UpdateLang();
    }
    private void UpdateLang()
    {
        if (firstLanguage != null) firstLanguage.SetActive(lang == 1);
        if (secondLanguage != null) secondLanguage.SetActive(lang == 2);
        if (thirdLanguage != null) thirdLanguage.SetActive(lang == 3);
    }

    private void Awake()
    {
        _buttonRight.onClick.AddListener(() => NextLang(false, true));
        _buttonLeft.onClick.AddListener(() => NextLang(true, false));
    }
}
