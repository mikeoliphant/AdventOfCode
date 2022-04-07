using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day8
    {
        int layerWidth = 25;
        int layerHeight = 6;
        Grid<byte>[] layers;

        void ReadInput()
        {
            string imageData = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2019\Day8.txt").Trim();

            int numLayers = imageData.Length / (layerWidth * layerHeight);

            layers = new Grid<byte>[numLayers];

            int pos = 0;

            for (int i = 0; i < numLayers; i++)
            {
                layers[i] = new Grid<byte>(layerWidth, layerHeight);

                for (int y = 0; y < layerHeight; y++)
                {
                    for (int x = 0; x < layerWidth; x++)
                    {
                        layers[i][x, y] = (byte)(imageData[pos++] - '0');
                    }
                }
            }
        }

        public int Compute()
        {
            ReadInput();

            int minZeros = int.MaxValue;
            Grid<byte> minLayer = null;

            foreach (Grid<byte> layer in layers)
            {
                int numZeros = layer.GetAllValues().Count(p => (p == 0));

                if (numZeros < minZeros)
                {
                    minZeros = numZeros;
                    minLayer = layer;
                }
            }

            return minLayer.GetAllValues().Count(p => (p == 1)) * minLayer.GetAllValues().Count(p => (p == 2));
        }

        public int Compute2()
        {
            ReadInput();

            Grid<char> image = new Grid<char>(layerWidth, layerHeight);

            for (int y = 0; y < layerHeight; y++)
            {
                for (int x = 0; x < layerWidth; x++)
                {
                    byte pixel = 2;

                    foreach (Grid<byte> layer in layers)
                    {
                        byte layerPixel = layer[x, y];

                        if (layerPixel != 2)
                        {
                            image[x, y] = (layerPixel == 0) ? ' ' : '#';

                            break;
                        }
                    }
                }
            }

            image.PrintToConsole();

            return 0;
        }
    }
}
