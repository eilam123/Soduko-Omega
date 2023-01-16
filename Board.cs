using Soduko_Omega.Exceptions;

namespace Soduko_Omega;

public class Board : ICloneable
{
    public Cell[,] board;

    public Board(IInputoutput io)
    {
        var input = io.ToString();

        boardLen = (int)Math.Sqrt(input.Length);
        board = new Cell[boardLen, boardLen];

        for (var i = 0; i < boardLen; i++)
        for (var j = 0; j < boardLen; j++)
            //inout[i* bordLen /+j] is an unclear statement
            board[i, j] = new Cell(input[i * boardLen + j], i, j, boardLen);

        if (!IsValid()) throw new InvalidBoardException();

        BuildPossibilities();
    }

    private Board(int boardLen)
    {
        this.boardLen = boardLen;
    }

    public int boardLen { get; set; }
    public int AmountOfFilledCells { get; set; } // by ants

    public object Clone()
    {
        var newBoard = new Board(boardLen);
        newBoard.board = new Cell[boardLen, boardLen];
        for (var i = 0; i < boardLen; i++)
        for (var j = 0; j < boardLen; j++)
        {
            newBoard.board[i, j] = new Cell(board[i, j].Value, i, j, boardLen);
            newBoard.board[i, j].PossibleValues = new List<char>(board[i, j].PossibleValues);
        }

        return newBoard;
    }

    public Cell GetCell(Tuple<int, int> pos)
    {
        return board[pos.Item1, pos.Item2];
    }


    public void SetCell(Tuple<int, int> pos, char value)
    {
        var cell = GetCell(pos);
        if (!cell.Failed())
            if (cell.Value == '0')
                AmountOfFilledCells++;
        cell.Value = value;
        board[pos.Item1, pos.Item2] = cell;
        board[pos.Item1, pos.Item2].PossibleValues.Clear();
    }

    public List<Tuple<int, int>> GetEmptyCells()
    {
        var list = new List<Tuple<int, int>>();
        for (var i = 0; i < boardLen; i++)
        for (var j = 0; j < boardLen; j++)
            if (board[i, j].Value == '0')
                list.Add(new Tuple<int, int>(i, j));

        return list;
    }

    public int NumOfFixedCells()
    {
        var count = 0;
        foreach (var cell in board)
            if (cell.Value != '0')
                count++;

        return count;
    }

    //function that builds for each cell a list of possible values
    public void BuildPossibilities()
    {
        for (var i = 0; i < boardLen; i++)
        for (var j = 0; j < boardLen; j++)
            if (board[i, j].Value == '0')
                for (var k = '1'; k <= boardLen + '0'; k++)
                    //expain checkrow/chekcol...
                    if (CheckRow(i, k) == 0 && CheckColumn(j, k) == 0 && CheckSquare(i, j, k) == 0)
                        board[i, j].PossibleValues.Add(k);
    }

    public void ClearPossibleValues()
    {
        for (var i = 0; i < boardLen; i++)
        for (var j = 0; j < boardLen; j++)
            board[i, j].PossibleValues.Clear();
    }

    private int CheckRow(int row, char value)
    {
        var count = 0;
        for (var i = 0; i < boardLen; i++)
            if (board[row, i].Value == value)
                count++;

        return count;
    }

    private int CheckColumn(int column, char value)
    {
        var count = 0;
        for (var i = 0; i < boardLen; i++)
            if (board[i, column].Value == value)
                count++;

        return count;
    }

    private int CheckSquare(int row, int column, char value)
    {
        var squareSize = (int)Math.Sqrt(boardLen);
        var squareRow = row / squareSize;
        var squareColumn = column / squareSize;
        var count = 0;
        for (var i = squareRow * squareSize; i < squareRow * squareSize + squareSize; i++)
        for (var j = squareColumn * squareSize; j < squareColumn * squareSize + squareSize; j++)
            if (board[i, j].Value == value)
                count++;

        return count;
    }


    // print the board like a grid that each value is in a cell
    public void PrintBoard()
    {
        Console.WriteLine();
        for (var row = 0; row < boardLen; row++)
        {
            for (var col = 0; col < boardLen; col++)
                // int value = board[row, col].Value - '0';
                // if (value < 10)
                Console.Write("+---");
            // else
            //     Console.Write("+----");
            Console.WriteLine("+");

            for (var col = 0; col < boardLen; col++)
                // int value = board[row, col].Value - '0';
                Console.Write("| " + board[row, col].Value + " ");

            Console.WriteLine("|");
        }

        for (var col = 0; col < boardLen; col++) Console.Write("+---");

        Console.WriteLine("+");
    }

