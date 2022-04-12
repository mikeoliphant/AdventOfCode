using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day23
    {
        IntcodeComputer[] computers = null;
        int numComputers = 50;
        Dictionary<int, Queue<(long X, long Y)>> packets = new Dictionary<int, Queue<(long X, long Y)>>();

        long natX = 0;
        long natY = 0;
        long lastNatY = -1;
        long numActivePackets = 0;

        void ReadInput()
        {
            long[] program = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2019\Day23.txt").ToLongs(',').ToArray();

            computers = new IntcodeComputer[numComputers];

            for (int i = 0; i < numComputers; i++)
            {
                computers[i] = new IntcodeComputer();
                computers[i].SetProgram(program);

                packets[i] = new Queue<(long X, long Y)>();
            }
        }

        void AddPacket(int computer, long x, long y)
        {
            if (computer < numComputers)
            {
                numActivePackets++;

                packets[computer].Enqueue((x, y));
            }

            if (computer == 255)
            {
                natX = x;
                natY = y;
            }
        }

        long InputFunction(int computerID)
        {
            var queue = packets[computerID];

            if (queue.Count > 0)
            {
                numActivePackets--;

                var packet = queue.Dequeue();

                computers[computerID].AddInput(packet.Y);

                return packet.X;
            }

            return -1;
        }


        public long Compute()
        {
            ReadInput();

            // Send the computers their addresses
            for (int i = 0; i < numComputers; i++)
            {
                int computerID = i;

                computers[i].InputFunc = delegate { return InputFunction(computerID); };
                computers[i].AddInput(i);
            }

            int idlePasses = 0;

            do
            {
                bool isIdle = true;

                for (int computerID = 0; computerID < numComputers; computerID++)
                {
                    IntcodeComputer computer = computers[computerID];

                    int opCode = computer.RunInstruction();

                    if (opCode == 4) // output
                    {
                        int id = (int)computer.GetLastOutput();
                        long x = computer.GetNextOutput();
                        long y = computer.GetNextOutput();

                        AddPacket(id, x, y);

                        isIdle = false;
                    }
                }

                if (isIdle)
                    idlePasses++;

                if ((numActivePackets == 0) && (idlePasses > 1000))
                {
                    idlePasses = 0;

                    if (lastNatY == natY)
                    {
                        return lastNatY;
                    }

                    lastNatY = natY;

                    AddPacket(0, natX, natY);
                }
            }
            while (true);

            throw new InvalidOperationException();
        }
    
    }
}
