using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using DependentChecker.Helper;
using Serilog.Events;

namespace SQLProfilerAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                LogHelper.CreateLog(LogEventLevel.Information, "Start of the program.");
                LogHelper.StartProgram();
                Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                LogHelper.CreateLog(LogEventLevel.Information, ex);
            }
            finally
            {
                LogHelper.CreateLog(LogEventLevel.Information,"End of the program.");
                Console.ReadLine();
            }
        }

        static void Run()
        {
            string folder = @"C:\workspace\";
            var correctTraceFile = "correct trace.xml";
            var wrongTraceFile = "wrong trace.xml";
            var correctTracePath = Path.Combine(folder, correctTraceFile);
            var wrongTracePath = Path.Combine(folder, wrongTraceFile);
            TraceXmlParser parser = new TraceXmlParser();

            var baseFolder= Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            baseFolder = baseFolder.Replace(@"file:\", string.Empty);
            var dic2 = parser.Read(wrongTracePath);
            Write(Path.Combine(baseFolder, "WrongTrace.txt"), dic2);

            var dic1 = parser.Read(correctTracePath);
            Write(Path.Combine(baseFolder, "CorrectTrace.txt"), dic1);
        }

        static void Write(string path, Dictionary<int, string> dic)
        {
            foreach (var item in dic)
            {
                FileHelper.Write(path,$"{item.Key:D5} {item.Value}");
            }
        }
    }
}
