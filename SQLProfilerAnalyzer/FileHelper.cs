using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLProfilerAnalyzer
{
    public class FileHelper
    {
        public static void Write(string path,string text)
        {
            using (StreamWriter file = new StreamWriter(path, append: true))
            {
                file.WriteLine(text);
            }
        }
    }
}
