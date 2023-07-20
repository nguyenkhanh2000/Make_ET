using Make_ET.DataModels;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Make_ET.DataModels.CGlobal;

namespace Make_ET.Redis
{
    public class S5G_ET_QUOTE
    {
        
        private static string setkey = "S5G_ET_QUOTE";
        public void FULL_ROW_QUOTE()
        {
            Connection.ConnectionRedis();
            IDatabase db = Connection.GetRedisDatabase();
            
            int startScore = 1;
            double dblScore = Convert.ToDouble(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
           


            string jsonData = @"
        [
            {
                ""Co"": ""TTE"",
                ""Re"": ""1385"",
                ""Ce"": ""1480"",
                ""Fl"": ""1290"",
                ""BQ4"": ""0"",
                ""BP3"": ""0"",
                ""BQ3"": ""0"",
                ""BP2"": ""0"",
                ""BQ2"": ""0"",
                ""BP1"": ""0"",
                ""BQ1"": ""0"",
                ""MP"": ""0"",
                ""MQ"": ""0"",
                ""MC"": ""0"",
                ""SP1"": ""0"",
                ""SQ1"": ""0"",
                ""SP2"": ""0"",
                ""SQ2"": ""0"",
                ""SP3"": ""0"",
                ""SQ3"": ""0"",
                ""SQ4"": ""0"",
                ""TQ"": ""0"",
                ""Op"": ""0"",
                ""Hi"": ""0"",
                ""Lo"": ""0"",
                ""Av"": ""0"",
                ""FB"": ""0"",
                ""FS"": ""0"",
                ""FR"": ""14529584"",
                ""SN"": ""61"",
                ""ST"": ""ST"",
                ""PO"": ""0"",
                ""Ri"": """",
                ""MPO"": ""0"",
                ""MQO"": ""0"",
                ""TQO"": ""0""
            },
            {
                ""Co"": ""FIT"",
                ""Re"": ""500"",
                ""Ce"": ""535"",
                ""Fl"": ""465"",
                ""BQ4"": ""0"",
                ""BP3"": ""0"",
                ""BQ3"": ""0"",
                ""BP2"": ""0"",
                ""BQ2"": ""0"",
                ""BP1"": ""0"",
                ""BQ1"": ""0"",
                ""MP"": ""0"",
                ""MQ"": ""0"",
                ""MC"": ""0"",
                ""SP1"": ""0"",
                ""SQ1"": ""0"",
                ""SP2"": ""0"",
                ""SQ2"": ""0"",
                ""SP3"": ""0"",
                ""SQ3"": ""0"",
                ""SQ4"": ""0"",
                ""TQ"": ""0"",
                ""Op"": ""0"",
                ""Hi"": ""0"",
                ""Lo"": ""0"",
                ""Av"": ""0"",
                ""FB"": ""0"",
                ""FS"": ""0"",
                ""FR"": ""120917359"",
                ""SN"": ""981"",
                ""ST"": ""ST"",
                ""PO"": ""0"",
                ""Ri"": """",
                ""MPO"": ""0"",
                ""MQO"": ""0"",
                ""TQO"": ""0""
            },
            {
                ""Co"": ""CVNM2002"",
                ""Re"": ""910"",
                ""Ce"": ""1170"",
                ""Fl"": ""650"",
                ""BQ4"": ""0"",
                ""BP3"": ""0"",
                ""BQ3"": ""0"",
                ""BP2"": ""0"",
                ""BQ2"": ""0"",
                ""BP1"": ""0"",
                ""BQ1"": ""0"",
                ""MP"": ""0"",
                ""MQ"": ""0"",
                ""MC"": ""0"",
                ""SP1"": ""0"",
                ""SQ1"": ""0"",
                ""SP2"": ""0"",
                ""SQ2"": ""0"",
                ""SP3"": ""0"",
                ""SQ3"": ""0"",
                ""SQ4"": ""0"",
                ""TQ"": ""0"",
                ""Op"": ""0"",
                ""Hi"": ""0"",
                ""Lo"": ""0"",
                ""Av"": ""0"",
                ""FB"": ""0"",
                ""FS"": ""0"",
                ""FR"": ""213000"",
                ""SN"": ""185"",
                ""ST"": ""EW"",
                ""PO"": ""0"",
                ""Ri"": """",
                ""MPO"": ""0"",
                ""MQO"": ""0"",
                ""TQO"": ""0""
            }
        ]";

            List<FULL_ROW_QUOTE> entries = JsonConvert.DeserializeObject<List<FULL_ROW_QUOTE>>(jsonData);
            foreach(var entry in entries) 
            {
                string serializeEntry = JsonConvert.SerializeObject(entry);
                //string score = entry.FR;
                //double scores = Double.Parse(score);

                db.SortedSetAdd(setkey, serializeEntry, dblScore);
            }

            // Deserialize the JSON data into an array of FULL_ROW_QUOTE objects
            //FULL_ROW_QUOTE[] quotes = JsonConvert.DeserializeObject<FULL_ROW_QUOTE[]>(jsonData);

            //for(int i = 0; i < quotes.Length; i++)
            //{
            //    string quoteJson = JsonConvert.SerializeObject(quotes[i]);
            //    int score = startScore + i;
            //    db.SortedSetAdd(key,quoteJson,score);
            //}
        }
        
    }
}
