using Make_ET;
using Make_ET.DataModels;
using Make_ET.Oracle;
using Make_ET.Redis;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using static Make_ET.DataModels.CGlobal;

namespace Make_ET
{
    public class Program
    {
        static System.Timers.Timer timer;
        static void Main(string[] args)
        {
            Start_App();
            // Keep the program running to allow the Timer to trigger the event
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }           
        static void Start_App()
        {
            timer = new System.Timers.Timer();
            timer.Interval = TimeSpan.FromMinutes(1).TotalMilliseconds;
            timer.Elapsed += CheckAndRunMakeET;
            timer.Start();
            Console.WriteLine("Application started. Waiting 15:00 PM to run MAKE ET");
        }
        static void CheckAndRunMakeET(object sender, ElapsedEventArgs e)
        {
            int targetHour = 14;
            int targetMinute = 36;

            //Get the current time
            DateTime currentTime = DateTime.Now;
            //Check if the current hour and minute match the target hour and minute(15:00)
            if(currentTime.Hour == targetHour && currentTime.Minute == targetMinute)
            {
                timer.Stop();
                App_MakeET();
            }
        }
        static async Task App_MakeET()
        {
            DataModels.ReadFile reader = new DataModels.ReadFile();
            //reader.Thread_QUOTE_SECURITY();
            await reader.Thread_QUOTE_MARKET_STATAsync();
            await reader.Thread_QUOTE_SECURITYAsync();
            await reader.Thread_QUOTE_LSAsync();
            await reader.Thread_QUOTE_OSAsync();
            await reader.Thread_QUOTE_FROOMAsync();

            await reader.Thread_QUOTE_LOAsync();
            await reader.Thread_QUOTE_SECURITYOLAsync();
            await reader.Thread_QUOTE_LEAsync();

            await reader.Thread_QUOTE_PUT_ADAsync();
            await reader.Thread_QUOTE_PUT_EXECAsync();
            await reader.Thread_QUOTE_PUT_DCAsync();

            await reader.Thread_VNX_INAVAsync();
            await reader.Thread_VNX_IINDEXAsync();
            await reader.Thread_VNX_VN30Async();
            await reader.Thread_VNX_VNINDEXAsync();
            await reader.Thread_VNX_VN100Async();
            await reader.Thread_VNX_VNALLAsync();
            await reader.Thread_VNX_VNMIDAsync();
            await reader.Thread_VNX_VNSMLAsync();
            await reader.Thread_VNX_VNXALLAsync();
          
            reader.Update_FULL();
            DateTime timedone = DateTime.Now;
            Console.WriteLine("Done!:{0}",timedone);
        }
    }
}

