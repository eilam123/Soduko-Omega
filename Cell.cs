namespace Soduko_Omega;
/// <summary>
/// Class representing a Cell in the Sudoku board
/// </summary>
public class Cell
{
    /// <summary>
    /// Constructor for a Cell
    /// </summary>
    /// <param name="value">Value of the cell</param>
    /// <param name="row">Row of the cell</param>
    /// <param name="column">Column of the cell</param>
    /// <param name="boardLen">Size of the board</param>
    public Cell(char value, int row, int column, int boardLen)
    {
        Value = value;
        IsFixed = value != '0';
        Row = row;
        Column = column;
        PossibleValues = new List<char>();
        Peers = new List<Tuple<int, int>>();
        RowPeers = new List<Tuple<int, int>>();
        ColPeers = new List<Tuple<int, int>>();
        BoxPeers = new List<Tuple<int, int>>();

        RowPeers2 = new List<Tuple<int, int>>();
        ColPeers2 = new List<Tuple<int, int>>();
        BoxPeers2 = new List<Tuple<int, int>>();

        for (var c = 0; c < boardLen; c++)
        {
            RowPeers2.Add(new Tuple<int, int>(row, c));
            if (c != column)
            {
                Peers.Add(new Tuple<int, int>(Row, c));
                RowPeers.Add(new Tuple<int, int>(Row, c));
            }
        }

        for (var r = 0; r < boardLen; r++)
        {
            ColPeers2.Add(new Tuple<int, int>(r, column));
            if (r != Row)
            {
                Peers.Add(new Tuple<int, int>(r, Column));
                ColPeers.Add(new Tuple<int, int>(r, Column));
            }
        }

        var boxRow = Row / (int)Math.Sqrt(boardLen) * (int)Math.Sqrt(boardLen);
        var boxCol = Column / (int)Math.Sqrt(boardLen) * (int)Math.Sqrt(boardLen);
        for (var r = boxRow; r < boxRow + (int)Math.Sqrt(boardLen); r++)
        for (var c = boxCol; c < boxCol + (int)Math.Sqrt(boardLen); c++)
        {
            BoxPeers2.Add(new Tuple<int, int>(r, c));
            if (r != Row && c != Column) Peers.Add(new Tuple<int, int>(r, c));

            if (r != Row || c != Column) BoxPeers.Add(new Tuple<int, int>(r, c));
        }
    }
    /// <summary>
    /// Value of the cell
    /// </summary>
    public char Value { get; set; }
    /// <summary>
    /// Flag indicating if the value of the cell is fixed
    /// </summary>
    public bool IsFixed { get; set; }
    /// <summary>
    /// Row of the cell
    /// </summary>
    public int Row { get; set; }
    /// <summary>
    /// Column of the cell
    /// </summary>
    public int Column { get; set; }
    /// <summary>
    /// List of possible values for the cell
    /// </summary>
    public List<char> PossibleValues { get; set; }
    /// <summary>
    /// List of cell peers (peers are cells that share a row, column or box)
    /// </summary>
    public List<Tuple<int, int>> Peers { get; set; }
    /// <summary>
    /// List of cell's row peers
    /// </summary>
    public List<Tuple<int, int>> RowPeers { get; set; }
    /// <summary>
    /// List of cell's column peers
    /// </summary>
    public List<Tuple<int, int>> ColPeers { get; set; }
    /// <summary>
    /// List of cell's box peers
    /// </summary>
    public List<Tuple<int, int>> BoxPeers { get; set; }
    /// <summary>
    /// List of cell's row peers
    /// </summary>
    public List<Tuple<int, int>> RowPeers2 { get; set; }
    /// <summary>
    /// List of cell's column peers
    /// </summary>
    public List<Tuple<int, int>> ColPeers2 { get; set; }
    /// <summary>
    /// List of cell's box peers
    /// </summary>
    public List<Tuple<int, int>> BoxPeers2 { get; set; }
    /// <summary>
    /// List of cell's box peers
    /// </summary>
    public bool Failed()
    {
        return PossibleValues.Count == 0;
    }
}