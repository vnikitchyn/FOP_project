using System;

static partial class Logger
{

    static readonly int sizeLimit = 5242880;
    static int count = 1;

    internal static void Logging(string pathTo, string whatToRecord, LogStatus eventLog)
    {
        int countForError = 1;
        FileStream logFile = new FileStream(pathTo, FileMode.Append, FileAccess.Write);
        //VN: Could not create more elegant decision how to split the logfile, but possibly it is OK 
        if (logFile.Length < sizeLimit)
        {
            StreamWriter sw = new StreamWriter(logFile, Encoding.Unicode);
            sw.Write("{0}: \r\n  {1}" + System.Environment.NewLine, eventLog, whatToRecord);
            sw.Close();
            logFile.Close();
        }
        else
        {
            countForError++;
            File.Move(pathTo, (pathTo + countForError + ".log"));
            StreamWriter sw = new StreamWriter(logFile, Encoding.Unicode);
            sw.Write("{0}\r\n " + System.Environment.NewLine, eventLog, whatToRecord);
            sw.Close();
            logFile.Close();
        }
    }
    internal static void Logging(string whatToRecord, LogStatus eventLog) // 
    {
        FileStream logFile = new FileStream(@"G:\Works\C#\trash\log.log", FileMode.Append, FileAccess.Write);

        if (logFile.Length < sizeLimit)
        {
            StreamWriter sw = new StreamWriter(logFile, Encoding.Unicode);
            sw.Write("{0}\r\n " + System.Environment.NewLine, eventLog, whatToRecord);
            sw.Close();
            logFile.Close();
        }
        else
        {
            count++;
            File.Move(@"G:\Works\C#\trash\log.log", (@"G:\Works\C#\trash\log" + count + ".log"));
            StreamWriter sw = new StreamWriter(logFile, Encoding.Unicode);
            sw.Write("{0}\r\n " + System.Environment.NewLine, eventLog, whatToRecord);
            sw.Close();
            logFile.Close();
        }
    }
    internal static void Logging(string whatToRecord) //for data extratcts to file
    {
        FileStream logFile = new FileStream(@"G:\Works\C#\trash\log.log", FileMode.Append, FileAccess.Write);

        if (logFile.Length < sizeLimit)
        {
            StreamWriter sw = new StreamWriter(logFile, Encoding.Unicode);
            sw.Write("{0}\r\n " + System.Environment.NewLine, whatToRecord);
            sw.Close();
            logFile.Close();
        }
        else
        {
            count++;
            File.Move(@"G:\Works\C#\trash\log.log", (@"G:\Works\C#\trash\log" + count + ".log"));
            StreamWriter sw = new StreamWriter(logFile, Encoding.Unicode);
            sw.Write("{0}\r\n " + System.Environment.NewLine, whatToRecord);
            sw.Close();
            logFile.Close();
        }
    }
}

enum LogStatus
{ Info, Debug, Warn, Error, Fatal }