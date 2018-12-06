using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            String errorMsg;
            OdbcConnection con = Connect("sa", "sa.pwd", "2055", "192.168.190.200", "mydb", out errorMsg);
            Console.WriteLine(errorMsg);
            if (con != null)
            {
                Console.WriteLine("In database {0}", con.Database);
                OdbcCommand command = con.CreateCommand();
                command.CommandText = "SELECT name FROM sysobjects WHERE type='U' ORDER BY name";
                OdbcDataReader reader = command.ExecuteReader();
               
                 foreach (String fName in reader.GetName())
                {
                    Console.Write(fName + ":");
                }
                Console.WriteLine();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        String col = reader.GetValue(i).ToString();
                        Console.Write(col + ":");
                    }
                    Console.WriteLine();
                }
                reader.Close();
                command.Dispose();
                Close(con);
                Console.WriteLine("Press any key too continue...");
                Console.ReadLine();
            }
        }

        private static OdbcConnection Connect(String strUserName, String strPassword, String strPort, String strHostName, String dbName, out String strErrorMsg)
        {
            OdbcConnection con = null;
            strErrorMsg = String.Empty;
            try
            {
                String conString = "Driver={Adaptive Server Enterprise};server=" + strHostName + ";" + "port=" + strPort + ";db=" + dbName + ";uid=" + strUserName + ";pwd=" + strPassword + ";";
                con = new OdbcConnection(conString);
                con.Open();
            }
            catch (Exception exp)
            {
                con = null;
                strErrorMsg = exp.ToString();
            }

            return con;
        }

        private static void Close(OdbcConnection con)
        {
            con.Close();
        }
    }
}