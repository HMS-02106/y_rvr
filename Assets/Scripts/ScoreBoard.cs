using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using R3;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField]
    private Board board;
    [SerializeField]
    private TextMeshProUGUI blackText;
    [SerializeField]
    private TextMeshProUGUI whiteText;

    void Start()
    {
        board.ObservableBlackScore.Subscribe(score => blackText.text = score.ToString());
        board.ObservableWhiteScore.Subscribe(score => whiteText.text = score.ToString());
    }
}
