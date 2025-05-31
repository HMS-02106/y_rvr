using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using R3;
using UnityEngine.SocialPlatforms.Impl;

public class SoundSystem : SerializedMonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip putStoneSound;
    [SerializeField]
    private IObservableScore observableScore;

    private void Start()
    {
        observableScore
            .ObservableBlackScore
            .Merge(observableScore.ObservableWhiteScore)
            .Debounce(System.TimeSpan.FromSeconds(0.1f))
            .Subscribe(_ => audioSource.PlayOneShot(putStoneSound))
            .AddTo(this);
    }
}
