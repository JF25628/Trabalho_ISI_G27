using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PremierLeagueAPI.Data
{
    public class DatabaseContext
    {
        private readonly string _connectionString;

        public DatabaseContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task ExecuteQueryAsync(string query)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(query, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<SqlDataReader> ExecuteQueryReaderAsync(string query)
        {
            var connection = GetConnection();
            await connection.OpenAsync();
            var command = new SqlCommand(query, connection);
            return await command.ExecuteReaderAsync();
        }
    }
}
