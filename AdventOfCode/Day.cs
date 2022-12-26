namespace AdventOfCode
{
    internal class Day
    {
        public string Year
        {
            get
            {
                string nameSpace = GetType().Namespace;

                return nameSpace.Substring(nameSpace.Length - 4);
            }
        }

        public string DayNumber
        {
            get
            {
                string typeName = GetType().Name;

                return typeName.Substring(3);
            }
        }

        public string DataFileDir
        {
            get { return @"C:\Code\AdventOfCode\Input\" + Year; }
        }

        public string DataFile
        {
            get { return Path.Combine(DataFileDir, "Day" + DayNumber + ".txt"); }
        }

        public string DataFileTest
        {
            get { return Path.Combine(DataFileDir, "Day" + DayNumber + "Test.txt"); }
        }

        public virtual long Compute()
        {
            throw new NotImplementedException();
        }

        public virtual long Compute2()
        {
            throw new NotImplementedException();
        }
    }
}
