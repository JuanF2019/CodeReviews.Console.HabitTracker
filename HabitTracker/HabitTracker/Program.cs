using HabitTracker;
using Microsoft.Extensions.Configuration;

var configurationBuilder = new ConfigurationBuilder();

configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
configurationBuilder.AddJsonFile("appsettings.json", optional:false, reloadOnChange: false);

IConfiguration configuration =  configurationBuilder.Build();

var habitDatabase = new HabitDatabase(configuration);

habitDatabase.TestConnection();
Console.WriteLine("Connection OK!");

habitDatabase.CreateHabitsTable();
Console.WriteLine("Table created");

Habit habit = habitDatabase.CreateHabit(new Habit(-1, "Test habit", DateTime.Now));

Console.WriteLine($"New habit created: \n {habit}");