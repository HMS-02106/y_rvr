using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class InfoViewer : MonoBehaviour
{
    [SerializeField]
    private PassAndGameEndDetector passAndGameEndDetector;
    [SerializeField]
    private TextMeshProUGUI infoText;

    void Start()
    {
        infoText.text = "";

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        passAndGameEndDetector.OnGameEnd += async () =>
        {
            cancellationTokenSource.Cancel();
            await Task.Delay(1500); // キャンセル完了まで待つ
            infoText.text = "GameSet!";
        };
        passAndGameEndDetector.OnPass += async color =>
        {
            infoText.text = $"{color} is passed.";
            try
            {
                await Task.Delay(1500, cancellationTokenSource.Token);
            }
            catch (TaskCanceledException)
            {
                // すでにキャンセルされている場合は無視
                Debug.Log("Task was canceled, ignoring delay.");
                return;
            }
            infoText.text = "";
        };
    }

}
