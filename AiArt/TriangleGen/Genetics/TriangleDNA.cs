using GeneticAlgo;
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
        public static int mutationIntensity = 30;

        /// <summary>
        /// Indicies of the position of each vertex
        /// </summary>
        public (Point, Point, Point) Verticies { get; set; }
        public Color Color { get; set; }

        public TriangleDNA((Point, Point, Point) verticies)
        {
            Verticies = verticies;
        }

        /// <summary>
        /// Triangle will only mutate it's own color
        /// </summary>
        public void Mutate(Random random, float mutationRate)
        {
            if (random.NextDouble()*1.5 <= mutationRate)
            {
                Color = Color.FromArgb(
                    Clamp(Color.R + random.Next(-mutationIntensity, mutationIntensity)),
                    Clamp(Color.G + random.Next(-mutationIntensity, mutationIntensity)),
                    Clamp(Color.B + random.Next(-mutationIntensity, mutationIntensity)));
            }

            static int Clamp(int x)
            {
                if (x <= 0) return 0;
                if (x >= 255) return 255;
                return x;
            }
        }

        public void CrossoverFrom(Random random, TriangleDNA other)
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
