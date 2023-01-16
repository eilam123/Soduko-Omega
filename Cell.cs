namespace Soduko_Omega;

public class Cell
{
    public Cell(char value, int row, int column, int boardLen)
    {
        Value = value;
        IsFixed = value != '0';
        Row = row;
        Column = column;
        // Box = (row / 3) * 3 + (column / 3);
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

    public char Value { get; set; }

    public bool IsFixed { get; set; }

    public int Row { get; set; }

    public int Column { get; set; }

    // private int Box { get; set; }
    public List<char> PossibleValues { get; set; }
    public List<Tuple<int, int>> Peers { get; set; }
    public List<Tuple<int, int>> RowPeers { get; set; }
    public List<Tuple<int, int>> ColPeers { get; set; }
    public List<Tuple<int, int>> BoxPeers { get; set; }
    public List<Tuple<int, int>> RowPeers2 { get; set; }
    public List<Tuple<int, int>> ColPeers2 { get; set; }

    public List<Tuple<int, int>> BoxPeers2 { get; set; }
    /*public Object Clone()
    {
        Cell cell = new Cell(Value, Row, Column);
        cell.IsFixed = IsFixed;
        cell.IsSolved = IsSolved;
        cell.PossibleValues = new List<char>(PossibleValues);
        return cell;
    }*/

    public override string ToString()
    {
        // return $"Value: {Value}, IsFixed: {IsFixed}, IsSolved: {IsSolved}, Row: {Row}, Column: {Column}, PossibleValues: {string.Join(", ", PossibleValues)}";
        return $"Value: {Value},  Row: {Row}, Column: {Column}, PossibleValues: {string.Join(", ", PossibleValues)}";
    }

    public bool Failed()
    {
        return PossibleValues.Count == 0;
    }
}