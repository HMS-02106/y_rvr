using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;
using UnityUtility.Linq;
using UnityUtility.Collections;
using Sirenix.OdinInspector.Editor.Examples;
using UnityUtility.Enums;
using Sirenix.Reflection.Editor;
using R3;
using Unity.Mathematics;
using System.IO;
using System;

public interface IBoard {
    IReadOnlyMatrix<Square> Squares { get; }
    IEnumerable<Square> GetDirectionEnumerable(MatrixIndex origin, Direction8 direction);
}
public class Board : MonoBehaviour, IBoard, IObservableScore
{
    [SerializeField]
    private Vector2Int size;
    [SerializeField]
    private Square originalSquare;

    private Matrix<Square> squareMatrix;

    private Subject<int> blackScoreSubject = new();
    private Subject<int> whiteScoreSubject = new();

    public IReadOnlyMatrix<Square> Squares => squareMatrix;

    public Observable<int> ObservableBlackScore => blackScoreSubject.AsObservable();
    public Observable<int> ObservableWhiteScore => whiteScoreSubject.AsObservable();

    public IEnumerable<Square> GetDirectionEnumerable(MatrixIndex origin, Direction8 direction) => squareMatrix.GetDirectionEnumerator(origin, direction);

    void Start() {
        squareMatrix = new Matrix<Square>(size.y, size.x);

        StoneFlipper flipper = new StoneFlipper(this);
        StoneProvider stoneProvider = new StoneProvider();

        EnumerableFactory
            .FromVector2Int(size)
            .ForEach(coord => {
                Square square = Instantiate(originalSquare, transform);
                var squareSize = square.SpriteSize;
                square.transform.position = new Vector2(coord.x * squareSize.x, coord.y * squareSize.y);
                square.debugText.text = coord.ToString();

                MatrixIndex index = new MatrixIndex(coord.y, coord.x);
                //これStoneProviderから提供してもらって、Select(_ => StoneProvider.Provide)でStoneを受け取り、それをValidateすべきだよなあ
                square
                    .ObservableEnter
                    .Select(_ => stoneProvider.GetNextStoneStatus())
                    .Where(stoneStatus => flipper.Validate(index, stoneStatus))
                    .Subscribe(_ => square.BorderStatus = BorderStatus.Selected);
                square
                    .ObservableExit
                    .Subscribe(_ => square.BorderStatus = BorderStatus.None);
                square
                    .ObservableClick
                    .Select(_ => stoneProvider.GetNextStoneStatus())
                    .Where(stoneStatus => flipper.Validate(index, stoneStatus))
                    .Subscribe(s =>
                    {
                        square.StoneStatus = s;
                        stoneProvider.Switch();
                        flipper.Put(index, s);
                    });
                // squareの状態が変わったらスコアを更新
                square.ObservableStoneChanged
                    .Subscribe(_ =>
                    {
                        int black = 0, white = 0;
                        foreach (var square in squareMatrix)
                        {
                            if (square.IsBlack)
                            {
                                black++;
                            }
                            else if (square.IsWhite)
                            {
                                white++;
                            }
                        }
                        blackScoreSubject.OnNext(black);
                        whiteScoreSubject.OnNext(white);
                    });
                // 行列にセット
                squareMatrix.Set(square, index);
            });
        // 中心に持ってくる
        this.transform.position = new Vector2(
            this.transform.position.x - originalSquare.SpriteSize.x * (size.x - 1) / 2,
            this.transform.position.y - originalSquare.SpriteSize.y * (size.y - 1) / 2
        );

        // 中心のマスに初期石をセットする
        squareMatrix.Get(size.x / 2 - 1, size.y / 2 - 1).StoneStatus = StoneStatus.White;
        squareMatrix.Get(size.x / 2, size.y / 2).StoneStatus = StoneStatus.White;
        squareMatrix.Get(size.x / 2, size.y / 2 - 1).StoneStatus = StoneStatus.Black;
        squareMatrix.Get(size.x / 2 - 1, size.y / 2).StoneStatus = StoneStatus.Black;
    }

    
}
