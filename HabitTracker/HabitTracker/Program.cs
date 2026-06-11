using HabitTracker.Repositories;
using Microsoft.Extensions.Configuration;
using HabitTracker.UI;

var configurationBuilder = new ConfigurationBuilder();

configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
configurationBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

IConfiguration configuration = configurationBuilder.Build();

HabitRepository.Configuration = configuration;
HabitRepository habitRepository = HabitRepository.GetInstance();
HabitOccurrenceRepository.Configuration = configuration;
HabitOccurrenceRepository habitOccurrenceRespository = HabitOccurrenceRepository.GetInstance();
Console.WriteLine("Connection OK");

habitRepository.CreateHabitsTable();
Console.WriteLine("habit table created");

habitOccurrenceRespository.CreateHabitsOccurrenceTable();
Console.WriteLine("habitOccurrence table created");

Console.Clear();

UserInterface.MainMenu();
