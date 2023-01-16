namespace Soduko_Omega;

public static class ConstraintPropagation
{
    /*public static bool NakedSingle(AntBoard? board, Cell cell)
    {
        bool changed = false;
        for (int row = 0; row < board.boardLen; row++)
        {
            for (int col = 0; col < board.boardLen; col++)
            {
                if (board.board[row, col].PossibleValues.Count == 1)
                {
                    board.board[row, col].Value = board.board[row, col].PossibleValues[0];
                    board.board[row, col].PossibleValues.Clear();
                    RemoveValueOfCellFromPeers(board.board[row, col], board);
                    changed = true;
                }
            }
        }

        return changed;
    }*/
    public static bool NakedSingle(Board? board, Cell cell)
    {
        var changed = false;

        foreach (var (row, col) in cell.RowPeers2)
            if (board.board[row, col].PossibleValues.Count == 1)
            {
                board.board[row, col].Value = board.board[row, col].PossibleValues[0];
                board.board[row, col].PossibleValues.Clear();
                RemoveValueOfCellFromPeers(board.board[row, col], board);
                changed = true;
            }


        return changed;
    }

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

    public static void RemoveValueOfCellFromPeers(Cell cell, Board board)
    {
        foreach (var (row, col) in cell.Peers) board.board[row, col].PossibleValues.Remove(cell.Value);
    }

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


    /*public static bool HiddenSingle(AntBoard? board)
    {
        bool changed = false;
        for (int row = 0; row < board.boardLen; row++)
        {
            for (int col = 0; col < board.boardLen; col++)
            {
                if (board.board[row, col].Value == '0')
                {
                    for (char i = '1'; i <= board.boardLen + '0'; i++)
                    {
                        if (board.board[row, col].PossibleValues.Contains(i))
                        {
                            bool found = false;
                            foreach (var (row2, col2) in board.board[row, col].RowPeers)
                            {
                                if (board.board[row2, col2].PossibleValues.Contains(i))
                                {
                                    found = true;
                                    break;
                                }
                            }

                            if (!found)
                            {
                                board.board[row, col].Value = i;
                                board.board[row, col].PossibleValues.Clear();
                                changed = true;
                            }

                            found = false;
                            foreach (var (row2, col2) in board.board[row, col].ColPeers)
                            {
                                if (board.board[row2, col2].PossibleValues.Contains(i))
                                {
                                    found = true;
                                    break;
                                }
                            }

                            if (!found)
                            {
                                board.board[row, col].Value = i;
                                board.board[row, col].PossibleValues.Clear();
                                changed = true;
                            }

                            found = false;
                            foreach (var (row2, col2) in board.board[row, col].BoxPeers)
                            {
                                if (board.board[row2, col2].PossibleValues.Contains(i))
                                {
                                    found = true;
                                    break;
                                }
                            }

                            if (!found)
                            {
                                board.board[row, col].Value = i;
                                board.board[row, col].PossibleValues.Clear();
                                changed = true;
                            }
                        }
                    }
                }
            }
        }

        return changed;
    }*/
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
    /*
    public static bool HiddenSingleForUnit(Cell[] cellArray)
    {
        List<char>[] lists = new List<char>[cellArray.Length];
        for (int i = 0; i < cellArray.Length; i++)
        {
            lists[i] = cellArray[i].PossibleValues;
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
            cellArray[index].Value = c;
            cellArray[index].PossibleValues.Clear();
            for (int i = 0; i < cellArray.Length; i++)
            {
                if (i != index)
                {
                    cellArray[i].PossibleValues.Remove(c);
                }
            }
        }

        return true;
    }
    */

    public static bool HiddenPairs(Board? board)
    {
        var changed = false;
        var count = 0;
        var TempRow = -1;
        var TempCol = -1;
        for (var row = 0; row < board.boardLen; row++)
        for (var col = 0; col < board.boardLen; col++)
            if (board.board[row, col].Value == '0')
                for (var i = '1'; i <= board.boardLen + '0'; i++)
                    if (board.board[row, col].PossibleValues.Contains(i))
                        for (var j = (char)(i + 1); j <= board.boardLen + '0'; j++)
                            if (board.board[row, col].PossibleValues.Contains(j))
                            {
                                foreach (var (row2, col2) in board.board[row, col].Peers)
                                {
                                    if (board.board[row2, col2].PossibleValues.Contains(i) &&
                                        board.board[row2, col2].PossibleValues.Contains(j))
                                    {
                                        count++;
                                        TempRow = row2;
                                        TempCol = col2;
                                    }

                                    if (count > 1) break;
                                }

                                if (count == 1)
                                {
                                    board.board[TempRow, TempCol].PossibleValues.Clear();
                                    board.board[TempRow, TempCol].PossibleValues.Add(i);
                                    board.board[TempRow, TempCol].PossibleValues.Add(j);
                                    board.board[row, col].PossibleValues.Clear();
                                    board.board[row, col].PossibleValues.Add(i);
                                    board.board[row, row].PossibleValues.Add(j);
                                    changed = true;
                                }

                                count = 0;
                            }

        return changed;
    }

    public static bool HiddenTriples(Board? board)
    {
        var changed = false;
        var count = 0;
        var TempRow = -1;
        var TempCol = -1;
        for (var row = 0; row < board.boardLen; row++)
        for (var col = 0; col < board.boardLen; col++)
            if (board.board[row, col].Value == '0')
                for (var i = '1'; i <= board.boardLen + '0'; i++)
                    if (board.board[row, col].PossibleValues.Contains(i))
                        for (var j = (char)(i + 1); j <= board.boardLen + '0'; j++)
                            if (board.board[row, col].PossibleValues.Contains(j))
                                for (var k = (char)(j + 1); k <= board.boardLen + '0'; k++)
                                    if (board.board[row, col].PossibleValues.Contains(k))
                                    {
                                        foreach (var (row2, col2) in board.board[row, col].Peers)
                                        {
                                            if (board.board[row2, col2].PossibleValues.Contains(i) &&
                                                board.board[row2, col2].PossibleValues.Contains(j) &&
                                                board.board[row2, col2].PossibleValues.Contains(k))
                                            {
                                                count++;
                                                TempRow = row2;
                                                TempCol = col2;
                                            }

                                            if (count > 2) break;
                                        }

                                        if (count == 2)
                                        {
                                            board.board[TempRow, TempCol].PossibleValues.Clear();
                                            board.board[TempRow, TempCol].PossibleValues.Add(i);
                                            board.board[TempRow, TempCol].PossibleValues.Add(j);
                                            board.board[row, col].PossibleValues.Clear();
                                            board.board[row, col].PossibleValues.Add(i);
                                            board.board[row, row].PossibleValues.Add(j);
                                            changed = true;
                                        }

                                        count = 0;
                                    }

        return changed;
    }
}