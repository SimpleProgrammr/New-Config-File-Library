namespace NewConfigFileUser
{
    public class Read
    {
        public Read(string filePath)
        {
            this.filePath = filePath;
            fileLines = File.ReadAllLines(this.filePath);
            Categories();
        }

        public string[] fileLines;

        public string filePath;
        public string[] CatArray;
        public int[] CatLocalisations;
        public string[] VarNames;

        public void Categories()
        {
            var catList = new List<string>();
            var catLoc = new List<int>();
            bool valuesLine = false;
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
                    continue;

                catList.Add(l);
                catLoc.Add(Array.IndexOf(fileLines, line));
            }

            CatArray = [.. catList];
            CatLocalisations = catLoc.ToArray();
        }

        public string[] VariableNames(string CatName)
        {
            if (!CatArray.Contains(CatName))
                throw new NotImplementedException($"Unable to fing category \"{CatName}\" ");

            bool valuesLine = false;
            var variableNames = new List<string>();

            for (int i = CatLocalisations[Array.IndexOf(CatArray, CatName)] + 1; ; i++)
            {
                
                if (fileLines[i] == "{")
                    continue;
                

                if (fileLines[i] == "}")
                    break;


                if (fileLines[i] == string.Empty)
                    continue;

                string name = fileLines[i].Remove(Array.IndexOf(fileLines[i].ToArray(), ':')).Trim();
                variableNames.Add(name);
            }

            VarNames = variableNames.ToArray();
            return variableNames.ToArray();
        }

        public string[] Values(string CatName)
        {
            if (!CatArray.Contains(CatName))
                throw new NotImplementedException($"Unable to fing category \"{CatName}\" ");

            bool valuesLine = false;
            var variableNames = new List<string>();

            for (int i = CatLocalisations[Array.IndexOf(CatArray, CatName)] + 1; ; i++)
            {

                if (fileLines[i] == "{")
                    continue;


                if (fileLines[i] == "}")
                    break;


                if (fileLines[i] == string.Empty)
                    continue;

                string name = fileLines[i].Substring(Array.IndexOf(fileLines[i].ToArray(), ':')+1).Trim();
                variableNames.Add(name);
            }

            VarNames = variableNames.ToArray();
            return variableNames.ToArray();
            
        }
    }



    public class Write
    {
        
    }
}
