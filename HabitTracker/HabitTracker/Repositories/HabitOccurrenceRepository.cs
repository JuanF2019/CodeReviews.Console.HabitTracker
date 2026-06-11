using HabitTracker.Model;
using HabitTracker.Helpers;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace HabitTracker.Repositories
{
    internal class HabitOccurrenceRepository
    {
        private const string ConnectionStringPath = "Databases:Habits:ConnectionString";
        private static HabitOccurrenceRepository? _instance;
        private readonly SqliteHelper _sqliteHelper;
        private readonly HabitRepository _habitRepository = HabitRepository.GetInstance();
        public static IConfiguration? Configuration
        {
            get;
            set
            {
                if (_instance != null)
                {
                    throw new InvalidOperationException("Cannot change the configuration since the repository has already been initialized.");
                }
                field = value;
            }
        }
        public static HabitOccurrenceRepository GetInstance()
        {
            if (_instance == null)
            {
                if (Configuration == null)
                {
                    throw new InvalidOperationException("Cannot create an instance since no configuration has been set for the repository.");
                }
                _instance = new HabitOccurrenceRepository(Configuration);
            }
            return _instance;
        }

        private HabitOccurrenceRepository(IConfiguration configuration)
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

        public void CreateHabitsOccurrenceTable()
        {
            string createQuery = "CREATE TABLE IF NOT EXISTS habitsOccurrences (" +
                "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                "occurredAt DATE TIME NOT NULL," +
                "notes TEXT," +
                "habitId INTEGER NOT NULL," +
                "FOREIGN KEY (habitId) REFERENCES habits(id));";

            using SqliteCommand command = _sqliteHelper.CreateCommandWithNewConnection(createQuery);
            command.ExecuteNonQuery();
        }

        public HabitOccurrence CreateHabitOccurrence(HabitOccurrence habitOccurrence)
        {
            string insertQuery = "INSERT INTO habitsOccurrences (occurredAt, notes, habitId) VALUES (" +
                "@occurredAt, @notes, @habitId) RETURNING *;";

            using SqliteConnection connection = _sqliteHelper.CreateAndOpenNewConnection();

            using SqliteCommand insertCommand = new SqliteCommand(insertQuery, connection);

            insertCommand.Parameters.AddWithValue("@occurredAt", habitOccurrence.OccurredAt);
            insertCommand.Parameters.AddWithValue("@notes", habitOccurrence.Notes == null ? DBNull.Value : habitOccurrence.Notes);
            insertCommand.Parameters.AddWithValue("@habitId", habitOccurrence.Habit.ID);

            SqliteDataReader insertReader = insertCommand.ExecuteReader();

            if (!insertReader.HasRows)
            {
                throw new Exception("No row returned after creating row.");
            }

            insertReader.Read();

            int id = Convert.ToInt32(insertReader["id"]);
            DateTime occurredAt = Convert.ToDateTime(insertReader["occurredAt"]);
            string? notes = Convert.ToString(insertReader["notes"]);

            int habitId = Convert.ToInt32(insertReader["habitId"]);
            Habit relatedHabit = _habitRepository.GetHabitById(habitId) ?? throw new Exception("Unexpected error. Related habit was not found.");
            
            return new HabitOccurrence(id, occurredAt, notes, relatedHabit);
        }

        public List<HabitOccurrence> GetAllHabitOccurrences()
        {
            string selectQuery = "SELECT * FROM habitsOccurrences;";

            using SqliteCommand command = _sqliteHelper.CreateCommandWithNewConnection(selectQuery);
            SqliteDataReader reader = command.ExecuteReader();

            List<HabitOccurrence> habitsOccurrences = [];

            while (reader.Read())
            {
                int id = Convert.ToInt32(reader["id"]);
                DateTime occurredAt = Convert.ToDateTime(reader["occurredAt"]);
                string? notes = Convert.ToString(reader["notes"]);

                int habitId = Convert.ToInt32(reader["habitId"]);
                Habit relatedHabit = _habitRepository.GetHabitById(habitId) ?? throw new Exception("Unexpected error. Related habit was not found.");

                habitsOccurrences.Add(new HabitOccurrence(id, occurredAt, notes, relatedHabit));
            }
            return habitsOccurrences;
        }

        public HabitOccurrence? GetHabitOccurrenceById(int id)
        {
            string selectQuery = "SELECT * FROM habitsOccurrences WHERE id = @id;";

            using SqliteCommand command = _sqliteHelper.CreateCommandWithNewConnection(selectQuery);
            command.Parameters.AddWithValue("id", id);

            SqliteDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                return null;
            }

            reader.Read();

            DateTime occurredAt = Convert.ToDateTime(reader["occurredAt"]);
            string? notes = Convert.ToString(reader["notes"]);

            int habitId = Convert.ToInt32(reader["habitId"]);
            Habit relatedHabit = _habitRepository.GetHabitById(habitId) ?? throw new Exception("Unexpected error. Related habit was not found.");

            return new HabitOccurrence(id, occurredAt, notes, relatedHabit);
        }

        public void UpdateHabitOccurrence(HabitOccurrence habitOccurrence)
        {
            string updateQuery = "UPDATE habitsOccurrences SET occurredAt = @occurredAt, notes = @notes, habitId = @habitId WHERE id = @id;";

            using SqliteCommand command = _sqliteHelper.CreateCommandWithNewConnection(updateQuery);

            command.Parameters.AddWithValue("@id", habitOccurrence.ID);
            command.Parameters.AddWithValue("@occurredAt", habitOccurrence.OccurredAt);
            command.Parameters.AddWithValue("@notes", habitOccurrence.Notes == null ? DBNull.Value : habitOccurrence.Notes);
            command.Parameters.AddWithValue("@habitId", habitOccurrence.Habit.ID);

            int numberOfUpdatedRecords = command.ExecuteNonQuery();

            if (numberOfUpdatedRecords == 0)
            {
                throw new Exception($"No habit occurrence found that matches the given habit occurrence with id = {habitOccurrence.ID}");
            }
        }

        public void DeleteHabitOccurrenceById(int id)
        {
            string deleteQuery = "DELETE FROM habitsOccurrences WHERE id = @id";

            using SqliteCommand command = _sqliteHelper.CreateCommandWithNewConnection(deleteQuery);

            command.Parameters.AddWithValue("@id", id);

            int numberOfDeletedRecords = command.ExecuteNonQuery();

            if (numberOfDeletedRecords == 0)
            {
                throw new Exception($"No habit occurrence found that has the given id. (id = {id})");
            }
        }

        public void DeleteHabitOccurrencesByRelatedHabitId(int habitId)
        {
            string deleteQuery = "DELETE FROM habitsOccurrences WHERE habitId = @habitId";

            using SqliteCommand command = _sqliteHelper.CreateCommandWithNewConnection(deleteQuery);

            command.Parameters.AddWithValue("@habitId", habitId);

            command.ExecuteNonQuery();
        }


    }
}
