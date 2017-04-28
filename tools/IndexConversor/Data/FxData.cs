using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexConversor.Data
{
    class FxData
    {
        public int Animation;
        public int OffsetX, OffsetY;

        public static void SaveFxData(FxData[] fxData, string path)
        {
            int size = fxData.Length;
            using (BinaryWriter bw = new BinaryWriter(new FileStream(path, FileMode.Create)))
            {
                bw.Write(new byte[263]);

                bw.Write((short)size);

                foreach (var f in fxData)
                {
                    bw.Write((short)f.Animation);
                    bw.Write((short)f.OffsetX);
                    bw.Write((short)f.OffsetY);
                }
            }
        }
        public static FxData[] LoadFxData(string path)
        {
            FxData[] fxData;

            using(BinaryReader br = new BinaryReader(new FileStream(path, FileMode.Open)))
            {
                br.ReadBytes(263);

                int size = br.ReadInt16();

                fxData = new FxData[size];

                for (int i = 0; i < size; i++)
                {
                    FxData f = new FxData();

                    f.Animation = br.ReadInt16();
                    f.OffsetX = br.ReadInt16();
                    f.OffsetY = br.ReadInt16();

                    fxData[i] = f;
                }

                return fxData;
            }
        }
    }
}
