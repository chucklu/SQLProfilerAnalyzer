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
                Console.WriteLine("End of the program.");
                Console.ReadLine();
            }
        }

        static void Run()
        {
            string folder = @"C:\workspace\";

            var baseFolder= Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
           var outputFolder = baseFolder.Replace(@"file:\", string.Empty);

           //WriteCorrect(sourceFolder, outputFolder);

           WriteWrong(sourceFolder, outputFolder);
        }

        static void WriteWrong(string sourceFolder, string outputFolder)
        {
            var wrongTraceFile = "wrong trace0518-002.xml";
            var wrongTracePath = Path.Combine(sourceFolder, wrongTraceFile);

            TraceXmlParser parser = new TraceXmlParser(wrongTracePath);
            var dic2 = parser.Read();
            Write(Path.Combine(outputFolder, "WrongTrace.txt"), dic2);
        }

        static void WriteCorrect(string sourceFolder, string outputFolder)
        {
            var correctTraceFile = "correct trace.xml";
            var correctTracePath = Path.Combine(sourceFolder, correctTraceFile);

            TraceXmlParser parser = new TraceXmlParser(correctTracePath);
            var dic1 = parser.Read();
            Write(Path.Combine(outputFolder, "CorrectTrace.txt"), dic1);
        }

        static void Write(string path, Dictionary<int, CustomEvent> dic)
        {
            foreach (var item in dic)
            {
                FileHelper.Write(path,$"{item.Key:D5}  {item.Value.TextData}");
            }
        }
    }
}
