using TMPro;
using UnityEngine;
using System.Threading.Tasks;

public class ReversiSceneSwitcher : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI widthTextField;
    [SerializeField]
    private TextMeshProUGUI heightTextField;
    [SerializeField]
    private TMP_Dropdown pointWeightDropDown;
    [SerializeField]
    private TextMeshProUGUI minRangeTextField;
    [SerializeField]
    private TextMeshProUGUI maxRangeTextField;
    [SerializeField]
    private TextMeshProUGUI errorTextField;

    void Start()
    {
        errorTextField.text = "";
    }

    public void Switch()
    {
        // なぜか ZERO WIDTH SPACEが含まれてしまうので削除する
        string widthText = widthTextField.text.Replace("\u200B", "").Trim();
        string heightText = heightTextField.text.Replace("\u200B", "").Trim();
        string minRangeText = minRangeTextField.text.Replace("\u200B", "").Trim();
        string maxRangeText = maxRangeTextField.text.Replace("\u200B", "").Trim();
        // ウェイトタイプを取得
        string pointWeightText = pointWeightDropDown.options[pointWeightDropDown.value].text;

        if (!int.TryParse(widthText, out int width))
        {
            _ = showError("Please enter an integer for width");
            return;
        }
        if (!int.TryParse(heightText, out int height))
        {
            _ = showError("Please enter an integer for height");
            return;
        }
        if (width < 4 || height < 4)
        {
            _ = showError("Please enter a width and height of at least 4");
            return;
        }
        if (!int.TryParse(minRangeText, out int minRange))
        {
            _ = showError("Please enter an integer for min range");
            return;
        }
        if (!int.TryParse(maxRangeText, out int maxRange))
        {
            _ = showError("Please enter an integer for max range");
            return;
        }
        if (minRange > maxRange)
        {
            _ = showError("Please enter a min range less than max range");
            return;
        }

        // 盤面サイズをPlayerPrefsに保存
        PlayerPrefs.SetInt("ReversiWidth", width);
        PlayerPrefs.SetInt("ReversiHeight", height);
        PlayerPrefs.SetInt("ReversiMinPoint", minRange);
        PlayerPrefs.SetInt("ReversiMaxPoint", maxRange);
        PlayerPrefs.SetString("ReversiPointWeight", pointWeightText);
        // シーン遷移
        UnityEngine.SceneManagement.SceneManager.LoadScene("Reversi");
    }

    private async Task showError(string message)
    {
        errorTextField.text = message;
        await Task.Delay(2000);
        errorTextField.text = "";
    }
}
