using Make_ET.DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Make_ET
{
    public class FileReader
    {        
        public static void ReadData(string filePath)
        {
            //using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            //{
            //    using (BinaryReader reader = new BinaryReader(fileStream))
            //    {
            //        while (fileStream.Position < fileStream.Length)
            //        {
            //            int length = reader.ReadInt32();
            //            byte[] stringBytes = reader.ReadBytes(length);
            //            string data = Encoding.UTF8.GetBytes(stringBytes);

            //            Console.WriteLine(data);
            //            Thread.Sleep(2000);
            //        }
            //    }
            //}
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                using (BinaryReader reader = new BinaryReader(fileStream))
                {
                    while (fileStream.Position < fileStream.Length)
                    {
                        int length = reader.ReadInt32();
                        byte[] stringBytes = reader.ReadBytes(length);
                        foreach(byte b in stringBytes)
                        {
                            Console.WriteLine($"{b:X2}");
                            
                        }
                        Thread.Sleep(500);
                    }
                }
            }

            //byte[] bHex = File.ReadAllBytes(filePath);

            //StringBuilder st = new StringBuilder();
            //int i = 0;

            //foreach (char c in bHex.Reverse())
            //{
            //    i++;
            //    if (i > 12 && i < 21)
            //    {
            //        st.Append(Convert.ToInt32(c).ToString("X2"));
            //    }           
            //}
            //long Output = Convert.ToInt64(st.ToString(), 16);

            //////Convert ticks to date time
            //DateTime dt = new DateTime(Output);

            //////Write the output date to console
            //Console.Write(Output);


        }
    }
}
