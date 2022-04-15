using System.Collections;

namespace AdventOfCode
{
    public class BitStream
    {
        BitArray bits;
        int streamPos = 0;

        public int StreamPosition { get { return streamPos; } }
        public int StreamLength { get { return bits.Length; } }

        public static BitStream FromHexString(string hexData)
        {
            BitStream stream = new BitStream();
            stream.CreateFromHexData(hexData);

            return stream;
        }

        public void CreateFromHexData(string hexData)
        {
            bits = new BitArray(hexData.Length * 4);

            int bitPos = 0;

            foreach (char c in hexData)
            {
                int val;

                if (c >= '0' && c <= '9')
                    val = c - '0';
                else if (c >= 'A' && c <= 'F')
                    val = 10 + (c - 'A');
                else
                    throw new ArgumentException("Hex data is invalid");

                for (int i = 0; i < 4; i++)
                {
                    bits[bitPos + i] = ((val & (1 << (3 - i))) != 0);
                }

                bitPos += 4;
            }
        }

        public bool Seek(int numBits)
        {
            streamPos += numBits;

            return (streamPos < bits.Length);
        }

        public bool ReadBit(out bool bit)
        {
            bit = false;

            if ((bits.Length - streamPos) < 1)
                return false;

            bit = bits[streamPos++];

            return true;
        }

        public bool ReadByte(int numBits, out byte data)
        {
            if (numBits > 8)
                throw new ArgumentOutOfRangeException("ReadByte can read a max of 8 bits");

            data = 0;

            if ((bits.Length - streamPos) < numBits)
                return false;

            for (int i = 0; i < numBits; i++)
            {
                if (bits[streamPos + i])
                    data |= (byte)(1 << ((numBits - 1) - i));
            }

            streamPos += numBits;

            return true;
        }

        public bool ReadUInt16(int numBits, out UInt16 data)
        {
            if (numBits > 16)
                throw new ArgumentOutOfRangeException("ReadUInt16 can read a max of 16 bits");

            data = 0;

            if ((bits.Length - streamPos) < numBits)
                return false;

            for (int i = 0; i < numBits; i++)
            {
                if (bits[streamPos + i])
                    data |= (UInt16)(1 << ((numBits - 1) - i));
            }

            streamPos += numBits;

            return true;
        }


        public override string ToString()
        {
            char[] data = new char[bits.Length];

            for (int i = 0; i < bits.Length; i++)
            {
                data[i] = bits[i] ? '1' : '0';
            }

            return new string(data);
        }
    }
}
