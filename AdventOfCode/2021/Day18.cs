namespace AdventOfCode._2021
{
    internal class Pair
    {
        public Pair Left { get; set; }
        public Pair Right { get; set; }
        public int Value { get; set; }
        public Pair PreviousTerminal { get; set; }
        public Pair NextTerminal { get; set; }

        public Pair()
        {
            Value = -1;
        }

        public Pair(Pair left, Pair right)
            : this()
        {
            this.Left = left;
            this.Right = right;
        }

        public Pair(int value)
            : this()
        {
            this.Value = value;
        }

        public Pair GetLeftmost()
        {
            if (Value != -1)
                return this;

            return Left.GetLeftmost();
        }

        public Pair GetRightmost()
        {
            if (Value != -1)
                return this;

            return Right.GetRightmost();
        }

        void SetNextTerminal(Pair nextTerminal)
        {
            if (Value != -1)
            {
                this.NextTerminal = nextTerminal;
            }
            else
            {
                Right.SetNextTerminal(nextTerminal);
            }
        }

        void SetPreviousTerminal(Pair previousTerminal)
        {
            if (Value != -1)
            {
                this.PreviousTerminal = previousTerminal;
            }
            else
            {
                Left.SetPreviousTerminal(previousTerminal);
            }
        }

        public void ComputeLeftRight()
        {
            if (Value != -1)
                return;

            Left.SetNextTerminal(Right.GetLeftmost());
            Right.SetPreviousTerminal(Left.GetRightmost());

            Left.ComputeLeftRight();
            Right.ComputeLeftRight();
        }

        public int GetMagnitude()
        {
            if (Value != -1)
                return Value;

            return (3 * Left.GetMagnitude()) + (2 * Right.GetMagnitude());
        }

        public Pair Duplicate()
        {
            if (Value != -1)
                return new Pair(Value);

            return new Pair(Left.Duplicate(), Right.Duplicate());
        }

        public override string ToString()
        {
            return (Value != -1) ? Value.ToString() : "[" + (Left.ToString() + "," + Right.ToString() + "]");
        }
    }

    internal class Day18
    {
        string[] numbers;

        void ReadInput()
        {
            numbers = File.ReadLines(@"C:\Code\AdventOfCode\Input\2021\Day18.txt").ToArray();
        }

        Pair Parse(string number)
        {
            Pair pair = new Pair();

            if (number[0] != '[')
            {
                pair.Value = int.Parse(number);

                return pair;
            }

            number = number.Substring(1, number.Length - 2);

            int pos = 0;
            int nest = 0;

            do
            {
                if (number[pos] == '[')
                    nest++;
                else if (number[pos] == ']')
                    nest--;

                pos++;
            }
            while (nest > 0);

            if (number[pos] == ',')
            {
                pair.Left = Parse(number.Substring(0, pos));
                pair.Right = Parse(number.Substring(pos + 1, number.Length - pos - 1));

                return pair;
            }
            else
            {
                throw new Exception();
            }
        }

        Pair Reduce(Pair pair, bool doSplit)
        {
            return Reduce(pair, 0, doSplit);
        }

        Pair Reduce(Pair pair, int nestLevel, bool doSplit)
        {
            if (pair.Value != -1)
            {
                if (doSplit && (pair.Value > 9))     // Split
                {
                    int half = pair.Value / 2;

                    return new Pair(new Pair(half), new Pair(pair.Value - half));
                }

                return pair;
            }

            if ((nestLevel >= 4) && (pair.Left.Value != -1) && (pair.Right.Value != -1))    // Explode
            {
                if (pair.Left.PreviousTerminal != null)
                    pair.Left.PreviousTerminal.Value += pair.Left.Value;

                if (pair.Right.NextTerminal != null)
                    pair.Right.NextTerminal.Value += pair.Right.Value;

                return new Pair(0);
            }

            Pair left = Reduce(pair.Left, nestLevel + 1, doSplit);

            if (left != pair.Left)
            {
                return new Pair(left, pair.Right);
            }

            Pair right = Reduce(pair.Right, nestLevel + 1, doSplit);

            if (right != pair.Right)
            {
                return new Pair(pair.Left, right);
            }

            return pair;
        }

        public Pair AddNumbers(Pair number1, Pair number2)
        {
            Pair pair = new Pair(number1, number2);

            Pair newPair = pair;

            do
            {
                do
                {
                    pair = newPair;

                    pair.ComputeLeftRight();
                    newPair = Reduce(pair, doSplit: false);
                }
                while (pair != newPair);

                pair.ComputeLeftRight();
                newPair = Reduce(pair, doSplit: true);
            }
            while (pair != newPair);

            return pair;
        }
    
        public long Compute()
        {
            ReadInput();

            //Pair pair = Parse("[[[[[4,3],4],4],[7,[[8,4],9]]],[1,1]]");

            Pair pair = null;

            foreach (string number in numbers)
            {
                Pair parsed = Parse(number);

                if (pair == null)
                {
                    pair = parsed;

                    continue;
                }

                pair = AddNumbers(pair, parsed);
            }

            int magnitude = pair.GetMagnitude();

            return magnitude;
        }

        public long Compute2()
        {
            ReadInput();

            long maxMagnitude = 0;

            Pair[] parsed = (from number in numbers select Parse(number)).ToArray();

            foreach (Pair pair1 in parsed)
            {
                foreach (Pair pair2 in parsed)
                {
                    if (pair1 != pair2)
                    {
                        int magnitude = AddNumbers(pair1.Duplicate(), pair2.Duplicate()).GetMagnitude();

                        maxMagnitude = Math.Max(maxMagnitude, magnitude);
                    }
                }
            }

            return maxMagnitude;
        }
    }
}
