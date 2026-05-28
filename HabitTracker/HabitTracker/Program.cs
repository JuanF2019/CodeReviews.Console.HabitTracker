using HabitTracker;
using HabitTracker.Model;
using HabitTracker.Repositories;
using Microsoft.Extensions.Configuration;

var configurationBuilder = new ConfigurationBuilder();

configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
configurationBuilder.AddJsonFile("appsettings.json", optional:false, reloadOnChange: false);

IConfiguration configuration =  configurationBuilder.Build();

HabitRepository.Configuration = configuration;


var habitDatabase = new HabitDatabase(configuration);

habitDatabase.TestConnection();
Console.WriteLine("Connection OK!");

habitDatabase.CreateHabitsTable();
Console.WriteLine("Table created");

Habit habit = habitDatabase.CreateHabit(new Habit(-1, "Test habit", DateTime.Now, 12));

Console.WriteLine($"New habit created: \n {habit}");