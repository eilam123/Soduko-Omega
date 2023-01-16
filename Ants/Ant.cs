namespace Soduko_Omega.Ants;

public class Ant
{
    // The greediness factor of the ant
    private readonly double _greediness;
    // The initial pheromone value for the pheromone matrix
    private readonly double _initialPherValue;
    // The update rate for the local pheromone
    private readonly double _localPherUpdate;
    // The pheromone matrix for the ant
    private readonly double[,,] _pherMatrix;
    // The current position of the ant
    private Tuple<int, int> _pos;
    // The board that the ant is working on
    public Board? AntBoard;

    /// <summary>
    /// Constructor for the ant class
    /// </summary>
    /// <param name="pherMatrix">The pheromone matrix for the ant</param>
    /// <param name="initialPherValue">The initial pheromone value for the pheromone matrix</param>
    /// <param name="localPherUpdate">The update rate for the local pheromone</param>
    /// <param name="greediness">The greediness factor of the ant</param>
    /// <param name="antBoard">The board that the ant is working on</param>
    /// <param name="pos">The current position of the ant</param>
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

    /// <summary>
    /// Retrieves the number of fixed cells in the board that the ant is working on
    /// </summary>
    /// <returns>The number of fixed cells in the board</returns>
    public int? GetNumFixedCells()
    {
        if (AntBoard != null) return AntBoard.NumOfFixedCells();
        return null;
    }

    /// <summary>
    /// Method for the ant's decision making and movement
    /// </summary>
    public void Step()
    {
        var cell = AntBoard.board[_pos.Item1, _pos.Item2];
        if (AntBoard.board[_pos.Item1, _pos.Item2].Value == '0' && cell.PossibleValues.Count > 0)
        {
            // The best value that the ant chooses
            var bestVal = '0';
            // The best pheromone value that the ant chooses
            double bestPher = 0;
            // The list of possible values for the current cell
            var possibleValues = AntBoard.board[_pos.Item1, _pos.Item2].PossibleValues;
            var random = new Random();
            var randomNumber = random.NextDouble();
            if (randomNumber > _greediness)
            {
                // Greedy selection
                foreach (var val in possibleValues)
                    if (_pherMatrix[_pos.Item1, _pos.Item2, val - '1'] > bestPher)
                    {
                        bestPher = _pherMatrix[_pos.Item1, _pos.Item2, val - '1'];
                        bestVal = val;
                    }
            }
            else
            {
                // Roulette wheel selection
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
            ConstraintPropagation.RunConstraintPropagation(AntBoard);
            // Update the pheromone matrix
            var pherValueToUpdate = _pherMatrix[_pos.Item1, _pos.Item2, bestVal - '1'];
            _pherMatrix[_pos.Item1, _pos.Item2, bestVal - '1'] =
                (1 - _localPherUpdate) * pherValueToUpdate + _localPherUpdate * _initialPherValue;
        }
        // Move the ant
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