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
        foreach (var squares in GetFlippableSquaresPerDirection(matrixIndex, stoneColor))
        {
            // ひっくり返す対象の石があるか
            if (squares.Any(square => square.StoneColor == stoneColor))
            {
                return true;
            }
        }   
        return false;
    }
    public void Put(MatrixIndex matrixIndex, StoneColor putStoneColor)
    {
        // indexに指定の色の石を置く
        board.Squares.Get(matrixIndex).StoneStatus = putStoneColor.ToStoneStatus();

        // 次に、その石を置いたことでひっくり返る石を取得し、色を変える
        // まず全方向に対して放射状にStoneを取得して
        GetFlippableSquaresPerDirection(matrixIndex, putStoneColor)
            // 同一方向に同じ色のStoneがある場合のみひっくり返す対象
            .Where(squares => squares.Any(square => square.StoneColor == putStoneColor))
            // 同じ色が来るまで繰り返す
            .Select(squares => squares.TakeUntil(square => square.StoneColor == putStoneColor).ToArray())
            .SelectMany(squares => squares)
            .ForEach(square => square.StoneStatus = putStoneColor.ToStoneStatus());
    }

    /// <summary>
    /// 石を置いたときにひっくり返す対象となる石の列を取得する
    /// </summary>
    /// <param name="matrixIndex">石を置く位置</param>
    /// <param name="stoneColor">石の色</param>
    /// <returns></returns>
    private IEnumerable<IEnumerable<Square>> GetFlippableSquaresPerDirection(MatrixIndex matrixIndex, StoneColor stoneColor) =>
        EnumUtils.All<Direction8>()
            .Where(direction =>
            {
                // 石を置く位置を起点として、隣に別の色の石がなければならない
                var adjacentStoneColor = board.Squares.Get(matrixIndex, direction)?.StoneColor;
                return adjacentStoneColor != null && adjacentStoneColor != stoneColor;
            })
            .Select(direction => board.GetDirectionEnumerable(matrixIndex, direction));
}
