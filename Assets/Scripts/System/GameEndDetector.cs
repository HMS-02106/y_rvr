using UnityEngine;
using R3;
using System.Linq;

public class GameEndDetector : MonoBehaviour
{
    [SerializeField]
    private Board board;

    void Start()
    {
        // ターンが変わるたびにEmptyの個数を数える
        board.ObservableTurnChanged.Subscribe(_ =>
        {
            int emptyCount = board.Squares.Count(square => square.StoneStatus == StoneStatus.Empty);
            // Emptyがなくなったらゲーム終了
            if (emptyCount == 0)
            {
                Debug.Log("Game End");
            }
        });
    }
}
