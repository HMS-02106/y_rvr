using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;

public interface IObservableScore
{
    Observable<int> ObservableBlackScore { get; }
    Observable<int> ObservableWhiteScore { get; }
}
