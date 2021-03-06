﻿/*
	Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCForge)
	
	Dual-licensed under the	Educational Community License, Version 2.0 and
	the GNU General Public License, Version 3 (the "Licenses"); you may
	not use this file except in compliance with the Licenses. You may
	obtain a copy of the Licenses at
	
	http://www.opensource.org/licenses/ecl2.php
	http://www.gnu.org/licenses/gpl-3.0.html
	
	Unless required by applicable law or agreed to in writing,
	software distributed under the Licenses are distributed on an "AS IS"
	BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
	or implied. See the Licenses for the specific language governing
	permissions and limitations under the Licenses.
*/
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MySql.Data.MySqlClient;
using MySql.Data.Types;

namespace MCForge
{
    static class MySQL
    {
        private static string connString = "Data Source=" + Server.MySQLHost + ";Port=" + Server.MySQLPort + ";User ID=" + Server.MySQLUsername + ";Password=" + Server.MySQLPassword + ";Pooling=" + Server.MySQLPooling;

        public static void executeQuery(string queryString, bool createDB = false)
        {
            if (!Server.useMySQL) return;

            int totalCount = 0;
    retry:  try
            {
                using (var conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    if (!createDB)
                    {
                        conn.ChangeDatabase(Server.MySQLDatabaseName);
                    }
                    MySqlCommand cmd = new MySqlCommand(queryString, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                if (!createDB)
                {
                    totalCount++;
                    if (totalCount > 10)
                    {
                        File.WriteAllText("MySQL_error.log", DateTime.Now + " " + queryString);
                        Server.ErrorLog(e);
                    }
                    else
                    {
                        goto retry;
                    }
                }
                else
                {
                    throw e;
                }
            }
        }

        public static DataTable fillData(string queryString, bool skipError = false)
        {
            DataTable toReturn = new DataTable("toReturn");
            if (!Server.useMySQL) return toReturn;

            int totalCount = 0;
    retry:  try
            {
                using (var conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    conn.ChangeDatabase(Server.MySQLDatabaseName);
                    using (MySqlDataAdapter da = new MySqlDataAdapter(queryString, conn))
                    {
                        da.Fill(toReturn);
                    }
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                totalCount++;
                if (totalCount > 10)
                {
                    if (!skipError)
                    {
                        File.WriteAllText("MySQL_error.log", DateTime.Now + " " + queryString);
                        Server.ErrorLog(e);
                    }
                }
                else
                    goto retry;
            }

            return toReturn;
        }
    }
}
