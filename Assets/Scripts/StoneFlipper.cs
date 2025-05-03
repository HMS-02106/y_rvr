using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityUtility.Collections;
using UnityUtility.Enums;
using UnityUtility.Linq;
using MoreLinq.Extensions;

public interface IStoneFlipper {
    /// <summary>
    /// マスに石を置けるか判定する
    /// </summary>
    /// <param name="matrixIndex">置きたいマスの座標</param>
    /// <param name="stoneStatus">置きたい石の色</param>
    /// <returns></returns>
    bool Validate(MatrixIndex matrixIndex, StoneColor stoneStatus);
    /// <summary>
    /// 石を置く
    /// </summary>
    /// <param name="matrixIndex">置く場所</param>
    /// <param name="stoneStatus">置く石の色</param>
    void Put(MatrixIndex matrixIndex, StoneColor stoneStatus);
}
public class StoneFlipper : IStoneFlipper
{
    private IBoard board;

    public StoneFlipper(IBoard board)
    {
        this.board = board;
    }

    public bool Validate(MatrixIndex matrixIndex, StoneColor stoneColor)
    {
        // すでに置かれている場所には置けない
        if (board.Squares.Get(matrixIndex).IsStoneExists)
        {
            return false;
        }
        // 石を置いた場合に、少なくとも1方向にひっくり返すことができるか
        return GetFlippableSquareSequencesPerDirection(matrixIndex, stoneColor).Count() > 0;
    }
    public void Put(MatrixIndex matrixIndex, StoneColor putStoneColor)
    {
        // indexに指定の色の石を置く
        board.Squares.Get(matrixIndex).StoneStatus = putStoneColor.ToStoneStatus();

        // 次に、その石を置いたことでひっくり返る石を取得し、色を変える
        // まず、ひっくり返す対象となるマスの列を取得する
        GetFlippableSquareSequencesPerDirection(matrixIndex, putStoneColor)
            // 同じ色が来るまで繰り返す
            .Select(squareSq => squareSq.TakeUntil(square => square.StoneColor == putStoneColor).ToArray())
            .SelectMany(squares => squares)
            // 色を変える
            .ForEach(square => square.StoneStatus = putStoneColor.ToStoneStatus());
    }

    /// <summary>
    /// 石を置いたときにひっくり返す対象となるマスの列を取得する
    /// </summary>
    /// <param name="matrixIndex">石を置く位置</param>
    /// <param name="stoneColor">石の色</param>
    /// <returns></returns>
    private IEnumerable<SquareSequence> GetFlippableSquareSequencesPerDirection(MatrixIndex matrixIndex, StoneColor stoneColor) =>
        EnumUtils.All<Direction8>()
            // 置くマスの隣に石があり、かつその石の色が異なる方向に絞る
            .Where(direction =>
            {
                // 隣のマスの石の色を取得する
                var adjacentStoneColor = board.Squares.Get(matrixIndex, direction)?.StoneColor;
                // 隣のマスに石があり、かつその色が異なる場合
                return adjacentStoneColor != null && adjacentStoneColor != stoneColor;
            })
            // その方向にあるマスを全て取得する
            .Select(direction => board.GetDirectionSquareSequence(matrixIndex, direction))
            // その方向のマスに同じ色が含まれれば、そこまでひっくり返すことができる
            .Where(squareSq => squareSq.ContainsColor(stoneColor));
}
