/*
Copyright (C) 2010-2013 David Mitchell

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using System;
using System.Data;
using MySql.Data.MySqlClient;
namespace MCForge
{
    namespace SQL
    {
        public static class MySQL //: Database //Extending for future improvement (Making it object oriented later)
        {
            private static string connStringFormat = "Data Source={0};Port={1};User ID={2};Password={3};Pooling={4}";
            private static MySqlParameterCollection parameters = new MySqlCommand().Parameters;

            public static string connString { get { return String.Format(connStringFormat, Server.MySQLHost, Server.MySQLPort, Server.MySQLUsername, Server.MySQLPassword, Server.DatabasePooling); } }
            [Obsolete("Preferably use Database.executeQuery instead")]
            public static void executeQuery(string queryString, bool createDB = false)
            {
                Database.executeQuery(queryString, createDB);
            }
            [Obsolete("Preferably use Database.executeQuery instead")]
            public static DataTable fillData(string queryString, bool skipError = false)
            {
                return Database.fillData(queryString, skipError);
            }

            /// <summary>
            /// Adds a parameter to the parameterized MySQL query.
            /// Use this before executing the query.
            /// </summary>
            /// <param name="name">The name of the parameter</param>
            /// <param name="param">The value of the parameter</param>
            public static void AddParams(string name, object param) {
                parameters.AddWithValue(name, param);
            }
            /// <summary>
            /// Clears the parameters added with <see cref="MCForge.SQL.MySQL.AddParams(System.string, System.string)"/>
            /// <seealso cref="MCForge.SQL.MySQL"/>
            /// </summary>
            public static void ClearParams() {
                parameters.Clear();
            }
            private static void AddMySQLParameters(MySqlCommand command) {
                foreach (MySqlParameter param in parameters)
                    command.Parameters.Add(param);
            }
            private static void AddMySQLParameters(MySqlDataAdapter dAdapter) {
                foreach (MySqlParameter param in parameters)
                    dAdapter.SelectCommand.Parameters.Add(param);
            }

            internal static void execute(string queryString, bool createDB = false) {
                using (var conn = new MySqlConnection(connString)) {
                    conn.Open();
                    if (!createDB) {
                        conn.ChangeDatabase(Server.MySQLDatabaseName);
                    }
                    using (MySqlCommand cmd = new MySqlCommand(queryString, conn)) {
                        AddMySQLParameters(cmd);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }

            internal static void fill(string queryString, DataTable toReturn) {
                using (var conn = new MySqlConnection(connString)) {
                    conn.Open();
                    conn.ChangeDatabase(Server.MySQLDatabaseName);
                    using (MySqlDataAdapter da = new MySqlDataAdapter(queryString, conn)) {
                        AddMySQLParameters(da);
                        da.Fill(toReturn);
                    }
                    conn.Close();
                }
            }
        }
    }
}

