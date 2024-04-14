namespace NewConfigFileUser
{
    public class Read
    {
        public Read(string fileP)
        {
            if (fileP == null || fileP == string.Empty)
                throw new ArgumentNullException(nameof(fileP));

            this.filePath = fileP;
            fileLines = File.ReadAllLines(this.filePath);
            RepairFile();
            Map();
        }

        public string filePath { get; set; }
        public string[] fileLines;

        public List<string> categories = [];
        public Dictionary<string, Dictionary<string, object>> values = [];

        public void RepairFile()
        {

        }

        public void Map()
        {
            bool valuesLine = false, objectVal = false;
            string category = "";
            int lnum = -1;

            foreach (var line in fileLines)
            {
                lnum++;
                string l = line.Trim();
                if (l == "")
                    continue;

                if (l.StartsWith('$') || objectVal)
                {
                    if (l == "{")
                        continue;

                    if (!objectVal)
                        ReadObject(lnum, category, l.Replace("$", ""));

                    objectVal = true;

                    if (l == "}")
                        objectVal = false;

                    continue;
                }

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


                values.Add(l, []);
                category = l;
            }

        }

        public void ReadObject(int lnum, string category, string objname)
        {
            lnum += 2;
            var vals = new Dictionary<string, object>();
            for (; ; lnum++)
            {
                if (fileLines[lnum].Contains("}"))
                    break;
                vals.Add(GetVarName(fileLines[lnum]), GetVarValue(fileLines[lnum]));
            }
            values[category].Add(objname, vals);

        }

        public string GetVarName(string line) => line.Split(":")[0].Trim();


        public object GetVarValue(string line)
        {
            line = line.Split(":")[1].Trim();

            switch (GetType(line))
            {
                case "String":
                    return line.Replace("\"","");

                case "Int":
                    return Convert.ToInt32(line);

                case "Double":
                    return Convert.ToDouble(line.Replace('.', ','));

                case "IntList":
                    var ints = new List<int>();
                    foreach (var item in line.Split('.'))
                    {
                        ints.Add(Convert.ToInt32(item.Trim()));
                    }
                    return ints;

                case "StringList":
                    var val = line.Replace("[", "").Replace("]", "").Replace("\"", "").Split(',').ToList();
                    return val;

                case "DoubleList":
                    var doubles = new List<double>();

                    foreach (var item in line.Replace("[","").Replace("]","").Split(','))
                    {
                        doubles.Add(Convert.ToDouble(item.Replace('.', ',').Trim()));
                    }
                    return doubles;
            }
            return "Unknow data notation";
        }

        private string GetType(string line)
        {
            if (line.StartsWith('\"') && line.EndsWith('\"'))
                return "String";

            else if (line.StartsWith('[') && line.EndsWith(']'))
            {
                line = line.Replace("[", "").Replace("]", "");
                if (line.Contains('\"'))
                {
                    return "StringList";
                }
                else if (line.Contains('.') || line.Contains(','))
                {

                    return "DoubleList";
                }
                else
                {
                    return "IntList";
                }

            }
            else if (line.Contains('.'))
            {
                return "Double";
            }
            else
                return "Int";
        }

    }

    public class Write
    {

    }

}
