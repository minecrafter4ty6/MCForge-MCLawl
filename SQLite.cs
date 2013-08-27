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
using System.Data.SQLite;
namespace MCForge
{
    namespace SQL
    {
        public static class SQLite //: Database //Extending for future improvement (Making it object oriented later).
        {
            private static string connStringFormat = "Data Source =" + Server.apppath + "/MCForge.db; Version =3; Pooling ={0}; Max Pool Size =1000;";
            private static SQLiteParameterCollection parameters = new SQLiteCommand().Parameters;

            public static string connString { get { return String.Format(connStringFormat, Server.DatabasePooling); } }
            [Obsolete("Preferably use Database.executeQuery instead")]
            public static void executeQuery(string queryString)
            {
                Database.executeQuery(queryString);
            }
            [Obsolete("Preferably use Database.executeQuery instead")]
            public static DataTable fillData(string queryString, bool skipError = false)
            {
                return Database.fillData(queryString, skipError);
            }

            /// <summary>
            /// Adds a parameter to the parameterized SQLite query.
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
            private static void AddSQLiteParameters(SQLiteCommand command) {
                foreach (SQLiteParameter param in parameters)
                    command.Parameters.Add(param);
            }
            private static void AddSQLiteParameters(SQLiteDataAdapter dAdapter) {
                foreach (SQLiteParameter param in parameters)
                    dAdapter.SelectCommand.Parameters.Add(param);
            }

            internal static void execute(string queryString) {
                using (var conn = new SQLiteConnection(SQLite.connString)) {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn)) {
                        AddSQLiteParameters(cmd);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }

            internal static void fill(string queryString, DataTable toReturn) {
                using (var conn = new SQLiteConnection(SQLite.connString)) {
                    conn.Open();
                    using (SQLiteDataAdapter da = new SQLiteDataAdapter(queryString, conn)) {
                        AddSQLiteParameters(da);
                        da.Fill(toReturn);
                    }
                    conn.Close();
                }
            }
        }
    }
}
