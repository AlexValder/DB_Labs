using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Data.Sqlite;
using DBLab2.ConsoleController.SqlCommands;

namespace DBLab2.DBLogic {
    public sealed class SqliteAdapter {
        private SqliteConnection sqlConnection;

        public SqliteAdapter(string databasePath) {
            sqlConnection = new SqliteConnection($"Data Source = {databasePath}");
            sqlConnection.Open();
        }

        public List<List<string>> Query(SqlSelect command) {
            var selectCommand = sqlConnection.CreateCommand();
            var data = new List<List<string>>();

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

        //Not tested properly
        public long InsertInto(ISqlCommand command) {
            var insertCommand = sqlConnection.CreateCommand();
            insertCommand.CommandText = command.Execute();
            var rawid = (long)insertCommand.ExecuteScalar();
            return rawid;
        }

        //Not tested properly
        public void Remove(ISqlCommand command) {
            var removeCommand = sqlConnection.CreateCommand();
            removeCommand.CommandText = command.Execute();
            removeCommand.ExecuteNonQuery();
        }

        public void Update(ISqlCommand command) {
            throw new NotImplementedException("This method is not implemented yet.");
        }

        ~SqliteAdapter() {
            sqlConnection.Dispose();
        }
    }
}
