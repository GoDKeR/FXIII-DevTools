using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexConversor.Data
{
    class HeadData
    {
        public int[] Walk;

        public static void SaveHeadData(HeadData[] headData, string path)
        {
            int size = headData.Length;
            using (BinaryWriter bw = new BinaryWriter(new FileStream(path, FileMode.Create)))
            {
                bw.Write(new byte[263]);

                bw.Write((short)size);

                foreach (var h in headData)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        bw.Write((short)h.Walk[i]);
                    }
                }
            }
        }
        public static HeadData[] LoadHeadData(string path)
        {
            HeadData[] headData;

            using (BinaryReader br = new BinaryReader(new FileStream(path, FileMode.Open)))
            {
                //Remove header file
                br.ReadBytes(263);

                int size = br.ReadInt16();

                headData = new HeadData[size];

                for (int i = 0; i < size; i++)
                {
                    HeadData h = new HeadData();

                    h.Walk = new int[4];

                    for (int j = 0; j < 4; j++)
                    {
                        h.Walk[j] = br.ReadInt16();
                    }
                    headData[i] = h;
                }

                return headData;
            }
        }
    }
}
