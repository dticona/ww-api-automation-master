// <copyright file="DataBaseConnection.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>

namespace WhiteWingsApi.Utils.DataBase
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class handles the DataBase conection
    /// </summary>
    public static class DataBaseConnection
    {
        public static string GetConnetionString()
        {
            string dbServer = ConfigurationSettings.AppSettings["DBServer"];
            string dbName = ConfigurationSettings.AppSettings["DBName"];
            string dbUser = ConfigurationSettings.AppSettings["DBUser"];
            string dbPassword = ConfigurationSettings.AppSettings["DBPassword"];

            return $@"Data Source={dbServer};Initial Catalog={dbName};User ID={dbUser}; Password={dbPassword}";
        }


        /// <summary>
        /// Gets the SQL db connection.
        /// </summary>
        /// <returns>the SQL connection</returns>
        public static SqlConnection GetSqlConnection()
        {
            SqlConnection connection = new SqlConnection(GetConnetionString());
            return connection;
        }

        /// <summary>
        /// Gets SQL Data
        /// </summary>
        /// <param name="query">the query to get values of db</param>
        /// <returns>he table with values related to query</returns>
        public static DataTable GetSqlData(string query)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = GetSqlConnection())
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                new SqlDataAdapter(command).Fill(table);
            }
            return table;
        }

        /// <summary>
        /// Executes a query to insert or update data in a database
        /// </summary>
        /// <param name="dbName">database name according the enviroment file</param>
        /// <param name="query">query to execute</param>
        public static void ExecuteSqlQuery(string query)
        {
            using (SqlConnection connection = GetSqlConnection())
            {
                SqlCommand command;
                connection.Open();
                command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
