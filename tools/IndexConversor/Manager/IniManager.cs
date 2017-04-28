using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DatConverter
{
    class IniManager
    {
        List<MainNode> fileData;
        int MainNodes;

        MainNode lastMain;

        public List<string> GetAllMains()
        {
            List<string> tmp = new List<string>();
            foreach(MainNode m in fileData)
            {
                tmp.Add(m.Name);
            }

            return tmp;

        }

        public IniManager(string path, bool create = false, bool order = false)
        {
            MainNodes = 0;

            if (!File.Exists(path) && create)
            {
                File.Create(path).Dispose();
                fileData = new List<MainNode>();
                return;
            }

            string[] tempData = File.ReadAllLines(path, Encoding.Default);

            fileData = new List<MainNode>();
            Match regex;

            foreach(string s in tempData)
            {
                if (!s.Equals(""))
                {
                    regex = Regex.Match(s, @"\[(.+)\]");

                    if (regex.Groups[1].Success)
                    {
                        fileData.Add(new MainNode(regex.Groups[1].Value));
                        MainNodes++;
                    }
                    else
                    {
                        if (MainNodes > 0)
                        {
                            regex = Regex.Match(s, @"(.+)\=(.+)");
                            if (regex.Groups[1].Success && regex.Groups[2].Success)
                            {
                                fileData[MainNodes - 1].values.Add(
                                    new ChildNode(regex.Groups[1].Value.Trim(), regex.Groups[2].Value.Trim()));
                            }
                        }
                    }
                }
            }

            if (order)
            {
                if (fileData.Count != 0)
                {
                    MainNode init = fileData[0];
                    fileData.RemoveAt(0);

                    fileData = fileData.OrderBy(x => int.Parse(x.Name.Substring(3))).ToList();
                    //fileData.Sort((x, y) => string.Compare(x.Name.Substring(3), y.Name.Substring(3)));
                    fileData.Insert(0, init);
                }
            }
        }

        public bool DeleteKey(string Main, string Key)
        {
            MainNode m;

            if (lastMain != null && Main.Equals(lastMain.Name))
                m = lastMain;
            else
            {
                m = fileData.Find(
                    x => x.Name.Equals(Main, StringComparison.InvariantCultureIgnoreCase));
                lastMain = m;
            }

            if (m!= null)
            {
                ChildNode c = m.values.Find(
                    x => x.kvp.Key.Equals(Key, StringComparison.InvariantCultureIgnoreCase));

                if (c != null)
                {
                    m.values.Remove(c);
                    return true;
                }
                else
                {
                    return false;
                }
            }else
            {
                return false;
            }
        }
        public string GetValue(string Main, string Key)
        {
            MainNode m;

            if (lastMain != null && Main.Equals(lastMain.Name))
                m = lastMain;
            else
            {
                m = fileData.Find(
                    x => x.Name.Equals(Main, StringComparison.InvariantCultureIgnoreCase));
                lastMain = m;
            }

            if (m != null)
            {
                ChildNode c = m.values.Find(
                    x => x.kvp.Key.Equals(Key, StringComparison.InvariantCultureIgnoreCase));

                if (c != null)
                    return c.kvp.Value;
            }

            return "";
        }

        public void AddMain(string Main)
        {
            if (!Main.Equals(""))
            {
                MainNode m = new MainNode(Main);
                fileData.Add(m);
                lastMain = m;
            }
        }

        public void AddKeyValue(string Main, string key, string value)
        {
            //Match regex;
            MainNode m;
            if (!Main.Equals("") && !key.Equals("") && !value.Equals(""))
            {
                int indexMain = -1;

                if (Main.Equals(lastMain.Name, StringComparison.InvariantCultureIgnoreCase))
                    m = lastMain;
                else
                {
                    m = fileData.Find(
                        x => x.Name.Equals(Main, StringComparison.InvariantCultureIgnoreCase));
                    lastMain = m;
                }

                if (m != null)
                {
                    indexMain = fileData.IndexOf(m);

                    string s = key + '=' + value;
                    //regex = Regex.Match(s, @"(.+)\=(.+)"); //this is obvious
                    if (fileData[indexMain].values == null)
                    {
                        fileData[indexMain].values = new List<ChildNode>();
                        fileData[indexMain].values.Add(new ChildNode(key, value));
                    }
                    else
                    {
                        fileData[indexMain].values.Add(new ChildNode(key, value));
                    }
                }
            }
        }
        public void ChangeValue(string Main, string Key, string Value)
        {
            MainNode m;
            int indexMain = -1;
            int indexChild = -1;

            if (Main.Equals(lastMain.Name, StringComparison.InvariantCultureIgnoreCase))
                m = lastMain;
            else
            {
                m = fileData.Find(
                    x => x.Name.Equals(Main, StringComparison.InvariantCultureIgnoreCase));
                lastMain = m;
            }

            if (m != null)
            {
                indexMain = fileData.IndexOf(m);

                ChildNode c = m.values.Find(
                    x => x.kvp.Key.Equals(Key, StringComparison.InvariantCultureIgnoreCase));

                if (c != null) {
                    indexChild = m.values.IndexOf(c);

                    c.kvp = new KeyValuePair<string, string>(Key, Value);
                }
                if (indexMain != -1 && indexChild != -1)
                {
                    m.values[indexChild] = c;
                    fileData[indexMain] = m;
                }
            }
        }

        public void DumpFile(string path)
        {
            StringBuilder sb = new StringBuilder();

            foreach (MainNode m in fileData)
            {
                sb.Append('[' + m.Name + ']' + Environment.NewLine);

                foreach(ChildNode c in m.values)
                {
                    sb.Append(c.kvp.Key + '=' + c.kvp.Value + Environment.NewLine);
                }
            }

            File.WriteAllText(path, sb.ToString(), Encoding.Default);
        }

        private string[] ReadText(string path)
        {
            Encoding enc;
            List<string> temp = new List<string>();
            StringBuilder sb = new StringBuilder();

            using (StreamReader sr = new StreamReader(path, true))
            {
                enc = sr.CurrentEncoding;

                while (!sr.EndOfStream)
                {
                    temp.Add(sr.ReadLine());
                }
            }

            return temp.ToArray();
        }
    }
}
