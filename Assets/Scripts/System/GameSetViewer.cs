using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetViewer : MonoBehaviour
{
    [SerializeField]
    private PassAndGameEndDetector passAndGameEndDetector;
    [SerializeField]
    private GameObject gameSetPanel;
    [SerializeField]
    private GameObject returnButton;

    void Start()
    {
        gameSetPanel.SetActive(false);
        returnButton.SetActive(false);

        passAndGameEndDetector.OnGameEnd += () =>
        {
            gameSetPanel.SetActive(true);
            returnButton.SetActive(true);
        };
    }

}
