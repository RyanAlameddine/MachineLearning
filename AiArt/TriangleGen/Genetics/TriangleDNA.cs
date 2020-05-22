using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleGen.Genetics
{
    class TriangleDNA : IDNA<TriangleDNA>
    {
        readonly int mutationIntensity = 5;

        /// <summary>
        /// Indicies of the position of each vertex
        /// </summary>
        public (IntPoint, IntPoint, IntPoint) Verticies { get; private set; }
        public Color Color { get; private set; }

        public TriangleDNA((IntPoint, IntPoint, IntPoint) verticies)
        {
            Verticies = verticies;
        }

        /// <summary>
        /// Triangle will only mutate it's own color
        /// </summary>
        public void Mutate(Random random, int mutationRate)
        {
            if (random.NextDouble() <= mutationRate)
            {
                Color = Color.FromArgb(
                    Color.R + random.Next(-mutationIntensity, mutationIntensity),
                    Color.G + random.Next(-mutationIntensity, mutationIntensity),
                    Color.B + random.Next(-mutationIntensity, mutationIntensity));
            }
        }

        public void Crossover(Random random, TriangleDNA other)
        {
            Verticies = (other.Verticies.Item1, other.Verticies.Item2, other.Verticies.Item3);
            Color = other.Color;
        }

        public void Randomize(Random random)
        {
            Color = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
        }

        public TriangleDNA Copy()
        {
            return new TriangleDNA(Verticies) { Color = Color };
        }
    }
}
