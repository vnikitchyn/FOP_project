using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FOPcsvToSQLApp
{

    static class Helper
    {
        static List<Person> personList = new List<Person>();

        internal static void WriteFromFileToFile(string pathFrom, string pathTo)
        {
            FileStream recordFile = new FileStream(pathTo, FileMode.Create);
            StreamReader sr = new StreamReader(pathFrom, Encoding.GetEncoding(1251));
            StreamWriter sw = new StreamWriter(recordFile, Encoding.GetEncoding(1251));

            for (int x = 0; x < 11; x++)
            {
                string line = sr.ReadLine();
                sw.WriteLine(line);
                Console.WriteLine(line);
            }
            Console.ReadKey();
            sr.Close();
            sw.Close();
        }

        internal static void WriteStreamToFile(StreamReader sr, string pathTo)
        {
            FileStream recordFile = new FileStream(pathTo, FileMode.Create);
            StreamWriter sw = new StreamWriter(recordFile, Encoding.GetEncoding(1251));
            Console.WriteLine("Please enter how many lines you need to parse from buffered stream into {0}", pathTo);
            string input = Console.ReadLine();
            int count = 1;
            int.TryParse(input, out count);

            for (int x = 0; x < count; x++)
            {
                string line = sr.ReadLine();
                sw.WriteLine(line);
                Console.WriteLine(line);
            }
            Console.WriteLine("Parsing to file is done. Press any key to continue");
            Console.ReadKey();
            sw.Close();
        }

        internal static void PersonToConsole(Person person, StreamReader sr, string line, string[] columns, string[] nameCollumns, string[] adressCollumns)
        {
            Console.WriteLine(person.ToString());
            line = sr.ReadLine();
            Console.ReadKey();
            Console.WriteLine("columns of Person:");
            Console.WriteLine(String.Join("||", columns));
            Console.WriteLine("columns of name:");
            Console.WriteLine(String.Join("||", nameCollumns));
            Console.WriteLine("columns of adress:");
            Console.WriteLine(String.Join("||", adressCollumns));
            Console.ReadKey();
        }

        internal static void ParseToConsole(string path)
        {
            StreamReader sr = new StreamReader(path, Encoding.GetEncoding(1251));
            string line = sr.ReadLine();
            for (int x = 0; line != null; x++)
            {

                if (x == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(line);
                    line = sr.ReadLine();
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine(line);
                    line = sr.ReadLine();
                    if (x % 25 == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(line);
                        line = sr.ReadLine();
                        Console.ResetColor();
                        Console.ReadKey();
                    }
                }
            }

            sr.Close();
        }


        internal static void ParseToPersonList(String pathFrom)
        {
            StreamReader sr = new StreamReader(pathFrom, Encoding.GetEncoding(1251));
            string line1 = sr.ReadLine();
            string line = sr.ReadLine();

            for (int x = 0; line != null; x++)
            {
                //parsing Person
                //VN: line.Replace("\"", string.Empty);    // Why does not remove "\"" by Replace here? Trim also does not work Trim(char[]={'"'}). But it works for arrays (spllitted line).
                string[] columns = line.Split(';');
                int id; int.TryParse(columns[0].Replace(@"\", string.Empty), out id); //@"\" - alternate of"\""
                string nameFull = columns[1].Replace(@"\", string.Empty);
                string adressFull = columns[2].Replace("\"", string.Empty);
                string type = columns[3].Replace("\"", string.Empty);
                string status = columns[4].Replace("\"", string.Empty);
                // Adress subparsing
                char[] adressSplitters = new char[] { ',' };
                string[] adressCollumns = adressFull.Split(adressSplitters, 3, StringSplitOptions.RemoveEmptyEntries);
                int index; int.TryParse(adressCollumns[0], out index); string region = null; string adressString = null;
                if (adressCollumns.Length > 1)
                { region = adressCollumns[1]; }
                if (adressCollumns.Length == 3)
                { adressString = adressCollumns[2]; }

                // Name subparsing
                string[] nameSplitters = new string[] { " " };
                string[] nameCollumns = nameFull.Split(nameSplitters, StringSplitOptions.RemoveEmptyEntries);
                string surname = nameCollumns[0]; string nameFirst = null; string father = null;
                if (nameCollumns.Length > 1)
                { nameFirst = nameCollumns[1]; }
                if (nameCollumns.Length == 3)
                { father = nameCollumns[2]; }
                // creating objects
                Adress adress = new Adress(index, region, adressString);
                Name name = new Name(nameFirst, surname, father);
                Person person = new Person(id, name, type, status, adress);
                personList.Add(person);

                   //Helper.PersonToConsole(person, sr, line,columns,nameCollumns,adressCollumns);
            }

            sr.Close();
        }
    }

}