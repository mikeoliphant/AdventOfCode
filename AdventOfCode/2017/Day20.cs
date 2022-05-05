namespace AdventOfCode._2017
{
    internal class Day20
    {
        class Particle
        {
            public LongVec3 Position { get; set; }
            public LongVec3 Velocity { get; set; }
            public LongVec3 Acceleration { get; set; }

            public override string ToString()
            {
                return Position.ToString() + " " + Velocity.ToString() + " " + Acceleration.ToString();
            }
        }

        List<Particle> particles = new List<Particle>();

        void ReadInput()
        {
            foreach (string particleStr in File.ReadLines(@"C:\Code\AdventOfCode\Input\2017\Day20.txt"))
            {
                var match = Regex.Match(particleStr, "p=<(.*)>, v=<(.*)>, a=<(.*)>");

                LongVec3 pos = new LongVec3(match.Groups[1].Value.ToLongs(',').ToArray());
                LongVec3 vel = new LongVec3(match.Groups[2].Value.ToLongs(',').ToArray());
                LongVec3 acc = new LongVec3(match.Groups[3].Value.ToLongs(',').ToArray());

                particles.Add(new Particle { Position = pos, Velocity = vel, Acceleration = acc });
            }
        }

        public long Compute()
        {
            ReadInput();

            return particles.IndexOf(particles.OrderBy(p => p.Acceleration.ManhattanDistance(LongVec3.Zero)).First());
        }

        public long Compute2()
        {
            ReadInput();

            List<Particle> toRemove = new List<Particle>();

            do
            {
                for (int pos1 = 0; pos1 < particles.Count; pos1++)
                {
                    for (int pos2 = pos1 + 1; pos2 < particles.Count; pos2++)
                    {
                        if (particles[pos1].Position == particles[pos2].Position)
                        {
                            toRemove.Add(particles[pos1]);
                            toRemove.Add(particles[pos2]);
                        }
                    }
                }

                foreach (Particle particle in toRemove)
                {
                    particles.Remove(particle);
                }

                toRemove.Clear();

                foreach (Particle p in particles)
                {
                    p.Velocity += p.Acceleration;
                    p.Position += p.Velocity;
                }
            }
            while (true);


            return 0;
        }
    }
}
