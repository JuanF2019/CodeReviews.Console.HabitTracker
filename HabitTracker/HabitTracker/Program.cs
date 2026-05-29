using HabitTracker;
using HabitTracker.Model;
using HabitTracker.Repositories;
using Microsoft.Extensions.Configuration;

var configurationBuilder = new ConfigurationBuilder();

configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
configurationBuilder.AddJsonFile("appsettings.json", optional:false, reloadOnChange: false);

IConfiguration configuration =  configurationBuilder.Build();

HabitRepository.Configuration = configuration;
HabitRepository habitRepository = HabitRepository.GetInstance();
HabitOccurrenceRespository habitOccurrenceRespository = new (configuration);
Console.WriteLine("Connection OK");

habitRepository.CreateHabitsTable();
Console.WriteLine("habit table created");

habitOccurrenceRespository.CreateHabitsOccurrenceTable();
Console.WriteLine("habitOcurrance table created");

//==================================================

Habit habit = habitRepository.CreateHabit(new Habit(-1, "Test habit","Some description"));
Console.WriteLine($"New habit created:\n{habit}");

Habit habitWithNullDescription = habitRepository.CreateHabit(new Habit(-1, "Test habit 2", null));
Console.WriteLine($"New habit created:\n{habitWithNullDescription}");

//==================================================

HabitOccurrence habitOccurrence = habitOccurrenceRespository.CreateHabitOccurrence(new HabitOccurrence(-1,DateTime.Now,"Some notes",habit));
Console.WriteLine($"New habit occurrence created:\n{habitOccurrence}");

HabitOccurrence habitOccurrenceWithNullNotes = habitOccurrenceRespository.CreateHabitOccurrence(new HabitOccurrence(-1, DateTime.Now, null, habitWithNullDescription));
Console.WriteLine($"New habit occurrence created:\n{habitOccurrenceWithNullNotes}");