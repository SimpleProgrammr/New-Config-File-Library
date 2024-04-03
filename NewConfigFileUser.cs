namespace NewConfigFileUser
{
    public class Read
    {
        public Read(string filePath)
        {
            this.filePath = filePath;
            fileLines = File.ReadAllLines(this.filePath);
            RepairFile();
            Map();
        }

        public string filePath;
        public string[] fileLines;
        public List<string> categories = new List<string>();
        public Dictionary<string, Dictionary<string, object>> values;

        public void RepairFile()
        {

        }

        public void Map()
        {
            bool valuesLine = false;
            string category = "";
            foreach (var line in fileLines)
            {
                string l = line.Trim();
                if (l == "")
                    continue;

                if (l == "{")
                {
                    valuesLine = true;
                    continue;
                }
                if (l == "}")
                {
                    valuesLine = false;
                    continue;
                }
                if (valuesLine)
                {
                    values[category].Add(GetVarName(l), GetVarValue(l));
                    continue;
                }


                values.Add(l, new Dictionary<string, object>());
                category = l;
            }

        }

        public string GetVarName(string line)
        {
            return line.Remove(line.IndexOf(":")).Trim();
        }

        public object GetVarValue(string line)
        {
            line = line.Substring(line.IndexOf(":")).Trim();

            if (line.StartsWith('\"') && line.EndsWith('\"'))
                return line;
            else if (line.StartsWith('[') && line.EndsWith(']'))
            {
                line = line.Replace("[", "").Replace("]", "");
                if (line.Contains('\"'))
                {
                    var val = line.Replace("\"", "").Trim().Split(',');
                    return val;
                }
                else if (line.Contains('.'))
                {
                    var val = line.Trim().Split(',');
                    double[] doubles = new double[val.Length];
                    double = Convert.ToDouble(val);
                }
            }

            return "";
        }

    }

    public class Write
    {

    }

}
