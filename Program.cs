
using System;
using System.IO;

public class Player
{
    public string Name;
    public char Symbol;

    private int[] positions = new int[3];
    private int markCount = 0;

    public Player(string name, char symbol)
    {
        Name = name;
        Symbol = symbol;
    }

    public void PlaceMark(char[] board, int position)
    {
        if (markCount == 3)
        {
            board[positions[0]] = ' ';

            positions[0] = positions[1];
            positions[1] = positions[2];
            positions[2] = position;
        }
        else
        {
            positions[markCount] = position;
            markCount++;
        }

        board[position] = Symbol;
    }
}

public class TicTacToe
{
    private char[] board = new char[9];
    private Player player1;
    private Player player2;

    public TicTacToe(string name1, string name2)
    {
        player1 = new Player(name1, 'X');
        player2 = new Player(name2, 'O');

        for (int i = 0; i < 9; i++)
        {
            board[i] = ' ';
        }
    }

    public void DisplayBoard()
    {
        Console.WriteLine();
        Console.WriteLine($" {ShowCell(0)} | {ShowCell(1)} | {ShowCell(2)}");
        Console.WriteLine("---+---+---");
        Console.WriteLine($" {ShowCell(3)} | {ShowCell(4)} | {ShowCell(5)}");
        Console.WriteLine("---+---+---");
        Console.WriteLine($" {ShowCell(6)} | {ShowCell(7)} | {ShowCell(8)}");
        Console.WriteLine();
    }

    private string ShowCell(int position)
    {
        return board[position] == ' '
            ? (position + 1).ToString()
            : board[position].ToString();
    }

    private int GetMove(Player player)
    {
        while (true)
        {
            Console.Write($"{player.Name} ({player.Symbol}), choose a square (1-9): ");
            string input = Console.ReadLine() ?? "";

            if (int.TryParse(input, out int choice) &&
                choice >= 1 && choice <= 9 &&
                board[choice - 1] == ' ')
            {
                return choice - 1;
            }

            Console.WriteLine("Invalid move! Choose an empty square from 1 to 9.");
        }
    }

    private bool CheckWin(char symbol)
    {
        int[,] lines =
        {
            { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 },
            { 0, 3, 6 }, { 1, 4, 7 }, { 2, 5, 8 },
            { 0, 4, 8 }, { 2, 4, 6 }
        };

        for (int i = 0; i < 8; i++)
        {
            if (board[lines[i, 0]] == symbol &&
                board[lines[i, 1]] == symbol &&
                board[lines[i, 2]] == symbol)
            {
                return true;
            }
        }

        return false;
    }

    private void SaveResult(string winner)
    {
        string result = DateTime.Now + " - " + winner + " won XO Arena.";
        File.AppendAllText("GameResults.txt", result + Environment.NewLine);
    }

    public void PlayGame()
    {
        Player currentPlayer = player1;

        Console.WriteLine("\n=== XO ARENA ===");
        Console.WriteLine("Each player can have a maximum of 3 marks.");
        Console.WriteLine("On your 4th move, your oldest mark disappears.");
        Console.WriteLine("Make 3 marks in a row to win!");

        while (true)
        {
            DisplayBoard();
            int position = GetMove(currentPlayer);
            currentPlayer.PlaceMark(board, position);

            Console.Clear();
            Console.WriteLine("=== XO ARENA ===");
            DisplayBoard();

            if (CheckWin(currentPlayer.Symbol))
            {
                Console.WriteLine($"{currentPlayer.Name} wins!");
                SaveResult(currentPlayer.Name);
                break;
            }

            currentPlayer = currentPlayer == player1 ? player2 : player1;
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("================================");
        Console.WriteLine("       WELCOME TO XO ARENA");
        Console.WriteLine("================================");

        Console.Write("Enter Player 1 name: ");
        string name1 = Console.ReadLine() ?? "Player 1";

        Console.Write("Enter Player 2 name: ");
        string name2 = Console.ReadLine() ?? "Player 2";

        string playAgain;

        do
        {
            Console.Clear();

            TicTacToe game = new TicTacToe(name1, name2);
            game.PlayGame();

            Console.Write("\nPlay again? (yes/no): ");
            playAgain = (Console.ReadLine() ?? "").Trim();

        } while (playAgain.Equals("yes", StringComparison.OrdinalIgnoreCase));

        Console.WriteLine("\nThanks for playing XO Arena!");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
