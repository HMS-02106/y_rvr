using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SquareSequence : IEnumerable<Square>
{
    private readonly IReadOnlyList<Square> squares;
    public SquareSequence(IReadOnlyList<Square> squares)
    {
        this.squares = squares;
    }

    public bool ContainsColor(StoneColor color) => squares.Any(square => square.StoneColor == color);

    public IEnumerator<Square> GetEnumerator()
    {
        return squares.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)squares).GetEnumerator();
    }
}