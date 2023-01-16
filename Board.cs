using Soduko_Omega.Exceptions;

namespace Soduko_Omega;

public class Board : ICloneable
{
    public Cell[,] board;
    /// <summary>
    /// this builds the board from the input string, the board is an array of cells
    /// </summary>
    /// <param name="io"></param>
    /// <exception cref="InvalidBoardException"></exception>
    public Board(IInputoutput io)
    {
        var input = io.ToString();

        boardLen = (int)Math.Sqrt(input.Length);
        board = new Cell[boardLen, boardLen];

        for (var i = 0; i < boardLen; i++)
        for (var j = 0; j < boardLen; j++)
            board[i, j] = new Cell(input[i * boardLen + j], i, j, boardLen);

        if (!IsValid()) throw new InvalidBoardException();

        BuildPossibilities();
    }

    private Board(int boardLen)
    {
        this.boardLen = boardLen;
    }

    public int boardLen { get; set; }
    public int AmountOfFilledCells { get; set; }
    /// <summary>
    /// implentation of ICloneable interface for deep copy
    /// </summary>
    /// <returns>new object type object</returns>
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
    /// <summary>
    /// get the cell at the given coordinates
    /// </summary>
    /// <param name="pos"></param>
    /// <returns>Cell object</returns>
    public Cell GetCell(Tuple<int, int> pos)
    {
        return board[pos.Item1, pos.Item2];
    }

    /// <summary>
    /// set the cell at the given coordinates
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="value"></param>
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
    /// <summary>
    /// this method returns list of the possitions of the empty cells in the board
    /// </summary>
    /// <returns>list of tuples</returns>
    public List<Tuple<int, int>> GetEmptyCells()
    {
        var list = new List<Tuple<int, int>>();
        for (var i = 0; i < boardLen; i++)
        for (var j = 0; j < boardLen; j++)
            if (board[i, j].Value == '0')
                list.Add(new Tuple<int, int>(i, j));

        return list;
    }
    /// <summary>
    /// this method returns the num of filled cells in the board
    /// </summary>
    /// <returns>count</returns>
    public int NumOfFixedCells()
    {
        var count = 0;
        foreach (var cell in board)
            if (cell.Value != '0')
                count++;

        return count;
    }
    /// <summary>
    ///function that builds for each cell a list of possible values
    /// </summary>
    public void BuildPossibilities()
    {
        for (var i = 0; i < boardLen; i++)
        for (var j = 0; j < boardLen; j++)
            if (board[i, j].Value == '0')
                for (var k = '1'; k <= boardLen + '0'; k++)
                    if (CheckRow(i, k) == 0 && CheckColumn(j, k) == 0 && CheckSquare(i, j, k) == 0)
                        board[i, j].PossibleValues.Add(k);
    }
    /// <summary>
    /// this function clears all the possible values for each cell
    /// </summary>
    public void ClearPossibleValues()
    {
        for (var i = 0; i < boardLen; i++)
        for (var j = 0; j < boardLen; j++)
            board[i, j].PossibleValues.Clear();
    }
    /// <summary>
    /// this function checks the amount of times a value appears in a row
    /// </summary>
    /// <param name="row"></param>
    /// <param name="value"></param>
    /// <returns>count</returns>
    private int CheckRow(int row, char value)
    {
        var count = 0;
        for (var i = 0; i < boardLen; i++)
            if (board[row, i].Value == value)
                count++;

        return count;
    }
    /// <summary>
    /// this function checks the amount of times a value appears in a column
    /// </summary>
    /// <param name="column"></param>
    /// <param name="value"></param>
    /// <returns>count</returns>
    private int CheckColumn(int column, char value)
    {
        var count = 0;
        for (var i = 0; i < boardLen; i++)
            if (board[i, column].Value == value)
                count++;

        return count;
    }
    /// <summary>
    /// this function checks the amount of times a value appears in a square
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <param name="value"></param>
    /// <returns>count</returns>
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

    /// <summary>
    /// this function prints the board
    /// </summary>
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
    /// <summary>
    /// this function checks if the board is valid
    /// </summary>
    /// <returns>bool</returns>
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
    /// <summary>
    /// convert board to string
    /// </summary>
    /// <returns>return string of all chars in the board</returns>
    public string BoardToString()
    {
        var final = "";
        for (var i = 0; i < boardLen; i++)
        for (var j = 0; j < boardLen; j++)
            final += board[i, j].Value;

        return final;
    }
    /// <summary>
    /// check if the given char is valid in his rows&&cols&&boxs
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="num"></param>
    /// <returns>true if can fit false if cant</returns>
    public bool IsValid(Cell cell, char num)
    {
        foreach (var (row, col) in cell.Peers)
            if (board[row, col].Value == num)
                return false;

        return true;
    }

}