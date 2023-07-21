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
using static Make_ET.DataModels.CGlobal;

namespace Make_ET
{
    class Program
    {
        static async Task Main(string[] args)
        {
            App_Start.Start_App();
            DataModels.ReadFile reader = new DataModels.ReadFile();
            //reader.Thread_QUOTE_SECURITY();
            await reader.Thread_QUOTE_MARKET_STATAsync();
            await reader.Thread_QUOTE_SECURITYAsync();
            await reader.Thread_QUOTE_LSAsync();
            await reader.Thread_QUOTE_OSAsync();
            await reader.Thread_QUOTE_FROOMAsync();

            await reader.Thread_QUOTE_PUT_ADAsync();
            await reader.Thread_QUOTE_PUT_EXECAsync();
            await reader.Thread_QUOTE_PUT_DCAsync();

            await reader.Thread_QUOTE_LOAsync();

            await reader.Thread_QUOTE_SECURITYOLAsync();
            await reader.Thread_QUOTE_LEAsync();

            await reader.Thread_VNX_INAVAsync();
            await reader.Thread_VNX_IINDEXAsync();
            await reader.Thread_VNX_VN30Async();
            await reader.Thread_VNX_VNINDEXAsync();
            await reader.Thread_VNX_VN100Async();
            await reader.Thread_VNX_VNALLAsync();
            await reader.Thread_VNX_VNMIDAsync();
            await reader.Thread_VNX_VNSMLAsync();
            await reader.Thread_VNX_VNXALLAsync();

            reader.Read_LAST_INDEX_HO();
            reader.Thread_FULL_ROW_QUOTE();
            reader.Redis_S5G_ET_QUOTE();
            reader.Redis_S5G_ET_PT();
            reader.Redis_S5G__ET_INDEX();
            reader.SaveData();

            //saveData saveDataOracle = new saveData();
            //saveDataOracle.Save();
        }           
    }
}

