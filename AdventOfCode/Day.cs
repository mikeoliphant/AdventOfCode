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

        public string DataFile
        {
            get { return @"C:\Code\AdventOfCode\Input\" + Year + @"\Day" + DayNumber + ".txt"; }
        }

        public string DataFileTest
        {
            get { return @"C:\Code\AdventOfCode\Input\" + Year + @"\Day" + DayNumber + "Test.txt"; }
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
