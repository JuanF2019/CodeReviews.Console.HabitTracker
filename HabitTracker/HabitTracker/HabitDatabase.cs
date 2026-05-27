using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace HabitTracker
{
    internal class HabitDatabase
    {
        private string _connectionString;
        private const string ConnectionStringPath = "Databases:Habits:ConnectionString";

        public HabitDatabase(IConfiguration configuration)
        {
            _connectionString = configuration[ConnectionStringPath] ?? throw new Exception($"Connection string not found at path: '{ConnectionStringPath}'");
        }

        private SqliteConnection CreateAndOpenNewConnection()
        {
            var connection = new SqliteConnection(_connectionString);
            connection.Open();
            return connection;
        }

        private SqliteCommand CreateCommandWithNewConnection(string query)
        {
            return new SqliteCommand(query, CreateAndOpenNewConnection());
        }

        public void TestConnection()
        {
            using SqliteConnection connection = CreateAndOpenNewConnection();
            connection.Close();
        }

        public void CreateHabitsTable()
        {
            string createQuery = "CREATE TABLE IF NOT EXISTS habits (" +
                "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                "name TEXT NOT NULL," +
                "dateAndTime DATETIME NOT NULL);";

            using SqliteCommand command = CreateCommandWithNewConnection(createQuery);
            command.ExecuteNonQuery();
        }

        public Habit CreateHabit(Habit habit)
        {
            string insertQuery = "INSERT INTO habits (name, dateAndTime) VALUES (" +
                "@name, @dateAndTime) RETURNING *;";

            using SqliteCommand command = CreateCommandWithNewConnection(insertQuery);

            command.Parameters.AddWithValue("@name", habit.Name);
            command.Parameters.AddWithValue("@dateAndTime", habit.DateAndTime);

            var reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                throw new Exception("No row returned after creating row.");
            }

            reader.Read();

            int id = Convert.ToInt32(reader["id"]);
            string name = Convert.ToString(reader["name"]) ?? "";
            DateTime dateAndTime = Convert.ToDateTime(reader["dateAndTime"]);

            return new Habit(id,name,dateAndTime);
        }
    }
}
