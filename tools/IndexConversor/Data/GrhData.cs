using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GrhData
{
    public int Grh { get; set; }
    public short sX { get; set; }
    public short sY { get; set; }
    public short pixelWidth { get; set; }
    public short pixelHeight { get; set; }
    public short numFrames { get; set; }
    public float speed { get; set; }
    public int fileNum { get; set; }
    public float tileWidth { get; set; }
    public float tileHeight { get; set; }

    public short[] frames { get; set; }

    public GrhData()
    {
        Grh = 0;
        sX = 0;
        sY = 0;
        pixelWidth = 0;
        pixelHeight = 0;
        numFrames = 0;
        speed = 0;
        fileNum = 0;
        tileWidth = 0.0f;
        tileHeight = 0.0f;
    }

    public static GrhData[] loadGrhDataFenix(string path, out int lastGrh)
    {
        short grh = 0;
        int tmpSpeed = 0;

        //set enough space
        GrhData[] grhData = new GrhData[32000];

        if (File.Exists(path))
        {
            using (BinaryReader br = new BinaryReader(new FileStream(path, FileMode.Open)))
            {
                //take the useless header
                br.ReadBytes(263);
                br.ReadInt64();
                br.ReadInt16();

                while (br.BaseStream.Position != br.BaseStream.Length)
                {
                    grh = br.ReadInt16();

                    GrhData g = new GrhData();
                    grh--;

                    g.numFrames = br.ReadInt16();

                    g.frames = new short[g.numFrames];

                    if (g.numFrames > 1)
                    {
                        for (int i = 0; i < g.numFrames; i++)
                        {
                            g.frames[i] = br.ReadInt16();
                        }
                        tmpSpeed = br.ReadInt16();
                        g.speed = (float)tmpSpeed * (float)g.numFrames * 1000.0f / 18.0f;
                        g.pixelHeight = grhData[g.frames[0]].pixelHeight;
                        g.pixelWidth = grhData[g.frames[0]].pixelWidth;

                        g.tileHeight = grhData[g.frames[0]].tileHeight;
                        g.tileWidth = grhData[g.frames[0]].tileWidth;
                    }
                    else
                    {
                        g.fileNum = br.ReadInt16();
                        g.sX = br.ReadInt16();
                        g.sY = br.ReadInt16();
                        g.pixelWidth = br.ReadInt16();
                        g.pixelHeight = br.ReadInt16();

                        g.tileWidth = (float)g.pixelWidth / 32.0f;
                        g.tileHeight = (float)g.pixelHeight / 32.0f;

                        g.frames[0] = (short)(grh + 1);
                    }
                    grhData[grh] = g;
                }
                br.Dispose();
            }
        }
        lastGrh = grh;
        return grhData;
    }

    public static void SaveGrhData130(GrhData[] grhData, string path, int grhs)
    {
        int size = grhs;
        using (BinaryWriter bw = new BinaryWriter(new FileStream(path, FileMode.Create)))
        {
            bw.Write(1513);
            bw.Write(size); //write the last grh

            for (int i = 0; i < size; i++)
            {
                if (grhData[i] == null)
                    continue;

                bw.Write(i + 1);
                bw.Write(grhData[i].numFrames);

                if (grhData[i].numFrames > 1)
                {
                    for (int j = 0; j < grhData[i].numFrames; j++)
                    {
                        bw.Write(grhData[i].frames[j]);
                    }

                    bw.Write((float)grhData[i].speed); //cast to Single to match 13.0 format
                }
                else
                {
                    bw.Write(grhData[i].fileNum);
                    bw.Write(grhData[i].sX);
                    bw.Write(grhData[i].sY);
                    bw.Write(grhData[i].pixelWidth);
                    bw.Write(grhData[i].pixelHeight);
                }
            }
            bw.Dispose();
        }
    }

    public static GrhData[] LoadGrhData(string path)
    {
        GrhData[] grhData;
        int size;
        using (BinaryReader br = new BinaryReader(new FileStream(path, FileMode.Open)))
        {
            br.ReadInt32();
            size = br.ReadInt32();

            grhData = new GrhData[size + 1];

            while(br.BaseStream.Position != br.BaseStream.Length)
            {

                GrhData g = new GrhData();

                g.Grh = br.ReadInt32();
                g.numFrames = br.ReadInt16();

                g.frames = new short[g.numFrames];

                if (g.numFrames > 1)
                {
                    for (int j = 0; j < g.numFrames; j++)
                    {
                        g.frames[j] = br.ReadInt16();
                    }

                    g.speed = br.ReadSingle();
                }
                else
                {
                    g.fileNum = br.ReadInt32();
                    g.sX = br.ReadInt16();
                    g.sY = br.ReadInt16();
                    g.pixelWidth = br.ReadInt16();
                    g.pixelHeight = br.ReadInt16();

                    g.tileWidth = g.pixelWidth / 32;
                    g.tileHeight = g.pixelHeight / 32;

                    g.frames[0] = (short)g.Grh;
                }

                grhData[g.Grh] = g;
            }
            br.Dispose();
        }

        return grhData;
    }
}
