using System;
using System.Collections.Generic;
using System.Linq;
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

        }
    }
}
