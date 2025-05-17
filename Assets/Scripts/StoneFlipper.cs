using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityUtility.Collections;
using UnityUtility.Enums;
using UnityUtility.Linq;
using MoreLinq.Extensions;

public class StoneFlipper
{
    private Board board;

    public StoneFlipper(Board board)
    {
        this.board = board;
    }

    /// <summary>
    /// 石を置く
    /// </summary>
    /// <param name="matrixIndex">置く場所</param>
    /// <param name="stoneStatus">置く石の色</param>
    /// <returns>石を置くことができたかどうか</returns>
    public bool Put(MatrixIndex matrixIndex, StoneColor putStoneColor)
    {
        // ここに石を置いたことでひっくり返る石を取得する
        var flippableSquares = GetFlippableSquareSequencesPerDirection(matrixIndex, putStoneColor).ToList();
        if (flippableSquares.Count == 0)
        {
            // ひっくり返すことができない場合は何もしない
            return false;
        }

        // indexに指定の色の石を置く
        board.Squares.Get(matrixIndex).StoneStatus = putStoneColor.ToStoneStatus();
        // 色を変える
        flippableSquares.ForEach(square => square.StoneStatus = putStoneColor.ToStoneStatus());
        return true;
    }

    /// <summary>
    /// 石を置いたときにひっくり返す対象となるマスの列を取得する
    /// </summary>
    /// <param name="matrixIndex">石を置く位置</param>
    /// <param name="stoneColor">石の色</param>
    /// <returns></returns>
    public IEnumerable<Square> GetFlippableSquareSequencesPerDirection(MatrixIndex matrixIndex, StoneColor stoneColor)
    {
        // すでに置かれている場所には置けない
        if (board.Squares.Get(matrixIndex).IsStoneExists)
        {
            return Enumerable.Empty<Square>();
        }
        return EnumUtils.All<Direction8>()
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
            .Where(squareSq => squareSq.ContainsColor(stoneColor))
            .SelectMany(squareSq => squareSq.TakeUntil(sq => sq.StoneColor == stoneColor));
    }
}
