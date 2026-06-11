using HabitTracker.Model;
using HabitTracker.Repositories;

namespace HabitTracker.UI
{
    internal static class UserInterface
    {
        public static void MainMenu()
        {

            bool exit = false;
            string? userInput;

            while (!exit)
            {
                Console.WriteLine("Main menu:");
                Console.WriteLine("1. Create a new habit");
                Console.WriteLine("2. View and edit habits");
                Console.WriteLine("3. Manage habit occurrences");
                Console.WriteLine("Type \"e\" to exit");

                userInput = (Console.ReadLine() ?? "").ToLower();

                bool validInput = false;
                int invalidInputCount = 0;

                while (!validInput)
                {
                    switch (userInput)
                    {
                        case "1":
                            validInput = true;
                            CreateHabit();
                            PressEnterToContinueAndClearConsoleAfterwards();
                            break;
                        case "2":
                            validInput = true;
                            ViewAndEditHabits();
                            PressEnterToContinueAndClearConsoleAfterwards();
                            break;
                        case "3":
                            validInput = true;
                            ManageHabitOccurrences();
                            PressEnterToContinueAndClearConsoleAfterwards();
                            break;
                        case "e":
                            validInput = true;
                            exit = true;
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
            }
        }
        private static void ClearCurrentLine()
        {
            Console.Write($"{new String(' ', Console.BufferWidth)}\r");
        }

        private static void PressEnterToContinueAndClearConsoleAfterwards()
        {
            PressEnterToContinue();
            Console.Clear();
        }

        private static void PressEnterToContinue()
        {
            Console.WriteLine("Press enter to continue:");
            while (Console.ReadKey(intercept: true).Key != ConsoleKey.Enter) ;
        }
        private static void CreateHabit()
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
                Console.WriteLine($"Habit created successfully: {habit}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error ocurred while creating the habit, please try again later: {ex.Message}");
            }
        }

        private static void PrintHabits(List<Habit> habits)
        {
            if (habits.Count == 0)
            {
                Console.WriteLine(new String('-', Console.BufferWidth));
                Console.WriteLine("No habits found.");
                Console.WriteLine(new String('-', Console.BufferWidth));
                return;
            }

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
        }

        private static Habit PickAHabit(List<Habit> habits)
        {
            Console.WriteLine("Please type the row number of the habit you want to select:");

            string? selectedRow;
            int selectedRowValue = -1;
            bool isValidSelection = false;
            bool atLeastOneInvalidInput = false;

            do
            {
                selectedRow = Console.ReadLine();
                isValidSelection = int.TryParse(selectedRow, out selectedRowValue) && selectedRowValue >= 0 && selectedRowValue < habits.Count;

                if (!isValidSelection)
                {
                    if (!atLeastOneInvalidInput)
                    {
                        Console.WriteLine("Invalid value, please try again:");
                        atLeastOneInvalidInput = true;
                    }
                    else
                    {
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                        ClearCurrentLine();
                    }
                }
            } while (!isValidSelection);

            return habits[selectedRowValue];
        }

