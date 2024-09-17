// used to define how much time the player has to answer a question.
public enum DifficultyLevel 
{
    Easy = 60,
    Medium = 45,
    Hard = 30,
}

namespace CSharpLearning
{
    public class MathGameLogic
    {
        public void ShowMenu()
        {
            Console.WriteLine("Choose an operation:");
            // couild use a single WriteLine with \n here but I think this is neater.
            Console.WriteLine("1. Addition");
            Console.WriteLine("2. Subtraction");
            Console.WriteLine("3. Multiplication");
            Console.WriteLine("4. Division");
            Console.WriteLine("5. Random Game Mode");
            Console.WriteLine("6. Change Difficulty");
            Console.WriteLine("7. History");
            Console.WriteLine("8. Exit");
        }

        public List<string> GetGameHistory()
        {
            return GameHistory;
        }

        public int MathOperation(int firstNumber, int secondNumber, char operation)
        {
            switch (operation)
            {
                case '+':
                    GameHistory.Add($"{firstNumber} + {secondNumber} = {firstNumber + secondNumber}");
                    return firstNumber + secondNumber;
                case '-':
                    GameHistory.Add($"{firstNumber} - {secondNumber} = {firstNumber - secondNumber}");
                    return firstNumber - secondNumber;
                case '*':
                    GameHistory.Add($"{firstNumber} * {secondNumber} = {firstNumber * secondNumber}");
                    return firstNumber * secondNumber;
                case '/':
                    while (firstNumber < 0 || firstNumber > 100)
                    {
                        try
                        {
                            Console.WriteLine("Please enter a number between 0 and 100");
                            firstNumber = Convert.ToInt32(Console.ReadLine());
                        }
                        catch (System.Exception)
                        {
                            // do nothing
                        }
                    }
                    GameHistory.Add($"{firstNumber} / {secondNumber} = {firstNumber / secondNumber}");
                    return firstNumber / secondNumber;
                default:
                    break;
            }
            return 0;
        }

        public void ChangeDifficulty(DifficultyLevel newDifficulty)
        {
            gameDifficulty = newDifficulty;
            Console.WriteLine($"Setting the difficutly level to: {newDifficulty}");
        }

        public DifficultyLevel Difficulty
        {
            get {return gameDifficulty;}
        }

        // Game is set to easy by default
        private DifficultyLevel gameDifficulty = DifficultyLevel.Easy;
        private List<string> GameHistory {get; set;} = new List<string>();

    }
}