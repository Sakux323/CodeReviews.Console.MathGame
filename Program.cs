/*
You need to create a Math game containing the 4 basic operations
The divisions should result on INTEGERS ONLY and dividends should go from 0 to 100. Example: Your app shouldn't present the division 7/2 to the user, since it doesn't result in an integer.
Users should be presented with a menu to choose an operation
You should record previous games in a List and there should be an option in the menu for the user to visualize a history of previous games.
You don't need to record results on a database. Once the program is closed the results will be deleted.

Challenges:
Try to implement levels of difficulty.
Add a timer to track how long the user takes to finish the game.
Create a 'Random Game' option where the players will be presented with questions from random operations
*/

using System.Diagnostics;
using CSharpLearning;

MathGameLogic mathGame = new MathGameLogic();
Random random = new Random();

int firstNumber;
int secondNumber;
int userMenuChoice;
int score = 0;
bool finished = false;

while (!finished) {
    userMenuChoice = SelectMenuOption(mathGame);

    firstNumber = random.Next(1, 101);
    secondNumber = random.Next(1, 101);

    switch (userMenuChoice)
    {
        case 1:
            score += await PerformOperation(mathGame, firstNumber, secondNumber, '+', score, mathGame.Difficulty);
            break;
        case 2:
            score += await PerformOperation(mathGame, firstNumber, secondNumber, '-', score, mathGame.Difficulty);
            break;
        case 3:
            score += await PerformOperation(mathGame, firstNumber, secondNumber, '*', score, mathGame.Difficulty);
            break;
        case 4:
            while (firstNumber % secondNumber != 0)
            {
                firstNumber = random.Next(1, 101);
                secondNumber = random.Next(1, 101);
            }
            score += await PerformOperation(mathGame, firstNumber, secondNumber, '/', score, mathGame.Difficulty);
            break;
        case 5:
            int numberOfQuestions = 99;
            Console.WriteLine("How many questions would you like to attempt?");
            while (!Int32.TryParse(Console.ReadLine(), out numberOfQuestions))
            {
                Console.WriteLine("How many questions would you like to attempt? please enter an integer");
            }

            while (numberOfQuestions > 0)
            {
                int randomOperation = random.Next(1, 5);
                switch (randomOperation)
                {
                    case 1:
                        firstNumber = random.Next(1, 101);
                        secondNumber = random.Next(1, 101);
                        score += await PerformOperation(mathGame, firstNumber, secondNumber, '+', score, mathGame.Difficulty);
                        break;
                    case 2:
                        firstNumber = random.Next(1, 101);
                        secondNumber = random.Next(1, 101);
                        score += await PerformOperation(mathGame, firstNumber, secondNumber, '-', score, mathGame.Difficulty);
                        break;
                    case 3:
                        firstNumber = random.Next(1, 101);
                        secondNumber = random.Next(1, 101);
                        score += await PerformOperation(mathGame, firstNumber, secondNumber, '*', score, mathGame.Difficulty);
                        break;
                    case 4:
                        firstNumber = random.Next(1, 101);
                        secondNumber = random.Next(1, 101);
                        while (firstNumber % secondNumber != 0)
                        {
                            firstNumber = random.Next(1, 101);
                            secondNumber = random.Next(1, 101);
                        }
                        score += await PerformOperation(mathGame, firstNumber, secondNumber, '/', score, mathGame.Difficulty);
                        break;
                }
                numberOfQuestions--;
            }
            break;
        case 6:
            Console.WriteLine($"Your current difficulty is {mathGame.Difficulty}.");
            Console.WriteLine("Please pick a difficulty level from 1 (Easy) to 3 (Hard)");
            Console.WriteLine("1. Easy");
            Console.WriteLine("2. Medium");
            Console.WriteLine("3. Hard");

            DifficultyLevel newDifficulty = DifficultyLevel.Easy;
            
            int userInput = 0;
            while (!Int32.TryParse(Console.ReadLine(), out userInput) || userInput < 1 || userInput > 3)
            {
                Console.WriteLine("Please pick a difficulty level from 1 (Easy) to 3 (Hard)");
            }

            switch (userInput) 
            {
                case 1:
                    newDifficulty = DifficultyLevel.Easy;
                    break;
                case 2:
                    newDifficulty = DifficultyLevel.Medium;
                    break;
                case 3:
                    newDifficulty = DifficultyLevel.Hard;
                    break;
                default:
                    newDifficulty = DifficultyLevel.Easy;
                    break;
            }

            mathGame.ChangeDifficulty(newDifficulty);
            Console.ReadLine();
            break;
        case 7:
            Console.WriteLine("Game History:\n");
            foreach (var operation in mathGame.GetGameHistory())
            {
                Console.WriteLine(operation);
            }

            Console.WriteLine("Press any key to continue.");
            Console.ReadLine();
            break;
        case 8:
            finished = true;
            Console.WriteLine($"Your final score was: {score}\nExitting, goodbye!");
            break;
    }
}

static void DisplayMathQuestion(int firstNumber, int secondNumber, char operation)
{
    Console.WriteLine($"{firstNumber} {operation} {secondNumber} = ??");
}

static int SelectMenuOption(MathGameLogic mathGame)
{
    int userSelection = -1;
    mathGame.ShowMenu();
    while (userSelection < 1 || userSelection > 8) 
    {
        while(!Int32.TryParse(Console.ReadLine(), out userSelection))
        {
            Console.WriteLine("Please enter a valid option (1-8)");
        }
    }
    
    return userSelection;
}

static async Task<int?> GetUserResponse(DifficultyLevel difficulty)
{
    int response = 0;

    Stopwatch counter = new Stopwatch();
    counter.Start();

    Task<string?> getUserInput = Task.Run(() => Console.ReadLine());

    try
    {
        string? userResponse = await Task.WhenAny(getUserInput, Task.Delay((int)difficulty * 1000)) == getUserInput ? getUserInput.Result : null;
        counter.Stop();

        if (userResponse != null && Int32.TryParse(userResponse, out response))
        {
            Console.WriteLine($"Time taken to answer: {counter.Elapsed.ToString(@"mm\:ss\.fff")}");
            return response;
        }
        else
        {
            throw new OperationCanceledException();
        }
    }
    catch(OperationCanceledException)
    {
        Console.WriteLine("Time is up.");
        return null;
    }
}

static async Task<int> PerformOperation(MathGameLogic mathGame, int firstNumber, int secondNumber, char operation, int score, DifficultyLevel difficulty)
{
    int result;
    int? userInput;

    DisplayMathQuestion(firstNumber, secondNumber, operation);
    result = mathGame.MathOperation(firstNumber, secondNumber, operation);
    userInput = await GetUserResponse(difficulty);
    score += ValidateResult(result, userInput, score);
    return 1;
}

static int ValidateResult(int result, int? userResponse, int score)
{
    if (result == userResponse)
    {
        Console.WriteLine("Congratulations, your answer is correct.");
        score++;
    }
    else
    {
        Console.WriteLine($"Try again!\nThe correct answer was: {result}");
    }
    return score;
}