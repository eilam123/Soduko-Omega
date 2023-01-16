namespace Soduko_Omega;
/// <summary>
/// A static class that implements various constraint propagation techniques for solving a Sudoku puzzle
/// </summary>
public static class ConstraintPropagation
{
    /// <summary>
    /// Method that runs all the constraint propagation techniques on the given board
    /// </summary>
    /// <param name="board">The board to run the constraint propagation techniques on</param>
    public static void RunConstraintPropagation(Board? board)
    {
        var succeedConstraintPropagation = 0;
        do
        {
            succeedConstraintPropagation = 0;
            if (NakedSingle(board))
                succeedConstraintPropagation++;
            if (HiddenSingle(board))
                succeedConstraintPropagation++;
            if (NakedPairs(board))
                succeedConstraintPropagation++;
           
        } while (succeedConstraintPropagation > 0);
    }
    /// <summary>
    /// Method that applies the Naked Single constraint propagation technique on the entire board
    /// </summary>
    /// <param name="board">The board to apply the technique on</param>
    /// <returns>True if the technique was successful, False otherwise</returns>
    public static bool NakedSingle(Board? board)
    {
        var changed = false;
        for (var row = 0; row < board.boardLen; row++)
        for (var col = 0; col < board.boardLen; col++)
            if (board.board[row, col].PossibleValues.Count == 1)
            {
                board.board[row, col].Value = board.board[row, col].PossibleValues[0];
                board.AmountOfFilledCells++;
                board.board[row, col].PossibleValues.Clear();
                RemoveValueOfCellFromPeers(board.board[row, col], board);
                changed = true;
            }

        return changed;
    }
    /// <summary>
    /// Method that removes the value of a specific cell from its peers' possible values
    /// </summary>
    /// <param name="cell">The cell whose value to remove</param>
    /// <param name="board">The board containing the cell and its peers</param>
    public static void RemoveValueOfCellFromPeers(Cell cell, Board board)
    {
        foreach (var (row, col) in cell.Peers) board.board[row, col].PossibleValues.Remove(cell.Value);
    }
    /// <summary>
    /// Method that applies the Naked Pairs constraint propagation technique on the entire board
    /// </summary>
    /// <param name="board">The board to apply the technique on</param>
    /// <returns>True if the technique was successful, False otherwise</returns>
    public static bool NakedPairs(Board? board)
    {
        var changed = false;
        for (var row = 0; row < board.boardLen; row++)
        for (var col = 0; col < board.boardLen; col++)
            if (board.board[row, col].PossibleValues.Count == 2)
                foreach (var (row2, col2) in board.board[row, col].Peers)
                    if (board.board[row2, col2].PossibleValues.Count == 2)
                        if (board.board[row, col].PossibleValues[0] == board.board[row2, col2].PossibleValues[0] &&
                            board.board[row, col].PossibleValues[1] == board.board[row2, col2].PossibleValues[1])
                            foreach (var (row3, col3) in board.board[row, col].Peers
                                         .Intersect(board.board[row2, col2].Peers))
                            {
                                if (board.board[row3, col3].PossibleValues
                                    .Remove(board.board[row, col].PossibleValues[0]))
                                    changed = true;

                                if (board.board[row3, col3].PossibleValues
                                    .Remove(board.board[row, col].PossibleValues[1]))
                                    changed = true;
                            }

        return changed;
    }


    /// <summary>
    /// Method that applies the Hidden Single constraint propagation technique on a specific row
    /// </summary>
    /// <param name="cell">The cell to use as a starting point for the row</param>
    /// <param name="board">The board containing the cell and its peers</param>
    /// <returns>True if the technique was successful, False otherwise</returns>
    public static bool HiddenSingle(Board? board)
    {
        var happened = false;
        for (var row = 0; row < board.boardLen; row++)
        for (var col = 0; col < board.boardLen; col++)
            if (row == col)
                if (HiddenSingleForRow(board.board[row, col], board) ||
                    HiddenSingleForCol(board.board[row, col], board))
                    happened = true;

        for (var row = 0; row < board.boardLen; row += (int)Math.Sqrt(board.boardLen))
        for (var col = 0; col < board.boardLen; col += (int)Math.Sqrt(board.boardLen))
            if (HiddenSingleForBox(board.board[row, col], board))
                happened = true;

        return happened;
    }


    public static bool HiddenSingleForRow(Cell cell, Board board)
    {
        var lists = new List<char>[cell.RowPeers2.Count];
        var j = 0;
        foreach (var (row, col) in cell.RowPeers2)
        {
            lists[j] = board.board[row, col].PossibleValues;
            j++;
        }


        var uniqueChars = lists.SelectMany((l, index) => l.Select(c => (c, index)))
            .GroupBy(p => p.c)
            .Where(g => g.Count() == 1)
            .Select(g => g.First())
            .ToList();
        if (uniqueChars.Count == 0)
            return false;
        foreach (var (c, index) in uniqueChars)
        {
            var (row, col) = cell.RowPeers2[index];
            board.board[row, col].Value = c;
            board.board[row, col].PossibleValues.Clear();
            RemoveValueOfCellFromPeers(board.board[row, col], board);
        }

        return true;
    }

    public static bool HiddenSingleForBox(Cell cell, Board board)
    {
        var lists = new List<char>[cell.BoxPeers2.Count];
        var j = 0;
        foreach (var (row, col) in cell.BoxPeers2)
        {
            lists[j] = board.board[row, col].PossibleValues;
            j++;
        }


        var uniqueChars = lists.SelectMany((l, index) => l.Select(c => (c, index)))
            .GroupBy(p => p.c)
            .Where(g => g.Count() == 1)
            .Select(g => g.First())
            .ToList();
        if (uniqueChars.Count == 0)
            return false;
        foreach (var (c, index) in uniqueChars)
        {
            var (row, col) = cell.BoxPeers2[index];
            board.board[row, col].Value = c;
            board.board[row, col].PossibleValues.Clear();
            RemoveValueOfCellFromPeers(board.board[row, col], board);
        }

        return true;
    }

    public static bool HiddenSingleForCol(Cell cell, Board board)
    {
        var lists = new List<char>[cell.ColPeers2.Count];
        var j = 0;
        foreach (var (row, col) in cell.ColPeers2)
        {
            lists[j] = board.board[row, col].PossibleValues;
            j++;
        }


        var uniqueChars = lists.SelectMany((l, index) => l.Select(c => (c, index)))
            .GroupBy(p => p.c)
            .Where(g => g.Count() == 1)
            .Select(g => g.First())
            .ToList();
        if (uniqueChars.Count == 0)
            return false;
        foreach (var (c, index) in uniqueChars)
        {
            var (row, col) = cell.ColPeers2[index];
            board.board[row, col].Value = c;
            board.board[row, col].PossibleValues.Clear();
            RemoveValueOfCellFromPeers(board.board[row, col], board);
        }

        return true;
    }
 
}