namespace Soduko_Omega.Ants;

public class Ant
{
    private readonly double _greediness;
    private readonly double _initialPherValue;
    private readonly double _localPherUpdate;
    private readonly double[,,] _pherMatrix;
    private Tuple<int, int> _pos;
    public Board? AntBoard;

    public Ant(double[,,] pherMatrix, double initialPherValue, double localPherUpdate, double greediness,
        Board? antBoard,
        Tuple<int, int> pos)
    {
        _pherMatrix = pherMatrix;
        _initialPherValue = initialPherValue;
        _localPherUpdate = localPherUpdate;
        _greediness = greediness;
        AntBoard = antBoard;
        _pos = pos;
    }

    public int? GetNumFixedCells()
    {
        if (AntBoard != null) return AntBoard.NumOfFixedCells();
        return null;
    }

    public void Step()
    {
        var cell = AntBoard.board[_pos.Item1, _pos.Item2];
        if (AntBoard.board[_pos.Item1, _pos.Item2].Value == '0' && cell.PossibleValues.Count > 0)
        {
            var bestVal = '0';
            double bestPher = 0;
            var possibleValues = AntBoard.board[_pos.Item1, _pos.Item2].PossibleValues;
            var random = new Random();
            var randomNumber = random.NextDouble();
            if (randomNumber > _greediness)
            {
                foreach (var val in possibleValues)
                    if (_pherMatrix[_pos.Item1, _pos.Item2, val - '1'] > bestPher)
                    {
                        bestPher = _pherMatrix[_pos.Item1, _pos.Item2, val - '1'];
                        bestVal = val;
                    }
            }
            else
            {
                double totalPher = 0;
                var wheel = new List<double>();
                for (var i = 0; i < possibleValues.Count; i++)
                {
                    wheel.Add(totalPher + _pherMatrix[_pos.Item1, _pos.Item2, possibleValues[i] - '1']);
                    totalPher = wheel[i];
                }

                var randomNumber2 = random.NextDouble();
                var spinValue = randomNumber2 * totalPher;
                for (var i = 0; i < wheel.Count; i++)
                    if (wheel[i] > spinValue)
                    {
                        bestVal = possibleValues[i];
                        break;
                    }
            }

            AntBoard.SetCell(_pos, bestVal);
            ConstraintPropagation.RemoveValueOfCellFromPeers(AntBoard.board[_pos.Item1, _pos.Item2], AntBoard);
            ConstraintPropagation.RemoveValueOfCellFromPeers(cell, AntBoard);
            SodukoSolver.RunConstraintPropagation(AntBoard);
            ;
            var pherValueToUpdate = _pherMatrix[_pos.Item1, _pos.Item2, bestVal - '1'];
            _pherMatrix[_pos.Item1, _pos.Item2, bestVal - '1'] =
                (1 - _localPherUpdate) * pherValueToUpdate + _localPherUpdate * _initialPherValue;
        }

        var newRowIndex = _pos.Item1;
        var newColIndex = _pos.Item2 + 1;
        if (newColIndex == AntBoard.boardLen)
        {
            newColIndex = 0;
            newRowIndex++;
        }

        if (newRowIndex == AntBoard.boardLen) newRowIndex = 0;

        _pos = new Tuple<int, int>(newRowIndex, newColIndex);
    }
}