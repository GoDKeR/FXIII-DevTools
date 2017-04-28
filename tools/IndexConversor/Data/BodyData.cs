using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexConversor.Data
{
    class BodyData
    {
        public int[] Walk;

        public int HeadOffsetX, HeadOffsetY;

        public static void SaveBodyData(BodyData[] bodyData, string path)
        {
            int size = bodyData.Length;
            using (BinaryWriter bw = new BinaryWriter(new FileStream(path, FileMode.Create)))
            {
                bw.Write(new byte[263]);

                bw.Write((short)size);

                foreach (var b in bodyData)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        bw.Write((short)b.Walk[i]);
                    }

                    bw.Write((short)b.HeadOffsetX);
                    bw.Write((short)b.HeadOffsetY);
                }
            }
        }
        public static BodyData[] LoadBodyData(string path)
        {
            BodyData[] bodyData;

            using (BinaryReader br = new BinaryReader(new FileStream(path, FileMode.Open)))
            {
                //Remove header file
                br.ReadBytes(263);

                int size = br.ReadInt16();

                bodyData = new BodyData[size];

                for (int i = 0; i < size; i++)
                {
                    BodyData b = new BodyData();

                    b.Walk = new int[4];

                    for (int j = 0; j < 4; j++)
                    {
                        b.Walk[j] = br.ReadInt16();
                    }

                    b.HeadOffsetX = br.ReadInt16();
                    b.HeadOffsetY = br.ReadInt16();

                    bodyData[i] = b;
                }

                return bodyData;
            }
        }
    }
}
