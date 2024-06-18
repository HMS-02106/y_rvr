using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;
using UnityUtility.Linq;
using UnityUtility.Extensions;

public class Board : MonoBehaviour
{
    [SerializeField]
    private Vector2Int size;
    [SerializeField]
    private Square square;

    void Start() {
        EnumerableFactory
            .FromVector2Int(size)
            .ForEach(coord => {
                var newSquare = Instantiate(square, transform);
                var squareSize = newSquare.SpriteSize;
                newSquare.transform.position = new Vector2(coord.x * squareSize.x, coord.y * squareSize.y);
            });
    }
}