        private static void EditHabit(Habit habit)
        {
            Console.Clear();

            bool exitEditHabitMenu = false;

            Habit updatedHabit = new(habit.ID, habit.Name, habit.Description);

            while (!exitEditHabitMenu)
            {
                Console.WriteLine("Please select which field you want to edit:");
                Console.WriteLine($"1. Name: {updatedHabit.Name}");
                Console.WriteLine($"2. Description: {updatedHabit.Description ?? "No value set"}");
                Console.WriteLine("Type \"s\" to save your changes and go back or \"b\" to go back without saving");

                bool isValidSelection = false;

                do
                {
                    string userSelection = (Console.ReadLine() ?? "").ToLower();

                    switch (userSelection)
                    {
                        case "1":
                            isValidSelection = true;
                            Console.WriteLine("Type a new name for your habit:");

                            bool isValidName;
                            bool atLeastOneInvalidRetry = false;
                            string newName;

                            do
                            {
                                newName = Console.ReadLine() ?? "";
                                isValidName = newName.Length > 0;
                                if (!isValidName)
                                {
                                    if (!atLeastOneInvalidRetry)
                                    {
                                        atLeastOneInvalidRetry = true;
                                        Console.WriteLine("Invalid name please try again:");
                                    }
                                    else
                                    {
                                        Console.CursorTop--;
                                        ClearCurrentLine();
                                    }
                                }
                            } while (!isValidName);

                            updatedHabit.Name = newName;
                            Console.Clear();
                            break;
                        case "2":
                            isValidSelection = true;
                            Console.WriteLine("Type a new description for your habit:");
                            string? newDescription = Console.ReadLine();
                            updatedHabit.Description = newDescription;
                            Console.Clear();
                            break;
                        case "s":
                            isValidSelection = true;
                            try
                            {
                                HabitRepository.GetInstance().UpdateHabit(updatedHabit);
                                habit.Name = updatedHabit.Name;
                                habit.Description = updatedHabit.Description;
                                Console.WriteLine($"Habit updated successfully: {habit}");
                                exitEditHabitMenu = true;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"An error ocurred while updating the habit, please try again later: {ex.Message}");
                            }
                            break;
                        case "b":
                            isValidSelection = true;
                            exitEditHabitMenu = true;
                            break;
                        default:
                            Console.WriteLine("Invalid selection, please try again");
                            break;
                    }
                } while (!isValidSelection);

            }
        }

        private static void RemoveHabit(Habit habitToDelete)
        {
            try
            {
                HabitOccurrenceRepository.GetInstance().DeleteHabitOccurrencesByRelatedHabitId(habitToDelete.ID);
                HabitRepository.GetInstance().DeleteHabitById(habitToDelete.ID);
                Console.WriteLine("Habit deleted succesfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was a problem trying to delete your habit, please try again later: {ex.Message}");
            }
        }

        private static void ViewAndEditHabits()
        {
            bool goBack = false;

            while (!goBack)
            {
                Console.Clear();

                Console.WriteLine("View And Edit Habits\n");
                Console.WriteLine("HABITS:");

                List<Habit> habits = HabitRepository.GetInstance().GetAllHabits();
                PrintHabits(habits);

                if (habits.Count > 0)
                {
                    Console.WriteLine("Select an action (e : edit, r : remove, b : go back to main menu):");

                    bool validInputAction = false;
                    bool atLeastOneInvalidInput = false;

                    do
                    {
                        string? userInput = Console.ReadLine();
                        string processedUserInput = userInput == null ? "" : userInput.ToLower();

                        switch (processedUserInput)
                        {
                            case "e":
                                validInputAction = true;
                                Habit habitToEdit = PickAHabit(habits);
                                EditHabit(habitToEdit);
                                PressEnterToContinue();
                                break;
                            case "r":
                                validInputAction = true;
                                Console.WriteLine("Removing a habit will also delete the related habit occurrences");
                                Habit habitToDelete = PickAHabit(habits);
                                RemoveHabit(habitToDelete);
                                PressEnterToContinue();
                                break;
                            case "b":
                                validInputAction = true;
                                goBack = true;
                                break;
                            default:
                                if (!atLeastOneInvalidInput)
                                {
                                    Console.WriteLine("Invalid input, please try again:");
                                    atLeastOneInvalidInput = true;
                                }
                                else
                                {
                                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                                    ClearCurrentLine();
                                }
                                break;
                        }
                    } while (!validInputAction);
                }
                else
                {
                    goBack = true;
                }
            }
        }
        private static void PrintHabitOccurrences(List<HabitOccurrence> habitOccurrences)
        {
            if (habitOccurrences.Count == 0)
            {
                Console.WriteLine(new String('-', Console.BufferWidth));
                Console.WriteLine("No habits occurrences found.");
                Console.WriteLine(new String('-', Console.BufferWidth));
                return;
            }

            string tableHeader = $"{"ROW#",-6} | {"ID",-4} | {"HABIT",-16} | {"OCCURRED AT",-24} | {"NOTES",-32} |";

            Console.WriteLine(tableHeader);
            Console.WriteLine(new String('-', tableHeader.Length));

            for (int i = 0; i < habitOccurrences.Count; i++)
            {
                HabitOccurrence habitOccurrence = habitOccurrences[i];
                Console.WriteLine($"{i,-6} | " +
                    $"{habitOccurrence.ID,-4} | " +
                    $"{habitOccurrence.Habit.Name[..Math.Min(habitOccurrence.Habit.Name.Length, 16)],-16} | " +
                    $"{habitOccurrence.OccurredAt,-24} | " +
                    $"{(habitOccurrence.Notes != null ? habitOccurrence.Notes[..Math.Min(habitOccurrence.Notes.Length, 32)] : ""),-32} |");
            }
        }

