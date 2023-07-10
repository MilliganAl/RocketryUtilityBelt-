using System;
using System.Collections.Generic;

public class UserInput
{

    public static string CalculationType { get; set; }
    public static double initialRadius { get; set; }
    public static double finalRadius { get; set; }
    public static double initialOrbitAltitude { get; set; }
    public static double finalSurfaceAltitude { get; set; }
    public static double initialSurfaceAltitude { get; set; }
    public static double finalOrbitAltitude { get; set; }
    public static double isp { get; set; }
    public static double fullMass { get; set; }
    public static double emptyMass { get; set; }



}
public class Program
{
    public static void Main()
    {
        Console.WriteLine("Welcome to the Delta-V Calculator");
        Console.WriteLine("Please select an option:");

        string[] options = { "Ascent", "Descent", "Rocket Equation" };
        List<int> selectedIndices = new List<int>();

        DisplayMenu(options, selectedIndices);

        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                UpdateSelectedIndices(selectedIndices, options.Length, -1);
                UpdateMenu(options, selectedIndices);
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                UpdateSelectedIndices(selectedIndices, options.Length, 1);
                UpdateMenu(options, selectedIndices);
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                break;
            }
        }

        foreach (int i in selectedIndices)
        {
            if (options[i] == "Ascent")
            {
                UserInput.initialRadius = GetUserInput("Enter the initial radius: ");
                UserInput.finalRadius = GetUserInput("Enter the final radius: ");
                double deltaV = CalculateAscentDeltaV(UserInput.initialRadius, UserInput.finalRadius);
                Console.WriteLine("Delta-V for Ascent: " + deltaV);
            }
            else if (options[i] == "Descent")
            {
                UserInput.initialRadius = GetUserInput("Enter the initial radius: ");
                UserInput.finalRadius = GetUserInput("Enter the final radius: ");
                double deltaV = CalculateDescentDeltaV(UserInput.initialRadius, UserInput.finalRadius);
                Console.WriteLine("Delta-V for Descent: " + deltaV);
            }
            else if (options[i] == "Rocket Equation")
            {
                UserInput.isp = GetUserInput("Enter the specific impulse: ");
                UserInput.fullMass = GetUserInput("Enter the full mass: ");
                UserInput.emptyMass = GetUserInput("Enter the empty mass: ");
                double deltaV = CalculateRocketDeltaV(UserInput.isp, UserInput.fullMass, UserInput.emptyMass);
                Console.WriteLine("Delta-V using Rocket Equation: " + deltaV);
            }
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    private static void DisplayMenu(string[] options, List<int> selectedIndices)
    {
        for (int i = 0; i < options.Length; i++)
        {
            if (selectedIndices.Contains(i))
            {
                Console.Write("> ");
                Console.ForegroundColor = GetColor(selectedIndices.IndexOf(i));
            }
            else
            {
                Console.Write("  ");
                Console.ResetColor();
            }

            Console.WriteLine(options[i]);
        }
    }

    private static void UpdateMenu(string[] options, List<int> selectedIndices)
    {
        Console.SetCursorPosition(0, Console.CursorTop - options.Length);
        DisplayMenu(options, selectedIndices);
    }

    private static ConsoleColor GetColor(int index)
    {
        ConsoleColor[] colors = { ConsoleColor.Yellow, ConsoleColor.Cyan, ConsoleColor.Magenta };
        return colors[index % colors.Length];
    }

    private static void UpdateSelectedIndices(List<int> selectedIndices, int optionsLength, int direction)
    {
        if (direction == -1)
        {
            if (selectedIndices.Count == 0)
            {
                selectedIndices.Add(optionsLength - 1);
            }
            else
            {
                for (int i = 0; i < selectedIndices.Count; i++)
                {
                    selectedIndices[i] = (selectedIndices[i] - 1 + optionsLength) % optionsLength;
                }
            }
        }
        else if (direction == 1)
        {
            if (selectedIndices.Count == 0)
            {
                selectedIndices.Add(0);
            }
            else
            {
                for (int i = 0; i < selectedIndices.Count; i++)
                {
                    selectedIndices[i] = (selectedIndices[i] + 1) % optionsLength;
                }
            }
        }
    }

    private static double GetUserInput(string prompt)
    {
        Console.Write(prompt);
        string input = Console.ReadLine();
        double value;
        while (!double.TryParse(input, out value))
        {
            Console.WriteLine("Invalid input. Please enter a valid numerical value.");
            Console.Write(prompt);
            input = Console.ReadLine();
        }
        return value;
    }

    private static double CalculateAscentDeltaV(double initialRadius, double finalRadius)
    {
        double deltaV = Math.Sqrt(2 * Constants.GravitationalConstant * Constants.EarthMass / initialRadius)
            - Math.Sqrt(2 * Constants.GravitationalConstant * Constants.EarthMass / finalRadius);
        return deltaV;
    }

    private static double CalculateDescentDeltaV(double initialRadius, double finalRadius)
    {
        double deltaV = Math.Sqrt(2 * Constants.GravitationalConstant * Constants.EarthMass / finalRadius)
            - Math.Sqrt(2 * Constants.GravitationalConstant * Constants.EarthMass / initialRadius);
        return deltaV;
    }

    private static double CalculateRocketDeltaV(double isp, double fullMass, double emptyMass)
    {
        double deltaV = isp * Constants.Gravity * Math.Log(fullMass / emptyMass);
        return deltaV;
    }
}

public static class Constants
{
    public const double GravitationalConstant = 6.67430e-11;
    public const double EarthMass = 5.972e24;
    public const double Gravity = 9.81;
}
