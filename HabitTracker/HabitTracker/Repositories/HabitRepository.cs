using HabitTracker.Model;
using HabitTracker.SqliteHelpers;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace HabitTracker.Repositories
{
    internal sealed class HabitRepository
    {
        private const string ConnectionStringPath = "Databases:Habits:ConnectionString";        
        public static IConfiguration? Configuration
        { 
            get;
            set
            {
                if(_instance != null)
                {
                    throw new InvalidOperationException("Cannot change the configuration since the repository has already been initialized.");
                }
                field = value;
            }
        }
        private static HabitRepository? _instance;

        private readonly SqliteHelper _sqliteHelper;

        public static HabitRepository GetInstance()
        {
            if( _instance == null)
            {
                if(Configuration == null)
                {
                    throw new InvalidOperationException("Cannot create an instance since no configuration has been set for the repository.");
                }
                _instance = new HabitRepository(Configuration);
            }
            return _instance;
        }

        private HabitRepository(IConfiguration configuration)
        {
            string connectionString = configuration[ConnectionStringPath] ?? throw new Exception($"Connection string not found at path: '{ConnectionStringPath}'");

            _sqliteHelper = new SqliteHelper(connectionString);

            try
            {
                _sqliteHelper.TestConnection();
            }
            catch (SqliteException ex)
            {
                throw new Exception("An error ocurred while testing the database connection with the provided connection string: " + ex.Message);
            }
        }

        public void CreateHabitsTable()
        {
            string createQuery = "CREATE TABLE IF NOT EXISTS habits (" +
                "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                "name TEXT NOT NULL," +
                "description TEXT)";

            using SqliteCommand command = _sqliteHelper.CreateCommandWithNewConnection(createQuery);
            command.ExecuteNonQuery();
        }

        //Returns a habit because the caller does not have the id which is assigned during row creation.
        public Habit CreateHabit(Habit habit)
        {
            string insertQuery = "INSERT INTO habits (name, description) VALUES (" +
                "@name, @description) RETURNING *;";

            using SqliteCommand command = _sqliteHelper.CreateCommandWithNewConnection(insertQuery);

            command.Parameters.AddWithValue("@name", habit.Name);
            command.Parameters.AddWithValue("@description", habit.Description == null? DBNull.Value:habit.Description);

            SqliteDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                throw new Exception("No row returned after creating row.");
            }

            reader.Read();

            int id = Convert.ToInt32(reader["id"]);
            string name = Convert.ToString(reader["name"]) ?? "";
            string? description = Convert.ToString(reader["description"]);

            return new Habit(id, name, description);
        }

        public List<Habit> GetAllHabits()
        {
            string selectQuery = "SELECT * FROM habits;";

            using SqliteCommand command = _sqliteHelper.CreateCommandWithNewConnection(selectQuery);
            SqliteDataReader reader = command.ExecuteReader();

            List<Habit> habits = [];

            while (reader.Read())
            {
                int id = Convert.ToInt32(reader["id"]);
                string name = Convert.ToString(reader["name"]) ?? "";
                string? description = Convert.ToString(reader["description"]);

                habits.Add(new Habit(id, name, description));
            }
            return habits;
        }

        public Habit? GetHabitById(int id)
        {
            string selectQuery = "SELECT * FROM habits WHERE id = @id;";

            using SqliteCommand command = _sqliteHelper.CreateCommandWithNewConnection(selectQuery);
            command.Parameters.AddWithValue("id", id);

            SqliteDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                return null;
            }

            reader.Read();

            string name = Convert.ToString(reader["name"]) ?? "";
            string? description = Convert.ToString(reader["description"]);

            return new Habit(id, name, description);
        }

        //Does not return anything because the user already holds the most up to date data for that record.
        public void UpdateHabit(Habit habit)
        {
            string updateQuery = "UPDATE habits SET name = @name, description = @description WHERE id = @id;";

            using SqliteCommand command = _sqliteHelper.CreateCommandWithNewConnection(updateQuery);

            command.Parameters.AddWithValue("@id", habit.ID);
            command.Parameters.AddWithValue("@name", habit.Name);
            command.Parameters.AddWithValue("@description", habit.Description == null ? DBNull.Value:habit.Description);            

            int numberOfUpdatedRecords = command.ExecuteNonQuery();

            if(numberOfUpdatedRecords == 0)
            {
                throw new Exception($"No habit found that matches the given habit with id = {habit.ID}");
            }
        }

        public void DeleteHabitById(int id)
        {
            string deleteQuery = "DELETE FROM habits WHERE id = @id";

            using SqliteCommand command = _sqliteHelper.CreateCommandWithNewConnection(deleteQuery);

            command.Parameters.AddWithValue("@id", id);

            int numberOfDeletedRecords = command.ExecuteNonQuery();

            if (numberOfDeletedRecords == 0)
            {
                throw new Exception($"No habit found that has the given id. (id = {id})");
            }
        }
    }
}
