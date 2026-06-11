using Microsoft.Data.Sqlite;

namespace HabitTracker.Helpers
{
    internal class SqliteHelper (string connectionString)
    {
        private string _connectionString = connectionString;

        public string ConnectionString { get { return _connectionString; } }

        public void TestConnection()
        {
            using SqliteConnection connection = CreateAndOpenNewConnection();
            connection.Close();
        }
        public SqliteConnection CreateAndOpenNewConnection()
        {
            var connection = new SqliteConnection(_connectionString);
            connection.Open();
            return connection;
        }

        public SqliteCommand CreateCommandWithNewConnection(string query)
        {
            return new SqliteCommand(query, CreateAndOpenNewConnection());
        }
    }
}
