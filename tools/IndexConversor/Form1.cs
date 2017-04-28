using DatConverter;
using IndexConversor.Data;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace IndexConversor
{
    public partial class Form1 : Form
    {
        BodyData[] bodyData;
        GrhData[] grhData;
        HeadData[] headData;
        HeadData[] helmetData;
        FxData[] fxData;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Cuerpos");
            comboBox1.Items.Add("Cabezas");
            comboBox1.Items.Add("Gráficos");
            comboBox1.Items.Add("Cascos");
            comboBox1.Items.Add("Fxs");

            comboBox1.SelectedIndex = 2;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0: LoadBodyData(); break;
                case 1: LoadHeadData(); break;
                case 2: LoadGrhData(); break;
                case 3: LoadHelmetData(); break;
                case 4: LoadFxData(); break;
            }

            string path = String.Empty;

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "FXIII Deindexed Files |*.dat";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    path = sfd.FileName;
                }
            }

            if (path.Length != 0)
            {
                IniManager writer = new IniManager(path, true);
                int i;

                writer.AddMain("INIT");

                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        #region Bodys
                        writer.AddKeyValue("INIT", "NumBodys", bodyData.Length.ToString());

                        i = 1;
                        foreach (BodyData b in bodyData)
                        {
                            writer.AddMain("Body" + i);

                            for (int j = 0; j < 4; j++)
                            {
                                writer.AddKeyValue("Body" + i, "Dir" + (j + 1), b.Walk[j].ToString());

                            }
                            writer.AddKeyValue("Body" + i, "HeadOffsetX", b.HeadOffsetX.ToString());
                            writer.AddKeyValue("Body" + i, "HeadOffsetY", b.HeadOffsetY.ToString());

                            i++;
                        }
                        #endregion
                        break;
                    case 1:
                        #region Heads
                        writer.AddKeyValue("INIT", "NumHeads", headData.Length.ToString());

                        i = 1;

                        foreach (var h in headData)
                        {
                            writer.AddMain("Head" + i);

                            for (int j = 0; j < 4; j++)
                            {
                                writer.AddKeyValue("Head" + i, "Dir" + (j + 1), h.Walk[j].ToString());
                            }
                            i++;
                        }
                        #endregion
                        break;

                    case 2:
                        #region Graphics
                        writer.AddKeyValue("INIT", "NumGrhs", grhData.Length.ToString());

                        writer.AddMain("GRAPHICS");
                        foreach (GrhData g in grhData)
                        {
                            if (g != null)
                            {
                                string m;
                                m = g.numFrames.ToString() + "-";
                                if (g.numFrames > 1)
                                {
                                    for (int j = 0; j < g.numFrames; j++)
                                    {
                                        m += g.frames[j].ToString() + "-";
                                    }

                                    m += g.speed;
                                }
                                else
                                {
                                    m += g.fileNum + "-";
                                    m += g.sX + "-";
                                    m += g.sY + "-";
                                    m += g.pixelWidth + "-";
                                    m += g.pixelHeight;
                                }
                                writer.AddKeyValue("GRAPHICS", "GRH" + g.Grh, m);
                            }
                        }
                        #endregion
                        break;

                    case 3:
                        #region Helmets
                        writer.AddKeyValue("INIT", "NumHelmets", helmetData.Length.ToString());

                        i = 1;

                        foreach (var h in helmetData)
                        {
                            writer.AddMain("Helmet" + i);

                            for (int j = 0; j < 4; j++)
                            {
                                writer.AddKeyValue("Helmet" + i, "Dir" + (j + 1), h.Walk[j].ToString());
                            }

                            i++;
                        }
                        #endregion
                        break;
                    case 4:
                        #region Fxs
                        writer.AddKeyValue("INIT", "NumFxs", fxData.Length.ToString());

                        i = 1;

                        foreach (var f in fxData)
                        {
                            writer.AddMain("Fx" + i);

                            writer.AddKeyValue("Fx" + i, "Animation", f.Animation.ToString());
                            writer.AddKeyValue("Fx" + i, "OffsetX", f.OffsetX.ToString());
                            writer.AddKeyValue("Fx" + i, "OffsetY", f.OffsetY.ToString());

                            i++;
                        }

                        #endregion
                        break;

                }
                writer.DumpFile(path);
            }
            MessageBox.Show("Listo!");
        }

        private void LoadGrhData()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "FXIII Indexed Files |*.ind";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    grhData = GrhData.LoadGrhData(ofd.FileName);
                }
            }
        }

        private void LoadHeadData()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "FXIII Indexed Files |*.ind";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    headData = HeadData.LoadHeadData(ofd.FileName);
                }
            }
        }

        private void LoadHelmetData()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "FXIII Indexed Files |*.ind";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    helmetData = HeadData.LoadHeadData(ofd.FileName);
                }
            }
        }

        private void LoadBodyData()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "FXIII Indexed Files |*.ind";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    bodyData = BodyData.LoadBodyData(ofd.FileName);
                }
            }
        }

        private void LoadFxData()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "FXIII Indexed Files |*.ind";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    fxData = FxData.LoadFxData(ofd.FileName);
                }
            }
        }

        private void SaveHelmetData()
        {
            int num = 0;
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "FXIII Deindexed Files |*.dat";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    IniManager reader = new IniManager(ofd.FileName);

                    num = int.Parse(reader.GetValue("INIT", "NumHelmets"));

                    helmetData = new HeadData[num];

                    List<string> mains = reader.GetAllMains();
                    mains.Remove("INIT");

                    for (int i = 0; i < num; i++)
                    {
                        HeadData h = new HeadData();

                        h.Walk = new int[4];

                        for (int j = 0; j < 4; j++)
                        {
                            h.Walk[j] = int.Parse(reader.GetValue("Helmet" + (i + 1), "Dir" + (j+1)));
                        }

                        helmetData[i] = h;
                    }
                }
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "FXIII Indexed Files |*.ind";
                string path = String.Empty;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    path = sfd.FileName;
                }

                if (path.Length != 0)
                    HeadData.SaveHeadData(helmetData, path);
            }
            Cursor.Current = Cursors.Default;
            MessageBox.Show("Listo!");

        }
        private void SaveFxData()
        {
            int num = 0;
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "FXIII Deindexed Files |*.dat";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    IniManager reader = new IniManager(ofd.FileName);

                    num = int.Parse(reader.GetValue("INIT", "NumFxs"));

                    fxData = new FxData[num];

                    List<string> mains = reader.GetAllMains();
                    mains.Remove("INIT");

                    for (int i = 0; i < num; i++)
                    {
                        FxData f = new FxData();

                        f.Animation = int.Parse(reader.GetValue("Fx" + (i + 1), "Animation"));
                        f.OffsetX = int.Parse(reader.GetValue("Fx" + (i + 1), "OffsetX"));
                        f.OffsetY = int.Parse(reader.GetValue("Fx" + (i + 1), "OffsetY"));

                        fxData[i] = f;
                    }
                }
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "FXIII Indexed Files |*.ind";
                string path = String.Empty;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    path = sfd.FileName;
                }

                if (path.Length != 0)
                    FxData.SaveFxData(fxData, path);
            }
            Cursor.Current = Cursors.Default;
            MessageBox.Show("Listo!");
        }
        private void SaveHeadData()
        {
            int num = 0;
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "FXIII Deindexed Files |*.dat";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    IniManager reader = new IniManager(ofd.FileName);

                    num = int.Parse(reader.GetValue("INIT", "NumHeads"));

                    headData = new HeadData[num];

                    List<string> mains = reader.GetAllMains();
                    mains.Remove("INIT");

                    for (int i = 0; i < num; i++)
                    {
                        HeadData h = new HeadData();

                        h.Walk = new int[4];

                        for (int j = 0; j < 4; j++)
                        {
                            h.Walk[j] = int.Parse(reader.GetValue("Head" + (i + 1), "Dir" + (j+1)));
                        }

                        headData[i] = h;
                    }
                }
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "FXIII Indexed Files |*.ind";
                string path = String.Empty;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    path = sfd.FileName;
                }

                if (path.Length != 0)
                    HeadData.SaveHeadData(headData, path);
            }
            Cursor.Current = Cursors.Default;
            MessageBox.Show("Listo!");
        }
        private void SaveBodyData()
        {
            int num = 0;
            using(OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "FXIII Deindexed Files |*.dat";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    IniManager reader = new IniManager(ofd.FileName);

                    num = int.Parse(reader.GetValue("INIT", "NumBodys"));

                    bodyData = new BodyData[num];

                    List<string> mains = reader.GetAllMains();
                    mains.Remove("INIT");

                    for (int i = 0; i < num; i++)
                    {
                        BodyData b = new BodyData();

                        b.Walk = new int[4];

                        for (int j = 0; j < 4; j++)
                        {
                            b.Walk[j] = int.Parse(reader.GetValue("Body" + (i+1), "Dir" + (j+1)));
                        }

                        b.HeadOffsetX = int.Parse(reader.GetValue("Body" + (i + 1), "HeadOffsetX"));
                        b.HeadOffsetY = int.Parse(reader.GetValue("Body" + (i + 1), "HeadOffsetY"));

                        bodyData[i] = b;
                    }
                }
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "FXIII Indexed Files |*.ind";
                string path = String.Empty;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    path = sfd.FileName;
                }

                if (path.Length != 0)
                    BodyData.SaveBodyData(bodyData, path);
            }
            Cursor.Current = Cursors.Default;
            MessageBox.Show("Listo!");
        }
        private void SaveGrhData()
        {
            int num = 0;
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "FXIII Deindexed Files |*.dat";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    IniManager reader = new IniManager(ofd.FileName);

                    num = int.Parse(reader.GetValue("INIT", "NumGrhs"));

                    grhData = new GrhData[num];

                    List<string> mains = reader.GetAllMains();
                    mains.Remove("INIT");

                    for (int i = 0; i < num; i++)
                    {
                        string grh = reader.GetValue("GRAPHICS", "Grh" + (i + 1));
                        string[] s = grh.Split('-');

                        if (s[0].CompareTo("") != 0)
                        {
                            GrhData g = new GrhData();

                            g.Grh = i + 1;

                            g.numFrames = short.Parse(s[0]);

                            if (g.numFrames > 1)
                            {
                                g.frames = new short[g.numFrames];
                                for (int j = 0; j < g.numFrames; j++)
                                {
                                    g.frames[j] = short.Parse(s[j + 1]);
                                }

                                g.speed = float.Parse(s[g.numFrames + 1]);
                            }
                            else
                            {
                                g.fileNum = int.Parse(s[1]);
                                g.sX = short.Parse(s[2]);
                                g.sY = short.Parse(s[3]);
                                g.pixelWidth = short.Parse(s[4]);
                                g.pixelHeight = short.Parse(s[5]);
                            }

                            grhData[i] = g;
                        }
                    }                    
                }

                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "FXIII Indexed Files |*.ind";
                    string path= String.Empty;
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        path = sfd.FileName;
                    }

                    if (path.Length!=0)
                        GrhData.SaveGrhData130(grhData, path, num);
                }
            }
            Cursor.Current = Cursors.Default;
            MessageBox.Show("Listo!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    #region Bodys
                    SaveBodyData();
                    #endregion
                    break;
                case 1:
                    #region Heads
                    SaveHeadData();
                    #endregion
                    break;
                case 2:
                    #region Graphics
                    SaveGrhData();
                    #endregion
                    break;
                case 3:
                    #region Helmets
                    SaveHelmetData();
                    #endregion
                    break;
                case 4:
                    #region Fxs
                    SaveFxData();
                    #endregion
                    break;
            }
        }
    }
}
