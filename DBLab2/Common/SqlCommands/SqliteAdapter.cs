using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Microsoft.Data.Sqlite;

namespace Common.SqlCommands {
    public static class SqliteAdapter {
        private static SqliteConnection? _sqlConnection;

        static SqliteAdapter() {
            _sqlConnection = null;
        }

        private static void Check() {
            if (_sqlConnection is null) {
                throw new MemberAccessException("SQL connection is not set. Call 'SetDatabase before using any other method.'");
            }
        }

        /// <summary>
        /// Sets the database to operate with.
        /// </summary>
        /// <param name="databasePath">Path to an EXISTING database.</param>
        public static bool TrySetDatabase(string databasePath) {
            var backup = _sqlConnection;
            var pervName = GlobalContainer.BdSelected;
            try {
                _sqlConnection = new SqliteConnection($"Data Source = {databasePath}");
                _sqlConnection.Open();
                GlobalContainer.BdSelected = Path.GetFileName(databasePath);

                var dict = new Dictionary<string, string[]>();
                var tables = new List<string>();
                using var command = _sqlConnection.CreateCommand();
                command.CommandText = "SELECT name FROM sqlite_master WHERE type = 'table' ORDER BY 1";

                var dt = new DataTable();
                using (var reader = command.ExecuteReader()) {
                    dt.Load(reader);
                }

                tables.AddRange(dt.Rows
                    .Cast<DataRow>()
                    .Select(row => row.ItemArray[0]?.ToString() ?? "")
                    .Where(s => !s.Contains("sqlite") && !s.Contains("__")));

                foreach (var table in tables) {
                    command.CommandText = $"SELECT * FROM {table}";
                    using var reader = command.ExecuteReader();
                    var fields = new List<string>();
                    for (var i = 0; i < reader.FieldCount; i++) {
                        fields.Add(reader.GetName(i));
                    }
                    dict.Add(table, fields.ToArray());
                }

                GlobalContainer.CacheMetadata(dict);
            }
            catch (Exception ex) {
                _sqlConnection = backup;
                GlobalContainer.BdSelected = pervName;
                Printer.Error(ex, "Failed to open database connection");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Selects the data from database.
        /// </summary>
        /// <param name="command"> Select command containing data and fields to return. </param>
        /// <returns>List of list of strings. It represents the following format: *field* : *property1* *property2* ... </returns>
        public static List<List<string>> Select(SqlSelect command) {
            Check();
            var data = new List<List<string>>();
            var selectCommand = _sqlConnection!.CreateCommand();
            selectCommand.CommandText = command.Execute();

            data.Add(command.Fields == null ?
                GlobalContainer.Fields(command.Table).ToList() :
                command.Fields.ToList()
            );

            using var reader = selectCommand.ExecuteReader();
            while (reader.Read()) {
                var tmp = new List<string>();
                for (int c = 0; c < reader.FieldCount; c++) {
                    tmp.Add(reader.GetString(c));
                }
                data.Add(tmp);
            }
            return data;
        }

        /// <summary>
        /// Inserts some data to the database.
        /// </summary>
        /// <param name="command">Insert command with the data to insert.</param>
        public static void InsertInto(SqlInsertInto command) {
            Check();

            var insertCommand = _sqlConnection!.CreateCommand();
            insertCommand.CommandText = command.Execute();
            var @return = insertCommand.ExecuteNonQuery();
            Printer.Info("{0} rows were affected", @return);
        }

        /// <summary>
        /// Deletes data from the database
        /// </summary>
        /// <param name="command">Params on what to delete.</param>
        public static void Delete(SqlDelete command) {
            Check();
            var removeCommand = _sqlConnection!.CreateCommand();
            removeCommand.CommandText = command.Execute();
            var @return = removeCommand.ExecuteNonQuery();
            Printer.Info("{0} rows were affected", @return);
        }

        /// <summary>
        /// Updated data in the database
        /// </summary>
        /// <param name="command">Update command with specified data and fields to update.</param>
        public static void Update(SqlUpdate command) {
            Check();
            var updateCommand = _sqlConnection!.CreateCommand();
            updateCommand.CommandText = command.Execute();
            var @return = updateCommand.ExecuteNonQuery();
            Printer.Info("{0} rows were affected", @return);
        }
    }
}
