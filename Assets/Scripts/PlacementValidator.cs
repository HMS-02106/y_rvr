using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityUtility.Collections;
using UnityUtility.Enums;

public interface IPlacementValidator {
    /// <summary>
    /// マスに石を置けるか判定する
    /// </summary>
    /// <param name="matrixIndex">置きたいマスの座標</param>
    /// <param name="stoneStatus">置きたい石の色</param>
    /// <returns></returns>
    bool Validate(MatrixIndex matrixIndex, StoneStatus stoneStatus);
}
public class PlacementValidator : IPlacementValidator
{
    private IReadOnlyBoard board;

    public PlacementValidator(IReadOnlyBoard board)
    {
        this.board = board;
    }

    public bool Validate(MatrixIndex matrixIndex, StoneStatus stoneStatus)
    {
        var greppedDirectionStones = EnumUtils.All<Direction8>()
            .Where(direction =>
            {
                var isOpposite = (board.Squares.Get(matrixIndex, direction)?.Stone)?.IsOpposite(stoneStatus);
                return isOpposite.HasValue && isOpposite.Value;
            })
            .Select(direction => board.GetDirectionEnumerable(matrixIndex, direction).Select(sq => sq.Stone));
        foreach(var stones in greppedDirectionStones) {
            if (stones.Any(stone => stone.Status == stoneStatus)) {
                return true;
            }
        }   
        return false;
    }
}
