namespace Soduko_Omega
{
    public class ConsoleInputHandler : IInputoutput

    {
        private string? input;

        public ConsoleInputHandler()
        {
            ReadInput();
        }

        public void ReadInput()
        {
            input = Console.ReadLine();
            if (!ValidateInput())
            {
                input = null;
            }

        }

        public bool ValidateInput()
        {
            if (input == null)
            {
                return false;
            }

            if (Math.Sqrt(Math.Sqrt(input.Length))-Math.Floor(Math.Sqrt(Math.Sqrt(input.Length))) != 0)
            {
                return false;
            }
            

            foreach (char c in input)
            {
                if (!(c >= '0' && c <= '0' + Math.Sqrt(input.Length)))
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