using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Make_ET
{
    public static class App_Start
    {
        private static DateTime currentTime = DateTime.Now;
        private static DateTime targetTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 10, 14, 0);
        private static TimeSpan timeDifference = targetTime - currentTime;
        public static void Start_App()
        {
            if (timeDifference.TotalMilliseconds > 0)
            {
                DateTime timeStart = DateTime.Now;
                Console.WriteLine("Start App: {0}", timeStart);
                //Console.WriteLine("Press any key to exit...");
                //Console.ReadKey();
                //DateTime timeEnd = DateTime.Now;
                //Console.WriteLine("End App: {0}", timeEnd);

                //TimeSpan appDuration = timeEnd - timeStart;
                //Console.WriteLine("App Duration: {0}", appDuration);

                //Console.WriteLine("Press any key to exit...");
                //Console.ReadKey();

                Thread.Sleep((int)timeDifference.Milliseconds);
            }                       
        }
        
    }
}
