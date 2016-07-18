using System;
using System.IO;
using System.Text;

namespace FOPcsvToSQLApp
{
    static partial class Logger
    {

        static readonly int sizeLimit = 5242880;
        static int count = 1;
        static int countForError = 1;

        internal static void Logging(string pathTo, string errorMessage, LogStatus eventLog)
        {
            
            try
            {
                FileStream logFile = new FileStream(pathTo, FileMode.Append, FileAccess.Write);
                if (logFile.Length < sizeLimit)
                {
                    StreamWriter sw = new StreamWriter(logFile, Encoding.Unicode);
                    sw.Write("{0}: \r\n  {1}" + System.Environment.NewLine, eventLog, errorMessage);
                    sw.Close();
                    logFile.Close();
                }
                else
                { if (pathTo.ToLower().Contains("error")) {
                        countForError++;
                        logFile.Close();
                        string pathTo2 = (pathTo + countForError + ".log");
                        File.Move(pathTo, pathTo2);
                    }
                      else {
                        count++;
                        logFile.Close();
                        string pathTo2 = (pathTo + count + ".log");
                        File.Move(pathTo, pathTo2);
                    }
                    FileStream logFileNew = new FileStream(pathTo, FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(logFileNew, Encoding.Unicode);
                    sw.Write("{0}\r\n " + System.Environment.NewLine, eventLog, errorMessage);
                    sw.Close();
                    logFileNew.Close();
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.ToString());
            }
        }
        internal static void Logging(string errorMessage, LogStatus eventLog) // 
        {   Logging(@"G:\Works\C#\trash\errorLog.log", errorMessage, eventLog);        }
        internal static void Logging(string whatToRecord) //for data extratcts to file
        {   Logging(@"G:\Works\C#\trash\Log.log", whatToRecord, LogStatus.Info);        }

    }

    enum LogStatus
    { Info, Debug, Warn, Error, Fatal }
}