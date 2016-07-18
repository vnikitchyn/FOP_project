using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FOPcsvToSQLApp
{



    public class ParseMain
    {

        static void Main(string[] args)
        {
            Wellcome();
            string input = Console.ReadLine();
            Switch(input);
        }

        static void Switch(string input)
        {
            Console.InputEncoding = Encoding.GetEncoding("windows-1251");
            switch (input.Replace(" ", String.Empty).ToLower())
            {

                case "namesearch":
                    Console.WriteLine("You chose sql name search query method \n NOTE: if  you do not need search by name, surname or father, you can skip this point by enter empty value \nPlease enter First Name you need to Select in Ukrainian...");
                    string name = Console.ReadLine();
                    Console.WriteLine("Please enter Surname you need to Select in Ukrainian...");
                    string surname = Console.ReadLine();
                    Console.WriteLine("Please enter father you need to Select in Ukrainian...");
                    string father = Console.ReadLine();
                    SQL.Find(name,surname,father);
                    break;
                case "simpleselect":
                    Console.WriteLine("You chose sql simple select query method \nPlease enter what you need to Select. examples: TOP 2 * , count (id)...");
                    input = Console.ReadLine();
                    SQL.Select(input);
                    break;
                // TOP 2 * , select count (id), ... possible fields: [id],[nameFirst],[surname],[father],[type],[status],[index],[region],[adressString],[initialId]
                case "whereselect":
                    Console.WriteLine("You chose sql where select query method \nPlease enter what you need to Select. examples: TOP 2 * , count (id)...");
                    input = Console.ReadLine();
                    Console.WriteLine("Now please enter where conditions you need to Select. examples: status not like 'зареєстровано' and surname like 'Козак'...");
                    string where = Console.ReadLine();
                    SQL.Select(input, where);
                    break;
                case "query":
                    Console.WriteLine("You chose sql full query method \nPlease enter your sql query fully \npossible fields: [id],[nameFirst],[surname],[father],[type],[status],[index],[region],[adressString],[initialId])...");
                    input = Console.ReadLine();
                    SQL.Select(input, 1);
                    break;
                case "parse":
                    SQL.ParseFromFIleToSQL(@"G:\Works\C#\trash\fop1.csv");
                    break;
                case "exit":
                    Console.WriteLine("You exited from the app");
                    Console.ReadKey();
                    break;

                default:
                    Console.WriteLine("It looks as mistake, please re-enter the keyword");
                    input = Console.ReadLine();
                    Switch(input);
                    break;
            }
        }
        static void Wellcome()
        {
            Console.WindowWidth = Console.LargestWindowWidth;
            Console.WriteLine("This app can parse FOP csv extras to SQL db, select some quries from that db.\n"
        + "Some other functions are hidden within the code If needed, please see Hepler class.\n\n"
        + "Possible options are:\n\n"
        + "namesearch - you can find rows by name, surname or fathername [best point by default]\n\n"
        + "simpleselect - you can use TOP 2 *, count (id), and similar simple statements to get needed data \n\n"
        + "whereselect -  you can use TOP 2 *, count (id) and similar simple statements to get needed data," 
        +"\nbut also you can use where conditions (examples: p.status not like 'зареєстровано' and p.surname like 'Козак' \n\n"
        + "query - you can enter FULL MS SQL query to run, please be careful\n\n"
        + "parse - parsing data"
          );
        } 
  
    }
}
