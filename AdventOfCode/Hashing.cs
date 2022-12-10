namespace AdventOfCode
{
    public class CountHash<T> where T : notnull
    {
        Dictionary<T, ulong> countHash = new Dictionary<T, ulong>();

        public long NumNonzero
        {
            get { return countHash.Count; }
        }

        public IEnumerable<KeyValuePair<T, ulong>> AllPairs
        {
            get { return countHash;  }
        }

        public ulong this[T hashVal]
        {
            get
            {
                if (!countHash.ContainsKey(hashVal))
                    return 0;

                return countHash[hashVal];
            }
            set
            {
                if (value == 0)
                    countHash.Remove(hashVal);
                else
                    countHash[hashVal] = value;
            }
        }

        public void Increment(T hashVal)
        {
            if (!countHash.ContainsKey(hashVal))
                countHash[hashVal] = 1;
            else
                countHash[hashVal]++;
        }

        public void Decrement(T hashVal)
        {
            if (countHash[hashVal] == 1)
                countHash.Remove(hashVal);
            else
                countHash[hashVal]--;
        }
    }
}
