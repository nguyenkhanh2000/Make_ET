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
        private static DateTime targetTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 15, 0, 0);
        private static TimeSpan timeDifference = targetTime - currentTime;
        public static void Start_App()
        {
            if (timeDifference.TotalMilliseconds > 0)
            {
                Thread.Sleep(timeDifference);
            }                       
        }
        
    }
}
