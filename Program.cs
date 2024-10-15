using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        // Read settings from a file
        string[] settings = File.ReadAllLines("..\\game-settings.txt");

        // Parse the board size from the settings file
        int boardSize = int.Parse(settings[0]);
        int[,] board = new int[boardSize, boardSize];

        // Parse starting position and direction
        string[] startInfo = settings[1].Split(',');
        int turtleX = int.Parse(startInfo[0]); // Starting X position
        int turtleY = int.Parse(startInfo[1]); // Starting Y position
        string direction = startInfo[2]; // Initial direction

        // Parse exit position
        string[] exitInfo = settings[2].Split(',');
        int exitX = int.Parse(exitInfo[0]);
        int exitY = int.Parse(exitInfo[1]);

        // Parse mines
        List<(int, int)> mines = new List<(int, int)>();
        for (int i = 3; i < settings.Length; i++)
        {
            string[] mineInfo = settings[i].Split(',');
            mines.Add((int.Parse(mineInfo[0]), int.Parse(mineInfo[1])));
        }

        // Read moves from file
        string[] moves = File.ReadAllLines("..\\moves.txt");

        // Process each sequence of moves
        for (int i = 0; i < moves.Length; i++)
        {
            string result = ProcessSequence(moves[i], ref turtleX, ref turtleY, ref direction, exitX, exitY, mines, boardSize);
            Console.WriteLine($"Sequence {i + 1}: {result}");
        }
    }

    static string ProcessSequence(string sequence, ref int turtleX, ref int turtleY, ref string direction, int exitX, int exitY, List<(int, int)> mines, int boardSize)
    {
        // Split the sequence into actions
        string[] actions = sequence.Split(',');

        foreach (string action in actions)
        {
            if (action == "m")
            {
                Move(ref turtleX, ref turtleY, direction);

                // Check if the turtle hits a mine
                if (IsMine(turtleX, turtleY, mines))
                {
                    return "Mine hit!";
                }

                // Check if the turtle goes outside the grid
                if (!IsInBounds(turtleX, turtleY, boardSize))
                {
                    return "Out of bounds!";
                }

                // Check if the turtle reaches the exit
                if (turtleX == exitX && turtleY == exitY)
                {
                    return "Success!";
                }
            }
            else if (action == "r")
            {
                Rotate(ref direction);
            }
        }

        return "Still in danger!";
    }

    static void Move(ref int turtleX, ref int turtleY, string direction)
    {
        switch (direction)
        {
            case "North":
                turtleX++;
                break;
            case "East":
                turtleY++;
                break;
            case "South":
                turtleX--;
                break;
            case "West":
                turtleY--;
                break;
        }
    }

    static void Rotate(ref string direction)
    {
        switch (direction)
        {
            case "North":
                direction = "South";
                break;
            case "East":
                direction = "E";
                break;
            case "South":
                direction = "West";
                break;
            case "West":
                direction = "North";
                break;
        }
    }

    static bool IsMine(int turtleX, int turtleY, List<(int, int)> mines)
    {
        foreach (var mine in mines)
        {
            if (mine.Item1 == turtleX && mine.Item2 == turtleY)
            {
                return true;    
            }

        }
        return false;
    }

    static bool IsInBounds(int turtleX, int turtleY, int boardSize)
    {
        return turtleX >= 0 && turtleX < boardSize && turtleY >= 0 && turtleY < boardSize;
    }

}