using OpenTK.Graphics.ES10;
using OpenTK.Graphics.OpenGL;
using System.Globalization;
using System.Runtime.CompilerServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ScrollBar;

namespace AdventOfCode._2022
{
    static class Vector3Helper
    {
        public static Vector3 Round(this Vector3 vec)
        {
            return new Vector3((float)Math.Round(vec.X), (float)Math.Round(vec.Y), (float)Math.Round(vec.Z));
        }
    }

    internal class Day22 : Day
    {
        SparseGrid<char> grid;
        IEnumerable<string> commands;

        (int MinX, int MinY, int MaxX, int MaxY) bounds;

        void ReadInput(string file)
        {
            var sections = File.ReadAllText(file).SplitParagraphs();

            grid = new SparseGrid<char>().CreateDataFromRows(sections[0].SplitLines());

            grid.DefaultValue = ' ';

            foreach (var pos in grid.FindValue(' '))
            {
                grid.RemoveValue(pos);
            }

            var match = Regex.Matches(sections[1].Trim(), @"\d+|R|L");

            commands = match.Select(m => m.Value);
        }

        int GetMinX(int y)
        {
            for (int x = bounds.MinX; x <= bounds.MaxX; x++)
            {
                if (grid.IsValid((x, y)))
                    return x;
            }

            throw new Exception();
        }

        int GetMaxX(int y)
        {
            for (int x = bounds.MaxX; x >= bounds.MinX; x--)
            {
                if (grid.IsValid((x, y)))
                    return x;
            }

            throw new Exception();
        }

        int GetMinY(int x)
        {
            for (int y = bounds.MinY; y <= bounds.MaxY; y++)
            {
                if (grid.IsValid((x, y)))
                    return y;
            }

            throw new Exception();
        }

        int GetMaxY(int x)
        {
            for (int y = bounds.MaxY; y >= bounds.MinY; y--)
            {
                if (grid.IsValid((x, y)))
                    return y;
            }

            throw new Exception();
        }

        long WalkPath(LongVec2 pos, int facing, Func<(LongVec2 Pos, int Facing), (LongVec2 Pos, int Facing)> getNeighbor)
        {
            foreach (string command in commands)
            {
                switch (command)
                {
                    case "R":
                        facing = LongVec2.TurnFacing(facing, 1);
                        break;
                    case "L":
                        facing = LongVec2.TurnFacing(facing, -1);
                        break;
                    default:
                        int num = int.Parse(command);

                        while (num > 0)
                        {
                            LongVec2 neighbor = pos;

                            neighbor.AddFacing(facing, 1);

                            char val;

                            if (grid.TryGetValue((int)neighbor.X, (int)neighbor.Y, out val))
                            {
                                if (val == '#')
                                {
                                    break;
                                }

                                pos = neighbor;
                            }
                            else
                            {
                                var next = getNeighbor((pos, facing));

                                if (grid.TryGetValue((int)next.Pos.X, (int)next.Pos.Y, out val))
                                {
                                    if (val == '#')
                                    {
                                        break;
                                    }
                                }

                                pos = next.Pos;
                                facing = next.Facing;
                            }

                            num--;

                            switch (facing)
                            {
                                case 0:
                                    grid[(int)pos.X, (int)pos.Y] = '^';
                                    break;
                                case 1:
                                    grid[(int)pos.X, (int)pos.Y] = '>';
                                    break;
                                case 2:
                                    grid[(int)pos.X, (int)pos.Y] = 'v';
                                    break;
                                case 3:
                                    grid[(int)pos.X, (int)pos.Y] = '<';
                                    break;
                            }
                        }

                        break;
                }
            }

            return ((pos.Y + 1) * 1000) + ((pos.X + 1) * 4) + LongVec2.TurnFacing(facing, -1);
        }

        public override long Compute()
        {
            ReadInput(DataFile);

            grid.GetBounds();

            bounds = grid.GetBounds();

            //grid.PrintToConsole();

            LongVec2 pos = new LongVec2(grid.GetAll().OrderBy(p => p.Y).ThenBy(p => p.X).First());

            int facing = 1;

            return WalkPath(pos, facing, delegate ((LongVec2 Pos, int Facing) state)
            {
                pos.AddFacing(facing, 1);

                switch (state.Facing)
                {
                    case 0:
                        state.Pos.Y = GetMaxY((int)state.Pos.X);
                        break;
                    case 1:
                        state.Pos.X = GetMinX((int)state.Pos.Y);
                        break;
                    case 2:
                        state.Pos.Y = GetMinY((int)state.Pos.X);
                        break;
                    case 3:
                        state.Pos.X = GetMaxX((int)state.Pos.Y);
                        break;
                }

                return state;
            });
        }

        public struct Axis
        {
            const float PiOver2 = (float)Math.PI / 2.0f;

            Vector3 forward = new Vector3(0, 1, 0);
            Vector3 right = new Vector3(1, 0, 0);
            Vector3 up = new Vector3(0, 0, 1);

            public Quaternion Rotation { get; set; } = Quaternion.Identity;

            public Vector3 Forward { get { return Vector3.Transform(forward, Rotation).Round(); } }
            public Vector3 Right { get { return Vector3.Transform(right, Rotation).Round(); } }
            public Vector3 Up { get { return Vector3.Transform(up, Rotation).Round(); } }

