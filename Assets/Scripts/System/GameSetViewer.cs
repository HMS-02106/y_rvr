using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetViewer : MonoBehaviour
{
    [SerializeField]
    private PassAndGameEndDetector passAndGameEndDetector;
    [SerializeField]
    private GameObject gameSetPanel;

    void Start()
    {
        gameSetPanel.SetActive(false);

        // ゲーム終了時にゲームセットパネルを表示する
        passAndGameEndDetector.OnGameEnd += () =>
        {
            gameSetPanel.SetActive(true);
        };
    }

}
