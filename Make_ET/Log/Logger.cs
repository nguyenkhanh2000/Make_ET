using Make_ET.DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Make_ET.Log
{
    public static class Logger
    {
        private static readonly string logFilePath;
        static Logger()
        {
            logFilePath = CConfig.LOG_FILE_PATH;
        }
        private static bool LogFileExists()
        {
            return File.Exists(logFilePath);
        }
        private static void CreateLogFile()
        {
            try
            {
                string logDirectory = Path.GetDirectoryName(logFilePath);
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }
                using (var fileStream = File.Create(logFilePath))
                {
                    
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public static void Log(string logLevel, string message)
        {
            try
            {
                if(!LogFileExists())
                {
                    CreateLogFile();
                }
                using(var streamWriter = new StreamWriter(logFilePath, true))
                {
                    var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {logLevel} {message}";
                    streamWriter.WriteLine(logEntry);
                }
            }catch (Exception ex)
            {               
                //Console.WriteLine($"Error occurred while logging: {ex.Message}");
                Logger.LogError("An error occurred: " + ex.Message);
            }
        }
        public static void LogInfo(string message)
        {
            Log("[INFO]", message);
        }
        public static void LogError(string message)
        {
            Log("[ERROR]", message);
        }
    }
}
