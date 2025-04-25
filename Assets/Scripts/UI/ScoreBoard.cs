using TMPro;
using UnityEngine;
using R3;
using Sirenix.OdinInspector;

public class ScoreBoard : SerializedMonoBehaviour
{
    [SerializeField]
    private IObservableScore observableScore;
    [SerializeField]
    private TextMeshProUGUI blackText;
    [SerializeField]
    private TextMeshProUGUI whiteText;

    void Start()
    {
        observableScore.ObservableBlackScore.Subscribe(score => blackText.text = score.ToString());
        observableScore.ObservableWhiteScore.Subscribe(score => whiteText.text = score.ToString());
    }
}