    //check if the board is valid
    public bool IsValid()
    {
        for (var i = 0; i < boardLen; i++)
        for (var j = 0; j < boardLen; j++)
            if (board[i, j].Value != '0')
                if (CheckRow(i, board[i, j].Value) > 1 || CheckColumn(j, board[i, j].Value) > 1 ||
                    CheckSquare(i, j, board[i, j].Value) > 1)
                    return false;

        return true;
    }

    public string BoardToString()
    {
        var final = "";
        for (var i = 0; i < boardLen; i++)
        for (var j = 0; j < boardLen; j++)
            final += board[i, j].Value;

        return final;
    }


    /*
    public bool Solve()
    {
        if (!IsValid())
        {
            return false;
        }

        BuildPossibilities();
        if (IsSolved())
        {
            return true;
        }

        Cell cell = GetCellWithFewestPossibilities();
        if (cell.PossibleValues.Count == 0)
        {
            return false;
        }

        foreach (char value in cell.PossibleValues.ToArray())
        {
            cell.Value = value;
            if (Solve())
            {
                return true;
            }

            cell.Value = '0';
            BuildPossibilities();
            
        }

        return false;
    }

    bool IsSolved()
    {
        for (int i = 0; i < boardLen; i++)
        {
            for (int j = 0; j < boardLen; j++)
            {
                if (board[i, j].Value == '0')
                {
                    return false;
                }
            }
        }

        return true;
    }

    Cell GetCellWithFewestPossibilities()
    {
        Cell cell = new Cell('0', 0, 0);
        int min = boardLen + 1;
        for (int i = 0; i < boardLen; i++)
        {
            for (int j = 0; j < boardLen; j++)
            {
                if (board[i, j].Value == '0' && board[i, j].PossibleValues.Count < min)
                {
                    min = board[i, j].PossibleValues.Count;
                    cell = board[i, j];
                }
            }
        }

        return cell;
    }
    */
    public bool IsValid(char num, int row, int col)
    {
        // Check if the number is present in the same row
        for (var i = 0; i < boardLen; i++)
            if (board[row, i].Value == num)
                return false;

        // Check if the number is present in the same column
        for (var i = 0; i < boardLen; i++)
            if (board[i, col].Value == num)
                return false;

        // Check if the number is present in the same subgrid
        var startRow = row - row % (int)Math.Sqrt(boardLen);
        var startCol = col - col % (int)Math.Sqrt(boardLen);
        for (var i = 0; i < (int)Math.Sqrt(boardLen); i++)
        for (var j = 0; j < (int)Math.Sqrt(boardLen); j++)
            if (board[startRow + i, startCol + j].Value == num)
                return false;

        // If the number is not present in the same row, column, or subgrid, it is valid
        return true;
    }

    public void RemovePossibleValueFromPeer(Cell cell)
    {
        foreach (var (row, col) in cell.Peers) board[row, col].PossibleValues.Remove(cell.Value);
    }

    public bool IsValid(Cell cell, char num)
    {
        foreach (var (row, col) in cell.Peers)
            if (board[row, col].Value == num)
                return false;

        return true;
    }

    public void RestorePossibleValueFromPeer(Cell cell)
    {
        foreach (var (row, col) in cell.Peers) board[row, col].PossibleValues.Add(cell.Value);
    }

    // find the cell with the fewest possibilities
    public Cell GetCellWithFewestPossibilities()
    {
        var cell = new Cell('0', 0, 0, boardLen);
        var min = boardLen + 1;
        for (var i = 0; i < boardLen; i++)
        for (var j = 0; j < boardLen; j++)
            if (board[i, j].Value == '0' && board[i, j].PossibleValues.Count < min)
            {
                min = board[i, j].PossibleValues.Count;
                cell = board[i, j];
            }

        return cell;
    }

    public int CountAmountOfSolvedCells()
    {
        var count = 0;
        for (var i = 0; i < boardLen; i++)
        for (var j = 0; j < boardLen; j++)
            if (board[i, j].Value != '0')
                count++;

        return count;
    }
}