        private static void CreateHabitOccurrence(Habit habit)
        {
            Console.WriteLine("Type the date and time you performed the habit (Use this format: YYYY-MM-DDTHH:MM):");

            string? userInput = Console.ReadLine();
            DateTime occurredAt;
            bool atLeastOneInvalidInput = false;

            while (userInput == null || userInput == "" || !DateTime.TryParse(userInput, out occurredAt))
            {
                userInput = Console.ReadLine();

                if (!atLeastOneInvalidInput)
                {
                    atLeastOneInvalidInput = true;
                    Console.WriteLine("Date is required and must match the format, please try again:");
                }
                else
                {
                    Console.CursorTop--;
                    ClearCurrentLine();
                }
            }

            Console.WriteLine("Write some notes (optional): ");

            string? notes = Console.ReadLine();

            HabitOccurrence habitOccurrence = new(-1, occurredAt, notes, habit);

            try
            {
                habitOccurrence = HabitOccurrenceRepository.GetInstance().CreateHabitOccurrence(habitOccurrence);
                Console.WriteLine($"Habit occurrence created successfully: {habitOccurrence}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error ocurred while creating the habit occurrence, please try again later: {ex.Message}");
            }
        }

        private static HabitOccurrence PickAHabitOccurrence(List<HabitOccurrence> habitOccurrence)
        {
            Console.WriteLine("Please type the row number of the habit occurrence you want to select:");

            string? selectedRow;
            int selectedRowValue = -1;
            bool isValidSelection = false;
            bool atLeastOneInvalidInput = false;

            do
            {
                selectedRow = Console.ReadLine();
                isValidSelection = int.TryParse(selectedRow, out selectedRowValue) && selectedRowValue >= 0 && selectedRowValue < habitOccurrence.Count;

                if (!isValidSelection)
                {
                    if (!atLeastOneInvalidInput)
                    {
                        Console.WriteLine("Invalid value, please try again:");
                        atLeastOneInvalidInput = true;
                    }
                    else
                    {
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                        ClearCurrentLine();
                    }
                }
            } while (!isValidSelection);

            return habitOccurrence[selectedRowValue];
        }

