using Microsoft.Data.SqlClient;
using System;

namespace AuthBackend
{
    public class DBConnection
    {
        private static readonly string ConnectionString;

        static DBConnection()
        {
            string server = "DESKTOP-3CLJ74A";
            string database = "SOFTONE_ASSESENT";
            string user = "sa";
            string password = "123";

            ConnectionString = $"Data Source={server};Initial Catalog={database};User ID={user};Password={password};Connection Timeout=180; TrustServerCertificate=True; Integrated Security=True;";
        }

        /// <summary>
        /// Returns a new open SqlConnection
        /// </summary>
        public static SqlConnection GetConnection()
        {
            var connection = new SqlConnection(ConnectionString);

            try
            {
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            { 
                Console.WriteLine("Database connection failed: " + ex.Message);
                throw;  
            }
        }

        /// <summary>
        /// Use this in using blocks to ensure proper disposal
        /// </summary>
        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}