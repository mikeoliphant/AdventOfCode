using System.Diagnostics;

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
            get
            {
                string path = Path.Combine(DataFileDir, "Day" + DayNumber + ".txt");

                //if (!File.Exists(path))
                //{
                //    string url = @"https://adventofcode.com/" + Year + "/day/" + DayNumber;

                //    using (var client = new HttpClient())
                //    {
                //        using (var s = client.GetStreamAsync(url))
                //        {
                //            using (var fs = new FileStream(path, FileMode.Create))
                //            {
                //                s.Result.CopyTo(fs);
                //            }
                //        }
                //    }
                //}

                return path;
            }
        }

        public string DataFileTest
        {
            get
            {
                string path = Path.Combine(DataFileDir, "Day" + DayNumber + "Test.txt");

                if (!File.Exists(path))
                {
                    Process.Start("notepad.exe", path).WaitForExit();
                }

                return path;
            }
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
