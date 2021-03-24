using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Data.Sqlite;
using DBLab2.ConsoleController.SqlCommands;

namespace DBLab2.DBLogic {
    public sealed class SqliteAdapter {
        private static SqliteConnection sqlConnection;

        static SqliteAdapter() {
            sqlConnection = null;
        }

        private static void Check() {
            if (sqlConnection is null) {
                throw new NullReferenceException("SQL connection is not set.");
            }
        }

        /// <summary>
        /// Sets the database to operate with.
        /// </summary>
        /// <param name="databasePath">Path to an EXISTING database.</param>
        public static void SetDatabase(string databasePath) {
            if (sqlConnection is not null) {
                sqlConnection.Dispose();
            }
            sqlConnection = new SqliteConnection($"Data Source = {databasePath}");
            sqlConnection.Open();
        }

        /// <summary>
        /// Selects the data from database.
        /// </summary>
        /// <param name="command"> Select command containing data and fields to return. </param>
        /// <returns>List of list of strings. It represents the following format: *field* : *property1* *property2* ... </returns>
        public static List<List<string>> Select(SqlSelect command) {
            Check();
            var data = new List<List<string>>();
            var selectCommand = sqlConnection.CreateCommand();
            selectCommand.CommandText = command.Execute();

            using (var reader = selectCommand.ExecuteReader()) {
                while (reader.Read()) {
                    var tmp = new List<string>();
                    for (int c = 0; c < command.Fields.Count; c++) {
                        tmp.Add(reader.GetString(c));
                    }
                    data.Add(tmp);
                }
            }
            return data;
        }

        /// <summary>
        /// Inserts some data to the database.
        /// </summary>
        /// <param name="command">Insert command with the data to insert.</param>
        public static void InsertInto(SqlInsertInto command) {
            Check();            
            var insertCommand = sqlConnection.CreateCommand();
            insertCommand.CommandText = command.Execute();
            insertCommand.ExecuteScalar();
        }

        /// <summary>
        /// Deletes data from the database
        /// </summary>
        /// <param name="command">Params on what to delete.</param>
        public static void Delete(SqlDelete command) {
            Check();
            var removeCommand = sqlConnection.CreateCommand();
            removeCommand.CommandText = command.Execute();
            removeCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// Updated data in the database
        /// </summary>
        /// <param name="command">Update command with specified data and fields to update.</param>
        public static void Update(SqlUpdate command) {
            Check();
            var updateCommand = sqlConnection.CreateCommand();
            updateCommand.CommandText = command.Execute();
            updateCommand.ExecuteNonQuery();
        }
    }
}
