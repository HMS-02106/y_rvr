using TMPro;
using UnityEngine;

public class ReversiSceneSwitcher : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI widthTextField;
    [SerializeField]
    private TextMeshProUGUI heightTextField;
    public void Switch()
    {
        // なぜか ZERO WIDTH SPACEが含まれてしまうので削除する
        string widthText = widthTextField.text.Replace("\u200B", "").Trim();
        string heightText = heightTextField.text.Replace("\u200B", "").Trim();

        int width = int.Parse(widthText);
        int height = int.Parse(heightText);

        // 盤面サイズをPlayerPrefsに保存
        PlayerPrefs.SetInt("ReversiWidth", width);
        PlayerPrefs.SetInt("ReversiHeight", height);
        // シーン遷移
        UnityEngine.SceneManagement.SceneManager.LoadScene("Reversi");
    }
}
