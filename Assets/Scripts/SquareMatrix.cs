using UnityUtility.Collections;

public class SquareMatrix : Matrix<Square>
{
    public SquareMatrix(int rowSize, int columnSize) : base(rowSize, columnSize)
    {
    }

    public MatrixIndex[] GetCenterIndexes()
    {
        return new[] {
            new MatrixIndex(RowSize / 2 - 1, ColumnSize / 2 - 1),
            new MatrixIndex(RowSize / 2,     ColumnSize / 2),
            new MatrixIndex(RowSize / 2 - 1, ColumnSize / 2),
            new MatrixIndex(RowSize / 2,     ColumnSize / 2 - 1),
        };
    }

    public void SetInitialStones()
    {
        var centerIndexes = GetCenterIndexes();
        Get(centerIndexes[0]).StoneStatus = StoneStatus.White;
        Get(centerIndexes[1]).StoneStatus = StoneStatus.White;
        Get(centerIndexes[2]).StoneStatus = StoneStatus.Black;
        Get(centerIndexes[3]).StoneStatus = StoneStatus.Black;
    }
}