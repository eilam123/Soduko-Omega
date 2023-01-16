namespace Soduko_Omega;

public static class SodukoSolver
{
    public static bool SolveSudoku(Board? board)
    {
        // Loop through each cell of the board
        for (var row = 0; row < board.boardLen; row++)
        for (var col = 0; col < board.boardLen; col++)
            // Check if the cell is empty
            if (board.board[row, col].Value == '0' && board.board[row, col].PossibleValues.Count > 0)
            {
                // Try filling the cell with a number from 1 to 9
                foreach (var num in board.board[row, col].PossibleValues.ToArray())
                    // Check if the number is valid for the cell
                    if (board.IsValid(board.board[row, col], num))
                    {
                        // Fill the cell with the number
                        board.board[row, col].Value = num;
                        // board.board[row, col].PossibleValues.Remove(board.board[row, col].Value);
                        RunConstraintPropagation(board);

                        // Recursively solve the rest of the board
                        if (SolveSudoku(board))
                            // If the board is solved, return true
                            return true;

                        // If the board is not solved, backtrack and try the next number
                        board.board[row, col].Value = '0';
                    }

                // If no number works, return false
                return false;
            }

        // If all cells are filled, the board is solved
        return true;
    }

    public static void RunConstraintPropagation(Board? board)
    {
        var succeedConstraintPropagation = 0;
        do
        {
            succeedConstraintPropagation = 0;
            if (ConstraintPropagation.NakedSingle(board))
                succeedConstraintPropagation++;
            if (ConstraintPropagation.HiddenSingle(board))
                succeedConstraintPropagation++;
            if (ConstraintPropagation.NakedPairs(board))
                succeedConstraintPropagation++;
            // if (ConstraintPropagation.HiddenPairs(board))
            //     succeedConstraintPropagation++;
            // if (ConstraintPropagation.HiddenTriples(board))
            //     succeedConstraintPropagation++;
        } while (succeedConstraintPropagation > 0);
    }
    /*
    public static bool SolveSudoku(AntBoard board)
    {
        /*while (true)
        {
            if (ConstraintPropagation.NakedSingle(board))
                continue;
            if (ConstraintPropagation.HiddenSingle(board))
                continue;
            if (ConstraintPropagation.NakedPairs(board))
                continue;
            if (ConstraintPropagation.HiddenPairs(board))
                continue;
            if(ConstraintPropagation.HiddenTriples(board))
                continue;
            break;
        }#1#
        // Loop through each cell of the board
        for (int row = 0; row < board.boardLen; row++)
        {
            for (int col = 0; col < board.boardLen; col++)
            {
                // Check if the cell is empty
                if (board.board[row, col].Value == '0')
                {
                    // Try filling the cell with a number 
                    foreach(char num in board.board[row, col].PossibleValues.ToArray())
                    {
                        // Fill the cell with the number
                        board.board[row, col].Value = num;
                        board.board[row, col].PossibleValues.Clear();
                        board.RemovePossibleValueFromPeer(board.board[row, col]);
                        // Recursively solve the rest of the board
                        if (SolveSudoku(board))
                        {
                            // If the board is solved, return true
                            return true;
                        }
                        // If the board is not solved, backtrack and try the next number
                        board.board[row, col].Value = '0';
                        // board.BuildPossibilities();
                        
                    }

                    // If no number works, return false
                    return false;
                }
            }
        }

        // If all cells are filled, the board is solved
        return true;
    }
    */
}