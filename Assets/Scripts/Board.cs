using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;
using UnityUtility.Linq;
using UnityUtility.Collections;

public class Board : MonoBehaviour
{
    [SerializeField]
    private Vector2Int size;
    [SerializeField]
    private Square originalSquare;

    private Matrix<Square> squareMatrix;

    void Start() {
        squareMatrix = new Matrix<Square>(size.y, size.x);

        EnumerableFactory
            .FromVector2Int(size)
            .ForEach(coord => {
                var newSquare = Instantiate(originalSquare, transform);
                var squareSize = newSquare.SpriteSize;
                newSquare.transform.position = new Vector2(coord.x * squareSize.x, coord.y * squareSize.y);

                // 行列にセット
                squareMatrix.Set(newSquare, coord.y, coord.x);
            });
        // 中心に持ってくる
        this.transform.position = new Vector2(
            this.transform.position.x - originalSquare.SpriteSize.x * (size.x - 1) / 2,
            this.transform.position.y - originalSquare.SpriteSize.y * (size.y - 1) / 2
        );

        // 中心のマスに初期石をセットする
        squareMatrix.Get(size.x / 2 - 1, size.y / 2 - 1).SetWhite();
        squareMatrix.Get(size.x / 2, size.y / 2).SetWhite();
        squareMatrix.Get(size.x / 2, size.y / 2 - 1).SetBlack();
        squareMatrix.Get(size.x / 2 - 1, size.y / 2).SetBlack();
    }
}
