using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FOPcsvToSQLApp
{
    static partial class SQL
    {

        private static StringBuilder sb = new StringBuilder();
        readonly static string connectionString =
                  "Data Source=PCVL\\SQL2014EX;Initial Catalog=fops;User ID=vick;Password=P@$$001";



        internal static void Find(string nameP, string surnameP, string fatherP)
        {
            sb.Clear();
            sb.Append("Select ").
            Append("[nameFirst],[surname],[father],[type],[status],[index],[region],[adressString]").
            Append(" from [fops].[dbo].[person]");

            if (!nameP.Replace(" ", String.Empty).Equals("") || !surnameP.Replace(" ", String.Empty).Equals("") || !fatherP.Replace(" ", String.Empty).Equals("")) {

                if (!nameP.Replace(" ", String.Empty).Equals("") && !surnameP.Replace(" ", String.Empty).Equals("") && !fatherP.Replace(" ", String.Empty).Equals("")) {
                    sb.Append("WHERE nameFirst like '").Append(nameP).Append("'")
                    .Append("AND surname like '").Append(surnameP).Append("'")
                    .Append("AND father like '").Append(fatherP).Append("'");
                }
                if (!nameP.Replace(" ", String.Empty).Equals("") && !surnameP.Replace(" ", String.Empty).Equals("") && fatherP.Replace(" ", String.Empty).Equals(""))
                {
                    sb.Append("WHERE nameFirst like '").Append(nameP).Append("'")
                    .Append("AND surname like '").Append(surnameP).Append("'");

                }
                if (!nameP.Replace(" ", String.Empty).Equals("") && surnameP.Replace(" ", String.Empty).Equals("") && fatherP.Replace(" ", String.Empty).Equals("")) {
                    sb.Append("WHERE nameFirst like '").Append(nameP).Append("'");
                }
                if (!nameP.Replace(" ", String.Empty).Equals("") && surnameP.Replace(" ", String.Empty).Equals("") && !fatherP.Replace(" ", String.Empty).Equals("")) {
                    sb.Append("WHERE nameFirst like '").Append(nameP).Append("'")
                    .Append("AND father like '").Append(fatherP).Append("'");
                }
                if (nameP.Replace(" ", String.Empty).Equals("") && !surnameP.Replace(" ", String.Empty).Equals("") && !fatherP.Replace(" ", String.Empty).Equals("")) {
                    sb.Append("WHERE surname like '").Append(surnameP).Append("'")
                    .Append("AND father like '").Append(fatherP).Append("'");
                }
                if (nameP.Replace(" ", String.Empty).Equals("") && !surnameP.Replace(" ", String.Empty).Equals("") && fatherP.Replace(" ", String.Empty).Equals("")) {
                    sb.Append("WHERE surname like '").Append(surnameP).Append("'");
                }
                if (nameP.Replace(" ", String.Empty).Equals("") && surnameP.Replace(" ", String.Empty).Equals("") && !fatherP.Replace(" ", String.Empty).Equals("")) {
                    sb.Append("WHERE father like '").Append(fatherP).Append("'");
                }
            }
            else
            {
                sb.Clear();
                sb.Append("Select TOP 10 ").
                Append("[nameFirst],[surname],[father],[type],[status],[index],[region],[adressString]").
                Append(" from [fops].[dbo].[person]");
                Console.WriteLine("You did not entered any values, top 10 rows will be selected as default query");
            }
            QueryExec(sb.ToString());
        }


        internal static void QueryExec(string sqlQuery)
        {
            Console.WriteLine("Executing your query...");
            using (SqlConnection connection = new SqlConnection(connectionString))

            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();
                    #region do not understand why below code returns System.InvalidOperationException: Invalid attempt to read when no data is present.
                    //        if (reader.HasRows == true)
                    //        {
                    //            Object[] values = new Object[reader.FieldCount];
                    //            int fieldCount = reader.GetValues(values);
                    //            for (int i = 0; i < fieldCount; i++)
                    //            {
                    //                string res = String.Join("|",values);
                    //        Logger.Logging(@"g:\works\c#\trash\sqlselectlog.log", res, LogStatus.Info);
                    //    }
                    //        }
                    //        else
                    //        { Logger.Logging(@"g:\works\c#\trash\sqlselectlog.log", "no results", LogStatus.Info); }
                    #endregion

                    while (reader.Read())
                    {

                        string nameFirst = reader.GetString(0);
                        string surname = reader.GetString(1);
                        string father = reader.GetString(2);
                        string type = reader.GetString(3);
                        string status = reader.GetString(4);
                        int index = reader.GetInt32(5);
                        string region = reader.GetString(6);
                        string adressString = reader.GetString(7);
                        StringBuilder sbs = new StringBuilder();
                        sbs.Append(nameFirst).Append("\t").
                        Append(surname).Append("\t").
                        Append(father).Append("\t").
                        Append(type).Append("\t").
                        Append(index).Append("\t").
                        Append(status).Append("\t").
                        Append(region).Append("\t").
                        Append(adressString).Append("\t");
                        Logger.Logging(sbs.ToString());
                    }

                    reader.Close();
                    command.Dispose();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Completed +\n PLease find your query result in G:\\Works\\C#\\trash\\log.log . " +
                        "in case of errors, it will be recorded to sqlErrorlog in the same folder");
                    Console.ReadKey();

                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Sorry, we got some errors or warining. \nPLease find details in the log in \nG:\\Works\\C#\\trash\\sqlErrorlog.log");

                    if (ex is SqlException)
                    { Logger.Logging(@"G:\Works\C#\trash\sqlErrorlog.log", ex.ToString(), LogStatus.Warn); }
                    else
                    { Logger.Logging(@"G:\Works\C#\trash\sqlErrorlog.log", ex.ToString(), LogStatus.Error); }
                    Console.ReadKey();
                }

            }
        }

        internal static void Select(string WhatToSelect)
        {
            sb.Clear();
            sb.Append("Select ").
            Append(WhatToSelect).
            Append(" from [fops].[dbo].[person]");
            Console.WriteLine("Executing your query...");
            using (SqlConnection connection = new SqlConnection(connectionString))

            {

                string sqlQuery = sb.ToString();

                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    #region do not understand why below code returns System.InvalidOperationException: Invalid attempt to read when no data is present.
                    //        if (reader.HasRows == true)
                    //        {
                    //            Object[] values = new Object[reader.FieldCount];
                    //            int fieldCount = reader.GetValues(values);
                    //            for (int i = 0; i < fieldCount; i++)
                    //            {
                    //                string res = String.Join("|",values);
                    //        Logger.Logging(@"g:\works\c#\trash\sqlselectlog.log", res, LogStatus.Info);
                    //    }
                    //        }
                    //        else
                    //        { Logger.Logging(@"g:\works\c#\trash\sqlselectlog.log", "no results", LogStatus.Info); }
                    #endregion

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string nameFirst = reader.GetString(1);
                        string surname = reader.GetString(2);
                        string father = reader.GetString(3);
                        string type = reader.GetString(4);
                        string status = reader.GetString(5);
                        int index = reader.GetInt32(6);
                        string region = reader.GetString(7);
                        string adressString = reader.GetString(8);
                        int initialId = reader.GetInt32(9);
                        StringBuilder sbs = new StringBuilder();
                        sbs.Append(nameFirst).Append("\t").
                        Append(surname).Append("\t").
                        Append(father).Append("\t").
                        Append(type).Append("\t").
                        Append(index).Append("\t").
                        Append(status).Append("\t").
                        Append(region).Append("\t").
                        Append(adressString).Append("\t").
                        Append(initialId).Append("\t");
                        Logger.Logging(sbs.ToString());
                    }

                    reader.Close();
                    command.Dispose();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Completed +\n PLease find your query result in G:\\Works\\C#\\trash\\log.log . "+
                        "in case of errors, it will be recorded to sqlErrorlog in the same folder");
                    Console.ReadKey();

                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Sorry, we got some errors or warining. \nPLease find details in the log in \nG:\\Works\\C#\\trash\\sqlErrorlog.log");

                    if (ex is SqlException)
                    { Logger.Logging(@"G:\Works\C#\trash\sqlErrorlog.log", ex.ToString(), LogStatus.Warn); }
                    else
                    { Logger.Logging(@"G:\Works\C#\trash\sqlErrorlog.log", ex.ToString(), LogStatus.Error); }
                     Console.ReadKey();
                }

            }
        }


        internal static void Select(string WhatToSelect, string where)
        {
                    sb.Clear();
                    sb.Append("Select ").
                    Append(WhatToSelect).
                    Append(" from [fops].[dbo].[person]").
                    Append("WHERE").
                    Append(where);
            Console.WriteLine("Executing your query...");
            using (SqlConnection connection = new SqlConnection(connectionString))

            {           
                string sqlQuery = sb.ToString();
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    #region do not understand why below code returns System.InvalidOperationException: Invalid attempt to read when no data is present.
                    //        if (reader.HasRows == true)
                    //        {
                    //            Object[] values = new Object[reader.FieldCount];
                    //            int fieldCount = reader.GetValues(values);
                    //            for (int i = 0; i < fieldCount; i++)
                    //            {
                    //                string res = String.Join("|",values);
                    //        Logger.Logging(@"g:\works\c#\trash\sqlselectlog.log", res, LogStatus.Info);
                    //    }
                    //        }
                    //        else
                    //        { Logger.Logging(@"g:\works\c#\trash\sqlselectlog.log", "no results", LogStatus.Info); }
                    #endregion

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string nameFirst = reader.GetString(1);
                        string surname = reader.GetString(2);
                        string father = reader.GetString(3);
                        string type = reader.GetString(4);
                        string status = reader.GetString(5);
                        int index = reader.GetInt32(6);
                        string region = reader.GetString(7);
                        string adressString = reader.GetString(8);
                        int initialId = reader.GetInt32(9);
                        StringBuilder sbs = new StringBuilder();
                        sbs.Append(nameFirst).Append("\t").
                        Append(surname).Append("\t").
                        Append(father).Append("\t").
                        Append(type).Append("\t").
                        Append(index).Append("\t").
                        Append(status).Append("\t").
                        Append(region).Append("\t").
                        Append(adressString).Append("\t").
                        Append(initialId).Append("\t");
                        Logger.Logging(sbs.ToString());
                    }

                    reader.Close();
                    command.Dispose();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Completed +\n PLease find your query result in G:\\Works\\C#\\trash\\log.log . " +
                        "in case of errors, it will be recorded to sqlErrorlog in the same folder");
                    Console.ReadKey();

                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Sorry, we got some errors or warining. \nPLease find details in the log in \nG:\\Works\\C#\\trash\\sqlErrorlog.log");

                    if (ex is SqlException)
                    { Logger.Logging(@"G:\Works\C#\trash\sqlErrorlog.log", ex.ToString(), LogStatus.Warn); }
                    else
                    { Logger.Logging(@"G:\Works\C#\trash\sqlErrorlog.log", ex.ToString(), LogStatus.Error); }
                    Console.ReadKey();
                }

            }
        }

        internal static void Select(String FullSelect, int x)
        {

            sb.Clear();
            sb.Append(FullSelect);
            Console.WriteLine("Executing your query...");
            using (SqlConnection connection = new SqlConnection(connectionString))

            {

                string sqlQuery = sb.ToString();

                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    #region do not understand why below code returns System.InvalidOperationException: Invalid attempt to read when no data is present.
                    //        if (reader.HasRows == true)
                    //        {
                    //            Object[] values = new Object[reader.FieldCount];
                    //            int fieldCount = reader.GetValues(values);
                    //            for (int i = 0; i < fieldCount; i++)
                    //            {
                    //                string res = String.Join("|",values);
                    //        Logger.Logging(@"g:\works\c#\trash\sqlselectlog.log", res, LogStatus.Info);
                    //    }
                    //        }
                    //        else
                    //        { Logger.Logging(@"g:\works\c#\trash\sqlselectlog.log", "no results", LogStatus.Info); }
                    #endregion

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string nameFirst = reader.GetString(1);
                        string surname = reader.GetString(2);
                        string father = reader.GetString(3);
                        string type = reader.GetString(4);
                        string status = reader.GetString(5);
                        int index = reader.GetInt32(6);
                        string region = reader.GetString(7);
                        string adressString = reader.GetString(8);
                        int initialId = reader.GetInt32(9);
                        StringBuilder sbs = new StringBuilder();
                        sbs.Append(nameFirst).Append("\t").
                        Append(surname).Append("\t").
                        Append(father).Append("\t").
                        Append(type).Append("\t").
                        Append(index).Append("\t").
                        Append(status).Append("\t").
                        Append(region).Append("\t").
                        Append(adressString).Append("\t").
                        Append(initialId).Append("\t");
                        Logger.Logging(sbs.ToString());
                    }

                    reader.Close();
                    command.Dispose();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Completed +\n PLease find your query result in G:\\Works\\C#\\trash\\log.log . " +
                        "in case of errors, it will be recorded to sqlErrorlog in the same folder");
                    Console.ReadKey();

                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Sorry, we got some errors or warining. \nPLease find details in the log in \nG:\\Works\\C#\\trash\\sqlErrorlog.log");

                    if (ex is SqlException)
                    { Logger.Logging(@"G:\Works\C#\trash\sqlErrorlog.log", ex.ToString(), LogStatus.Warn); }
                    else
                    { Logger.Logging(@"G:\Works\C#\trash\sqlErrorlog.log", ex.ToString(), LogStatus.Error); }
                    Console.ReadKey();
                }

            }
        }



        internal static void ParseFromPersonToSQL(string nameFirst, string surname, string father, string type, string status,
                                                  int index, string region, string adressString, int initialId)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlQuery = "INSERT INTO person (nameFirst, surname, father, type, status, [index], region, adressString, [initialId])"
                            + "VALUES (@nameFirst, @surname, @father, @type, @status, @index, @region, @adressString, @initialId)";

                //        string selectLastIdsqlQuery = "SELECT max (id) from person";

                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@nameFirst", nameFirst);
                    command.Parameters.AddWithValue("@surname", surname);
                    command.Parameters.AddWithValue("@father", father);
                    command.Parameters.AddWithValue("@type", type);
                    command.Parameters.AddWithValue("@status", status);
                    command.Parameters.AddWithValue("@index", index);
                    command.Parameters.AddWithValue("@region", region);
                    command.Parameters.AddWithValue("@adressString", adressString);
                    command.Parameters.AddWithValue("@initialId", initialId);

                    int rowAffected = command.ExecuteNonQuery();
                    //     string res = ("ID recorded: " + initialId); // it works, but looks as excessive
                    //     Logger.Logging("G:\\Works\\C#\\trash\\sqlTransActlog.log", res, LogStatus.Info);

                    command.Parameters.Clear();
                    command.Dispose();
                    command = null;
                }


                catch (Exception ex) when (ex is SqlException)
                {
                    Logger.Logging(@"G:\Works\C#\trash\sqlErrorlog.log", (" \n initialId affected: " + initialId + "\n" + ex.ToString()), LogStatus.Warn);
                }
                catch (Exception ex)
                {
                    Logger.Logging(@"G:\Works\C#\trash\sqlErrorlog.log", (ex.ToString() + " \n initialId: " + initialId), LogStatus.Error);
                }
            }
        }


        internal static void ParseFromFIleToSQL(String pathFrom)
        {
            StreamReader sr = new StreamReader(pathFrom, Encoding.GetEncoding(1251));
            string line1 = sr.ReadLine();
            string line = sr.ReadLine();
            Console.WriteLine("Parsing from {0} to SQL db (new itmes are adding to exist db)", pathFrom);
            for (int x = 0; line != null; x++)
            {
                //parsing Person
                //VN: line.Replace("\"", string.Empty);    // Why does not remove "\"" by Replace here? Trim also does not work Trim(char[]={'"'}). But it works for arrays (spllitted line).
                string[] columns = line.Split(';');
                int id; int.TryParse(columns[0].Replace("\"", string.Empty), out id); //@"\" - alternate of"\""
                string nameFull = columns[1].Replace("\"", string.Empty);
                string adressFull = columns[2].Replace("\"", string.Empty);
                string type = columns[3].Replace("\"", string.Empty);
                string status = columns[4].Replace("\"", string.Empty);
                // Adress subparsing
                char[] adressSplitters = new char[] { ',' };
                string[] adressCollumns = adressFull.Split(adressSplitters, 3, StringSplitOptions.RemoveEmptyEntries);
                int index = 0;
                string region = "";
                string adressString = "";
                if (adressCollumns.Length > 0)
                {
                    int.TryParse(adressCollumns[0], out index);
                    if (adressCollumns.Length > 1)
                    { region = adressCollumns[1]; }
                    if (adressCollumns.Length == 3)
                    { adressString = adressCollumns[2]; }
                }
                // Name subparsing
                string[] nameSplitters = new string[] { " " };
                string[] nameCollumns = nameFull.Split(nameSplitters, StringSplitOptions.RemoveEmptyEntries);
                string surname = nameCollumns[0]; string nameFirst = ""; string father = "";
                if (nameCollumns.Length > 1)
                { nameFirst = nameCollumns[1]; }
                if (nameCollumns.Length == 3)
                { father = nameCollumns[2]; }
                // creating objects
                Adress adress = new Adress(index, region, adressString);
                Name name = new Name(nameFirst, surname, father);
                Person person = new Person(id, name, type, status, adress);
                SQL.ParseFromPersonToSQL(nameFirst, surname, father, type, status, index, region, adressString, id);
                line = sr.ReadLine();
                //   Helper.PersonToConsole(person, sr, line, columns, nameCollumns, adressCollumns);
            }
            sr.Close();
        }
    }
}


//SQL create table script
// USE[fops]
//        GO

//--Object:  Table [dbo].[person]    Script Date: 09.07.2016 20:18:49 
//SET ANSI_NULLS ON
//GO

//SET QUOTED_IDENTIFIER ON
//GO

//CREATE TABLE[dbo].[person](
//	[id]
//        [int] IDENTITY(1,1) NOT NULL,

//   [nameFirst] [text]
//        NULL,
//	[surname]
//        [text]
//        NULL,
//	[father]
//        [text]
//        NULL,
//	[type]
//        [text]
//        NULL,
//	[status]
//        [text]
//        NULL,
//	[index]
//        [int] NULL,
//	[region]
//        [text]
//        NULL,
//	[adressString]
//        [text]
//        NULL,
//	[initialId]
//        [int] NULL,
// CONSTRAINT[PK_person] PRIMARY KEY CLUSTERED
//(
//   [id] ASC
//)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
//) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]

//GO