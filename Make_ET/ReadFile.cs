using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Make_ET
{
    public class ReadFile
    {
        public static byte[][] ReadData(string filePath)
        {
            List<byte[]> byteArrays = new List<byte[]>();

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                using (BinaryReader reader = new BinaryReader(fileStream))
                {
                    while (fileStream.Position < fileStream.Length)
                    {
                        int length = reader.ReadInt32();
                        byte[] stringBytes = reader.ReadBytes(length);

                        byteArrays.Add(stringBytes);

                        foreach (byte b in stringBytes)
                        {
                            Console.WriteLine($"{b:X2}");
                        }

                        Thread.Sleep(1000);
                    }
                }
            }
            
            return byteArrays.ToArray();
            
        }
    }
}
