using FortCode.Service.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace FortCode.Service
{
    public class DbConnectionFactory: IDbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory(string connectionString) => _connectionString = connectionString;

        public IDbConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
