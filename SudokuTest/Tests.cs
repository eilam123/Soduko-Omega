using System;
using Soduko_Omega;
using Soduko_Omega.Ants;
using Soduko_Omega.IO;
using Xunit;
using SudokuTest;

namespace SudokuTest
{
    public class Tests
    {
        [Fact]
        public void Test1()
        {
            string input = "800000070006010053040600000000080400003000700020005038000000800004050061900002000";
            string expected = "";
            
            Board board = new Board(input);
            AntSolver solver = new AntSolver(board, Constants.GLOBAL_PHER_UPDATE, Constants.BEST_PHER_EVAPORATION, Constants.NUM_OF_ANTS);
            Board solved = solver.Solve(Constants.LOCAL_PHER_UPDATE, Constants.GREEDINESS);
            string actual = solved.BoardToString();
            
            Assert.Equal(expected, actual);


        }
    }
}