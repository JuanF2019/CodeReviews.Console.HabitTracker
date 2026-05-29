using HabitTracker;
using HabitTracker.Model;
using HabitTracker.Repositories;
using Microsoft.Extensions.Configuration;

var configurationBuilder = new ConfigurationBuilder();

configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
configurationBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

IConfiguration configuration = configurationBuilder.Build();

HabitRepository.Configuration = configuration;
HabitRepository habitRepository = HabitRepository.GetInstance();
HabitOccurrenceRespository habitOccurrenceRespository = new(configuration);
Console.WriteLine("Connection OK");

habitRepository.CreateHabitsTable();
Console.WriteLine("habit table created");

habitOccurrenceRespository.CreateHabitsOccurrenceTable();
Console.WriteLine("habitOcurrance table created");

bool exit = false;
string? userInput;

while (!exit)
{
    Console.WriteLine("Main menu:");
    Console.WriteLine("1. Create a new habit");
    Console.WriteLine("2. View and edit habits");

    userInput = Console.ReadLine();

    bool validInput = false;
    int invalidInputCount = 0;

    while (!validInput)
    {
        switch (userInput)
        {
            case "1":
                validInput = true;
                CreateHabit();
                break;
            case "2":
                validInput = true;
                ViewAndEditHabits();
                break;
            default:
                if (invalidInputCount == 0)
                {
                    Console.WriteLine("Invalid input, please try again:");
                }
                else
                {
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    ClearCurrentLine();
                }
                invalidInputCount++;
                userInput = Console.ReadLine();
                break;
        }
    }
    PressEnterToContinueAndClearConsoleAfterwards();
}

static void ClearCurrentLine()
{
    Console.Write($"{new String(' ', Console.BufferWidth)}\r");
}

static void PressEnterToContinueAndClearConsoleAfterwards()
{
    Console.WriteLine("Press enter to continue:");

    var key = Console.ReadKey(intercept: true);

    while (key.Key != ConsoleKey.Enter)
    {
        key = Console.ReadKey(intercept: true);
    }

    Console.Clear();
}
static void CreateHabit()
{
    Console.WriteLine("Type a name for your habit:");

    string? userInput = Console.ReadLine();

    while (userInput == null || userInput == "")
    {
        Console.WriteLine("Name is required");
        userInput = Console.ReadLine();
    }

    string habitName = userInput;

    Console.WriteLine("Please write a description for your habit: ");

    string? habitDescription = Console.ReadLine();

    Habit habit = new(-1, habitName, habitDescription);

    try
    {
        habit = HabitRepository.GetInstance().CreateHabit(habit);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error ocurred while creating the habit, please try again later: {ex.Message}");
    }
    Console.WriteLine($"Habit created successfully: {habit}");
}

static Habit PickAHabit(List<Habit> habits)
{
    return null;
}

static void EditHabit(Habit habit)
{

}

static void DeleteHabit(Habit habit)
{

}

static void ViewAndEditHabits()
{    
    List<Habit> habits = HabitRepository.GetInstance().GetAllHabits();

    string tableHeader = $"{"ROW#",-6} | {"ID",-4} | {"NAME",-16} | {"DESCRIPTION",-32} |";

    Console.WriteLine(tableHeader);
    Console.WriteLine(new String('-', tableHeader.Length));

    for (int i = 0; i < habits.Count; i++)
    {
        Habit habit = habits[i];
        Console.WriteLine($"{i,-6} | " +
            $"{habit.ID,-4} | " +
            $"{habit.Name[..Math.Min(habit.Name.Length, 16)],-16} | " +
            $"{(habit.Description != null ? habit.Description[..Math.Min(habit.Description.Length, 32)] : ""),-32} |");
    }

    Console.WriteLine("Select an action (e : edit, r : remove, b : go back to main menu):");

    string? userInput = Console.ReadLine();

    bool validInputAction = false;
    int invalidInputActionCount = 0;

    while (!validInputAction)
    {
        switch (userInput)
        {
            case "e":
                break;
            case "d":
                break;
            case "n":
                break;
            default:
                if (invalidInputActionCount == 0)
                {
                    Console.WriteLine("Invalid input, please try again:");
                }
                else
                {
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    ClearCurrentLine();
                }
                invalidInputActionCount++;
                userInput = Console.ReadLine();
                break;
        }
    }

}

/*

Habit habit = habitRepository.CreateHabit(new Habit(-1, "Test habit","Some description"));
Console.WriteLine($"New habit created:\n{habit}");

Habit habitWithNullDescription = habitRepository.CreateHabit(new Habit(-1, "Test habit 2", null));
Console.WriteLine($"New habit created:\n{habitWithNullDescription}");

//==================================================

HabitOccurrence habitOccurrence = habitOccurrenceRespository.CreateHabitOccurrence(new HabitOccurrence(-1,DateTime.Now,"Some notes",habit));
Console.WriteLine($"New habit occurrence created:\n{habitOccurrence}");

HabitOccurrence habitOccurrenceWithNullNotes = habitOccurrenceRespository.CreateHabitOccurrence(new HabitOccurrence(-1, DateTime.Now, null, habitWithNullDescription));
Console.WriteLine($"New habit occurrence created:\n{habitOccurrenceWithNullNotes}");

*/