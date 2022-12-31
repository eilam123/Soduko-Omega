namespace Soduko_Omega
{
    public class ConsoleInputHandler : IInputoutput

    {
        private string? input;

        public ConsoleInputHandler(int boardSize)
        {
            ReadInput(boardSize);
        }

        public void ReadInput(int boardSize)
        {
            input = Console.ReadLine();
            if (!ValidateInput(boardSize))
            {
                input = null;
            }

        }

        public bool ValidateInput(int boardSize)
        {
            if (input == null)
            {
                return false;
            }

            if (input.Length != Math.Pow(boardSize, 2))
            {
                return false;
            }


            foreach (char c in input)
            {
                if (!(c >= '0' && c <= '0' + boardSize))
                {
                    return false;
                }

            }

            return true;
        }

        public override string? ToString()
        {
            return input;
        }
    }
}