            public Axis()
            {
            }

            public Axis(Quaternion rotation)
            {
                this.Rotation = rotation;
            }

            public Axis(Axis other)
            {
                this.Rotation = other.Rotation;
            }

            public Axis RotateLeft()
            {
                return new Axis(Rotation * Quaternion.CreateFromAxisAngle(forward, -PiOver2));
            }

            public Axis RotateRight()
            {
                return new Axis(Rotation * Quaternion.CreateFromAxisAngle(forward, PiOver2));
            }

            public Axis RotateForwad()
            {
                return new Axis(Rotation * Quaternion.CreateFromAxisAngle(right, -PiOver2));
            }

            public Axis RotateBack()
            {
                return new Axis(Rotation * Quaternion.CreateFromAxisAngle(right, PiOver2));
            }

            public Axis Invert()
            {
                return new Axis(Quaternion.Inverse(Rotation));
            }

            public Vector3 Transform(Vector3 point)
            {
                return Vector3.Transform(point, Rotation).Round();
            }

            public override string ToString()
            {
                return Forward.ToString() + " " + Right.ToString() + " " + Up.ToString();
            }
        }

        Dictionary<Vector3, (int Col, int Row)> faces = new Dictionary<Vector3, (int Col, int Row)>();
        Dictionary<(int Col, int Row), Axis> axes = new Dictionary<(int Col, int Row), Axis>();

        const int faceSize = 4;

        bool ReadFaces(Axis axis, (int Col, int Row) startPos)
        {
            char val;

            grid.TryGetValue(startPos.Col * faceSize, startPos.Row * faceSize, out val);

            if (val == ' ')
                return false;

            if (faces.ContainsValue(startPos))
                return false;

            faces[axis.Up] = startPos;
            axes[startPos] = axis;

            ReadFaces(axis.RotateLeft(), (startPos.Col - 1, startPos.Row));
            ReadFaces(axis.RotateRight(), (startPos.Col + 1, startPos.Row));
            ReadFaces(axis.RotateForwad(), (startPos.Col, startPos.Row - 1));
            ReadFaces(axis.RotateBack(), (startPos.Col, startPos.Row + 1));

            return true;
        }

        public static (int X, int Y) OffsetFromFacing(int facing)
        {
            switch (facing)
            {
                case 0:
                    return (0, 1);
                case 1:
                    return (1, 0);
                case 2:
                    return (0, -1);
                case 3:
                    return (-1, 0);
            }

            throw new Exception();
        }

        public static int FacingFromOffset((int X, int Y) offset)
        {
            if (offset.X < 0)
                return 3;

            if (offset.X > 0)
                return 1;

            if (offset.Y < 0)
                return 2;

            if (offset.Y > 0)
                return 0;

            throw new Exception();
        }

        public override long Compute2()
        {
            ReadInput(DataFileTest);

            Axis axis = new Axis();

            var bounds = grid.GetBounds();

            int width = (bounds.MaxX + 1);
            int height = (bounds.MaxY + 1);

            int maxRow = width / faceSize;
            int maxCol = height / faceSize;

            for (int row = 0; row < maxRow; row++)
            {
                for (int col = 0; col < maxCol; col++)
                {
                    if (ReadFaces(axis, (col, row)))
                        break;
                }
            }

            LongVec2 pos = new LongVec2(grid.GetAll().OrderBy(p => p.Y).ThenBy(p => p.X).First());

            int facing = 1;

            return WalkPath(pos, facing, delegate ((LongVec2 Pos, int Facing) state)
            {
                var colRow = state.Pos / faceSize;

                Axis axis = axes[((int)colRow.X, (int)colRow.Y)];

                state.Pos.AddFacing(state.Facing, 1);
                state.Pos.X = ModHelper.PosMod(state.Pos.X, width);
                state.Pos.Y = ModHelper.PosMod(state.Pos.Y, height);

                switch (state.Facing)
                {
                    case 0:
                        axis = axis.RotateForwad();
                        break;
                    case 1:
                        axis = axis.RotateRight();
                        break;
                    case 2:
                        axis = axis.RotateBack();
                        break;
                    case 3:
                        axis = axis.RotateLeft();
                        break;
                }

                var colRow2 = faces[axis.Up];

                Axis axis2 = axes[colRow2];
                (long X, long Y) gridPos = (state.Pos.X % faceSize, state.Pos.Y % faceSize);
                gridPos.Y = faceSize - gridPos.Y - 1;

                Vector3 cubePos = axis.Transform(new Vector3(gridPos.X, gridPos.Y, 0));

                Vector3 inverted = axis2.Invert().Transform(cubePos);

                LongVec2 newPos = new LongVec2((int)inverted.X, (int)inverted.Y);

                var offset = OffsetFromFacing(state.Facing);

                Vector3 cubeOffset = axis.Transform(new Vector3(offset.X, offset.Y, 0));

                var invertedOffset = axis2.Invert().Transform(cubeOffset);

                int newFacing = FacingFromOffset(((int)invertedOffset.X, (int)invertedOffset.Y));

                grid.PrintToConsole();

                return ((new LongVec2(colRow2) * faceSize) + newPos, newFacing);
            });
        }
    }
}
