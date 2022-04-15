namespace AdventOfCode._2021
{
    internal class Day16
    {
        bool ReadLiteral(BitStream stream, out long value)
        {
            value = 0;

            bool dataContinues;

            List<byte> bytes = new List<byte>();

            do
            {
                if (!stream.ReadBit(out dataContinues))
                    return false;

                byte group;

                if (!stream.ReadByte(4, out group))
                    return false;

                bytes.Add(group);
            }
            while (dataContinues);

            for (int i = 0; i < bytes.Count; i++)
            {
                value |= ((long)bytes[i] << ((bytes.Count - (i + 1)) * 4));
            }

            return true;
        }

        long versionSum = 0;

        bool ReadPacket(BitStream stream, out long value)
        {
            value = 0;

            byte version;
            byte id;

            stream.ReadByte(3, out version);
            stream.ReadByte(3, out id);

            versionSum += version;

            if (id == 4)
            {
                if (!ReadLiteral(stream, out value))
                {
                    return false;
                }
            }
            else
            {
                bool typeID;

                stream.ReadBit(out typeID);

                List<long> values = new List<long>();

                if (!typeID)  // Length type 0
                {
                    UInt16 length;

                    stream.ReadUInt16(15, out length);

                    int endPosition = stream.StreamPosition + length;

                    while (stream.StreamPosition != endPosition)
                    {
                        ReadPacket(stream, out value);

                        values.Add(value);
                    }
                }
                else  // Length type 1
                {
                    UInt16 numSubPackets;

                    stream.ReadUInt16(11, out numSubPackets);

                    for (int i = 0; i < numSubPackets; i++)
                    {
                        long subValue;

                        ReadPacket(stream, out subValue);

                        values.Add(subValue);
                    }
                }

                switch (id)
                {
                    case 0:
                        value = values.Sum();
                        break;
                    case 1:
                        value = values.Aggregate((long)1, (acc, val) => acc * val);
                        break;
                    case 2:
                        value = values.Min();
                        break;
                    case 3:
                        value = values.Max();
                        break;
                    case 5:
                        value = (values[0] > values[1]) ? 1 : 0;
                        break;
                    case 6:
                        value = (values[0] < values[1]) ? 1 : 0;
                        break;
                    case 7:
                        value = (values[0] == values[1]) ? 1 : 0;
                        break;
                }
            }

            return true;
        }

        long Decode(string transmission)
        {
            BitStream stream = BitStream.FromHexString(transmission);

            long value;

            ReadPacket(stream, out value);

            return value;
        }

        public long Compute()
        {
            //Decode("D2FE28");
            //Decode("38006F45291200");
            //Decode("EE00D40C823060");

            //long result = Decode("C200B40A82");

            long result = Decode(File.ReadAllText(@"C:\Code\AdventOfCode\Input\2021\Day16.txt").Trim());

            return result;
        }
    }
}
