namespace Soduko_Omega.Ants;

public class AntSolver
{
    private readonly Ant[]? _ants;
    private readonly double _bestPherEvap;
    private readonly Board? _board;
    private readonly double _globalPherUpdate;
    private readonly double _initialPhervalue;
    private readonly int _numAnts;
    private readonly double[,,]? _pherMatrix;
    private double _bestPherToAdd;
    private Board? _solution;

    public AntSolver(Board? board, double globalPherUpdate, double bestPherEvap, int numAnts)
    {
        _board = board;
        _solution = _board;
        _globalPherUpdate = globalPherUpdate;
        _bestPherEvap = bestPherEvap;
        _numAnts = numAnts;
        if (board != null)
        {
            _initialPhervalue = 1 / Math.Pow(board.boardLen, 2);
            _ants = new Ant[_numAnts];
            _pherMatrix = new double[board.boardLen, board.boardLen, board.boardLen];
            for (var i = 0; i < board.boardLen; i++)
            for (var j = 0; j < board.boardLen; j++)
            for (var k = 0; k < board.boardLen; k++)
                _pherMatrix[i, j, k] = _initialPhervalue;
        }

        _bestPherToAdd = 0;
    }

    private void GlobalPheromoneMatrixUpdate()
    {
        if (_board != null)
            for (var i = 0; i < _board.boardLen; i++)
            for (var j = 0; j < _board.boardLen; j++)
                if (_solution != null)
                {
                    var solutionCell = _solution.board[i, j];
                    if (!solutionCell.Failed())
                        if (_pherMatrix != null)
                            _pherMatrix[i, j, solutionCell.Value - '1'] =
                                _pherMatrix[i, j, solutionCell.Value - '1'] * (1 - _globalPherUpdate) +
                                _globalPherUpdate * _bestPherToAdd;
                }
    }

    public Board? Solve(double localPherUpdate, double greediness)
    {
        var solved = false;
        var cycle = 1;

        while (!solved)
        {
            var random = new Random();
            // ConstraintPropagation.NakedSingle(board);
            // ConstraintPropagation.HiddenSingle(board);
            var emptyCells = _board?.GetEmptyCells() ?? new List<Tuple<int, int>>();
            for (var i = 0; i < _numAnts; i++)
                // do not use random take the next empty cell
                // var cell = _board?.GetCellWithFewestPossibilities();
                if (emptyCells.Count > _numAnts)
                {
                    var randomNumber1 = random.Next(0, emptyCells.Count - 1);
                    var startPos =
                        new Tuple<int, int>(emptyCells[randomNumber1].Item1, emptyCells[randomNumber1].Item2);
                    emptyCells.RemoveAt(randomNumber1);
                    var boardCopy = (Board)_board.Clone();
                    if (_ants != null)
                        if (_pherMatrix != null)
                            _ants[i] = new Ant(_pherMatrix, _initialPhervalue, localPherUpdate, greediness, boardCopy,
                                startPos);
                }
                else
                {
                    var randomNumber2 = random.Next(0, emptyCells.Count - 1);
                    var randomNumber1 = random.Next(0, emptyCells.Count - 1);
                    var startPos = new Tuple<int, int>(randomNumber1, randomNumber2);
                    var boardCopy = (Board)_board.Clone();
                    if (_ants != null)
                        if (_pherMatrix != null)
                            _ants[i] = new Ant(_pherMatrix, _initialPhervalue, localPherUpdate, greediness, boardCopy,
                                startPos);
                }

            if (_board != null)
                for (var step = 0; step < (int)Math.Pow(_board.boardLen, 2); step++)
                for (var i = 0; i < _numAnts; i++)
                    _ants?[i].Step();

            var bestAntFixedCount = 0;
            Ant bestAnt = null;

            foreach (var ant in _ants)
            {
                var NumFixed = ant.GetNumFixedCells();

                if (NumFixed == (int)Math.Pow(ant.AntBoard.boardLen, 2))
                {
                    _solution = ant.AntBoard;
                    return _solution;
                }

                if (NumFixed > bestAntFixedCount)
                {
                    bestAnt = ant;
                    bestAntFixedCount = (int)NumFixed;
                }
            }

            double pherToAdd = (int)Math.Pow(_board.boardLen, 2) /
                               ((int)Math.Pow(_board.boardLen, 2) - bestAntFixedCount);
            if (pherToAdd > _bestPherToAdd)
            {
                if (bestAnt != null) _solution = bestAnt.AntBoard;
                _bestPherToAdd = pherToAdd;
            }

            GlobalPheromoneMatrixUpdate();
            _bestPherToAdd *= 1 - _bestPherEvap;
            Console.WriteLine("cycle: " + cycle);
            _solution?.PrintBoard();
            Console.WriteLine("Num Fixed: " + bestAntFixedCount);

            cycle++;
        }

        return _solution;
    }
}