        private static void EditHabitOccurrence(HabitOccurrence habitOccurrence)
        {
            Console.Clear();

            bool exitEditHabitOccurrenceMenu = false;

            HabitOccurrence updatedHabitOccurrence = new(habitOccurrence.ID, habitOccurrence.OccurredAt, habitOccurrence.Notes, habitOccurrence.Habit);

            while (!exitEditHabitOccurrenceMenu)
            {
                Console.WriteLine("Please select which field you want to edit:");
                Console.WriteLine($"1. Occurred At: {updatedHabitOccurrence.OccurredAt}");
                Console.WriteLine($"2. Notes: {updatedHabitOccurrence.Notes ?? "No value set"}");
                Console.WriteLine("Type \"s\" to save your changes and go back or \"b\" to go back without saving");

                bool isValidSelection = false;

                do
                {
                    string userSelection = (Console.ReadLine() ?? "").ToLower();

                    switch (userSelection)
                    {
                        case "1":
                            isValidSelection = true;
                            Console.WriteLine("Type a new occurred at date for your habit occurrence (Format: YYYY-MM-DDTHH:MM):");

                            bool atLeastOneInvalidRetry = false;
                            string? userInput = Console.ReadLine(); ;
                            DateTime newOccurredAt;

                            while (userInput == null || userInput == "" || !DateTime.TryParse(userInput, out newOccurredAt))
                            {

                                Console.WriteLine("Date is required and must match the format, please try again:");
                                userInput = Console.ReadLine();
                                if (!atLeastOneInvalidRetry)
                                {
                                    atLeastOneInvalidRetry = true;
                                    Console.WriteLine("Invalid name please try again:");
                                }
                                else
                                {
                                    Console.CursorTop--;
                                    ClearCurrentLine();
                                }
                            }

                            updatedHabitOccurrence.OccurredAt = newOccurredAt;
                            Console.Clear();
                            break;
                        case "2":
                            isValidSelection = true;
                            Console.WriteLine("Type a note for your habit occurrence:");
                            string? newNote = Console.ReadLine();
                            updatedHabitOccurrence.Notes = newNote;
                            Console.Clear();
                            break;
                        case "s":
                            isValidSelection = true;
                            try
                            {
                                HabitOccurrenceRepository.GetInstance().UpdateHabitOccurrence(updatedHabitOccurrence);
                                habitOccurrence.OccurredAt = updatedHabitOccurrence.OccurredAt;
                                habitOccurrence.Notes = updatedHabitOccurrence.Notes;
                                Console.WriteLine($"Habit occurrence updated successfully: {habitOccurrence}");
                                exitEditHabitOccurrenceMenu = true;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"An error ocurred while updating the habit occurrence, please try again later: {ex.Message}");
                            }
                            break;
                        case "b":
                            isValidSelection = true;
                            exitEditHabitOccurrenceMenu = true;
                            break;
                        default:
                            Console.WriteLine("Invalid selection, please try again");
                            break;
                    }
                } while (!isValidSelection);

            }
        }

        private static void RemoveHabitOccurrence(HabitOccurrence habitOccurrenceToDelete)
        {
            try
            {
                HabitOccurrenceRepository.GetInstance().DeleteHabitOccurrenceById(habitOccurrenceToDelete.ID);
                Console.WriteLine("Habit occurrence deleted succesfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was a problem trying to delete your habit occurrence, please try again later: {ex.Message}");
            }
        }

        private static void ManageHabitOccurrences()
        {
            bool goBack = false;

            while (!goBack)
            {
                Console.Clear();

                Console.WriteLine("Manage Habit Occurrences\n");
                Console.WriteLine("HABIT OCCURRENCES:");

                List<HabitOccurrence> habitOccurrences = HabitOccurrenceRepository.GetInstance().GetAllHabitOccurrences();
                PrintHabitOccurrences(habitOccurrences);

                if (habitOccurrences.Count > 0)
                {
                    Console.WriteLine("Select an action (c : create ; e : edit ; r : remove ; b : go back to main menu):");
                }
                else
                {
                    Console.WriteLine("Select an action (c : create ; b : go back to main menu):");
                }
                bool validInputAction = false;
                bool atLeastOneInvalidInput = false;

                do
                {
                    string? userInput = Console.ReadLine();
                    string processedUserInput = userInput == null ? "" : userInput.ToLower();

                    switch (processedUserInput)
                    {
                        case "c":
                            validInputAction = true;
                            Console.Clear();
                            Console.WriteLine("Create Habit Occurrence");
                            Console.WriteLine("EXISTING HABITS:");
                            List<Habit> habits = HabitRepository.GetInstance().GetAllHabits();
                            PrintHabits(habits);
                            Habit selectedHabit = PickAHabit(habits);
                            CreateHabitOccurrence(selectedHabit);
                            break;
                        case "e" when habitOccurrences.Count > 0:
                            validInputAction = true;
                            HabitOccurrence habitOccurrenceToEdit = PickAHabitOccurrence(habitOccurrences);
                            EditHabitOccurrence(habitOccurrenceToEdit);
                            PressEnterToContinue();
                            break;
                        case "r" when habitOccurrences.Count > 0:
                            validInputAction = true;
                            HabitOccurrence habitOccurrenceToDelete = PickAHabitOccurrence(habitOccurrences);
                            RemoveHabitOccurrence(habitOccurrenceToDelete);
                            PressEnterToContinue();
                            break;
                        case "b":
                            validInputAction = true;
                            goBack = true;
                            break;
                        default:
                            if (!atLeastOneInvalidInput)
                            {
                                Console.WriteLine("Invalid input, please try again:");
                                atLeastOneInvalidInput = true;
                            }
                            else
                            {
                                Console.SetCursorPosition(0, Console.CursorTop - 1);
                                ClearCurrentLine();
                            }
                            break;
                    }
                } while (!validInputAction);

            }
        }
    }
}
