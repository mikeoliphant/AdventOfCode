namespace AdventOfCode._2024
{
    class Block
    {
        public int ID;
        public int Number;
        public int FreeAfter;

        public override string ToString()
        {
            return new string((char)('0' + ID), Number) + new string('.', FreeAfter);
        }
    }

    internal class Day9 : Day
    {
        LinkedList<Block> ReadData(string data)
        {
            LinkedList<Block> blocks = new();

            for (int i = 0; i < data.Length; i += 2)
            {
                Block block = new()
                {
                    ID = i / 2,
                    Number = data[i] - '0'
                };

                if (i < (data.Length - 1))
                {
                    block.FreeAfter = data[i + 1] - '0';
                }

                blocks.AddLast(block);
            }

            return blocks;
        }

        void Compress(LinkedList<Block> blocks, bool moveAll)
        {
            var lastBlock = blocks.Last;

            while (lastBlock != blocks.First)
            {
                var freeBlock = blocks.First;

                var nextLast = lastBlock.Previous;

                while (freeBlock != lastBlock)
                {
                    if (freeBlock.Value.FreeAfter == 0)
                    {
                        freeBlock = freeBlock.Next;

                        continue;
                    }

                    if (lastBlock.Value.Number <= freeBlock.Value.FreeAfter)
                    {
                        int toMove = lastBlock.Value.Number;

                        lastBlock.Previous.Value.FreeAfter += (toMove + lastBlock.Value.FreeAfter);

                        lastBlock.Value.FreeAfter = freeBlock.Value.FreeAfter - toMove;

                        blocks.Remove(lastBlock);
                        blocks.AddAfter(freeBlock, lastBlock);

                        freeBlock.Value.FreeAfter = 0;

                        //Console.WriteLine(string.Concat(blocks));

                        continue;
                    }
                    else
                    {
                        if (moveAll)
                        {
                            freeBlock = freeBlock.Next;
                        }
                        else
                        {
                            int toMove = freeBlock.Value.FreeAfter;

                            lastBlock.Value.Number -= toMove;

                            blocks.AddAfter(freeBlock, new Block()
                            {
                                ID = lastBlock.Value.ID,
                                Number = toMove,
                                FreeAfter = 0
                            });

                            freeBlock.Value.FreeAfter = 0;
                        }
                    }
                }

                lastBlock = nextLast;
            }
        }

        long GetChecksum(IEnumerable<Block> blocks)
        {
            long checksum = 0;

            long pos = 0;

            foreach (Block block in blocks)
            {
                int num = block.Number;

                while (num > 0)
                {
                    checksum += pos * block.ID;

                    num--;
                    pos++;
                }

                pos += block.FreeAfter;
            }

            return checksum;
        }

        public override long Compute()
        {
            //var blocks = ReadData("2333133121414131402");

            var blocks = ReadData(File.ReadAllText(DataFile).Trim());

            //Console.WriteLine(string.Concat(blocks));

            Compress(blocks, moveAll: false);

            //Console.WriteLine(string.Concat(blocks));

            long checksum = GetChecksum(blocks);

            return checksum;
        }

        public override long Compute2()
        {
            //var blocks = ReadData("2333133121414131402");

            var blocks = ReadData(File.ReadAllText(DataFile).Trim());

            Compress(blocks, moveAll: true);

            long checksum = GetChecksum(blocks);

            // 6834767167924 - too high
            // 7399101431680 - too high

            return checksum;
        }
    }
}
