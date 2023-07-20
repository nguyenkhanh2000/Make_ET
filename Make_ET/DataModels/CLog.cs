using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Make_ET.DataModels
{
    public class CLog
    {
        public void CLogErrors(Exception ex)
        {
            string logFilePath = "error.log";
            using (StreamWriter writer = new StreamWriter(logFilePath,true)) 
            {
                writer.WriteLine($"Date/Time: {DateTime.Now}");
                writer.WriteLine($"Message: {ex.Message}");
                writer.WriteLine($"Stack Trace: {ex.StackTrace}");
                writer.WriteLine(new string('-', 50));
            }
        }
    }
}
