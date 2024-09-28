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
    bool Validate(MatrixIndex matrixIndex, StoneStatus stoneStatus);
    /// <summary>
    /// 石を置く
    /// </summary>
    /// <param name="matrixIndex">置く場所</param>
    /// <param name="stoneStatus">置く石の色</param>
    void Put(MatrixIndex matrixIndex, StoneStatus stoneStatus);
}
public class StoneFlipper : IStoneFlipper
{
    private IBoard board;

    public StoneFlipper(IBoard board)
    {
        this.board = board;
    }

    public bool Validate(MatrixIndex matrixIndex, StoneStatus stoneStatus)
    {
        // すでに置かれている場所には置けない
        if (board.Squares.Get(matrixIndex).Stone.IsExist) {
            return false;
        }
        foreach(var stones in GetFlippableStonesPerDirection(matrixIndex, stoneStatus)) {
            if (stones.Any(stone => stone.Status == stoneStatus)) {
                return true;
            }
        }   
        return false;
    }
    public void Put(MatrixIndex matrixIndex, StoneStatus putStoneStatus)
    {
        // まず全方向に対して放射状にStoneを取得して
        GetFlippableStonesPerDirection(matrixIndex, putStoneStatus)
            // 同一方向に同じ色のStoneがある場合のみひっくり返す対象
            .Where(stones => stones.Any(stone => stone.Status == putStoneStatus))
            // 同じ色が来るまで繰り返す
            .Select(stones => stones.TakeUntil(stone => stone.Status == putStoneStatus).ToArray())
            .SelectMany(stones => stones)
            .ForEach(stone => stone.Status = putStoneStatus);
    }

    /// <summary>
    /// 石を置いたときにひっくり返す対象となる石の列を取得する
    /// </summary>
    /// <param name="matrixIndex">石を置く位置</param>
    /// <param name="stoneStatus">石の色</param>
    /// <returns></returns>
    private IEnumerable<IEnumerable<IStone>> GetFlippableStonesPerDirection(MatrixIndex matrixIndex, StoneStatus stoneStatus) =>
        EnumUtils.All<Direction8>()
            .Where(direction =>
            {
                // 石を置く位置を起点として、隣に別の色の石がなければならない
                var isOpposite = (board.Squares.Get(matrixIndex, direction)?.Stone)?.IsOpposite(stoneStatus);
                return isOpposite.HasValue && isOpposite.Value;
            })
            .Select(direction => board.GetDirectionEnumerable(matrixIndex, direction).Select(sq => sq.Stone));
}
