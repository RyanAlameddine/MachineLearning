using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleGen.Genetics
{
    class TriangleMeshDNA : IDNA<TriangleMeshDNA>
    {
        const int mutationIntensity = 3;

        readonly TriangleDNA[] triangles;
        readonly Point[,] points;

        /// <summary>
        /// Generates a mesh of triangles which spans an image of width/height.
        /// The distance between verticies is equal to width/horizontalSlices
        /// and the distance between horizontal verts is equal to height/verticalSlices.
        /// </summary>
        public TriangleMeshDNA(int width, int height, int horizontalSlices, int verticalSlices)
        {
            points = new Point[horizontalSlices, verticalSlices];

            //Divides the image into a grid and places a vertex at the intersection between each slice
            int sliceWidth = width / horizontalSlices;
            int sliceHeight = height / verticalSlices;
            for (int x = 0; x < horizontalSlices; x++)
            {
                for (int y = 0; y < verticalSlices; y++)
                {
                    //Register the new point
                    points[x, y] = new Point(x * sliceWidth, y * sliceHeight);
                }
            }

            LinkedList<TriangleDNA> tris = new LinkedList<TriangleDNA>();
            //Generate two tris from each vertex at the bottom-left corner of the grid quad
            for (int x = 0; x < points.GetLength(0) - 1; x++)
            {
                for (int y = 0; y < points.GetLength(1) - 1; y++)
                {
                    tris.AddLast(new TriangleDNA((new IntPoint(x, y), new IntPoint(x + 1, y), new IntPoint(x + 1, y + 1))));
                    tris.AddLast(new TriangleDNA((new IntPoint(x, y), new IntPoint(x, y + 1), new IntPoint(x + 1, y + 1))));
                }
            }
            triangles = tris.ToArray();
        }

        public void Crossover(Random random, TriangleMeshDNA other)
        {
            int split1 = random.Next(0, triangles.Length);
            int split2 = random.Next(0, triangles.Length);

            for (int i = Math.Min(split1, split2); i < Math.Max(split1, split2); i++)
            {
                //Copy the triangle and the points
                triangles[i] = other.triangles[i].Copy();
                CopyPoint(triangles[i].Verticies.Item1);
                CopyPoint(triangles[i].Verticies.Item2);
                CopyPoint(triangles[i].Verticies.Item3);

                void CopyPoint(IntPoint indicies)
                {
                    points[indicies.X, indicies.Y] = other.points[indicies.X, indicies.Y];
                }
            }
        }

        public void Mutate(Random random, int mutationRate)
        {
            foreach (TriangleDNA triangle in triangles)
            {
                triangle.Mutate(random, mutationRate);
            }
            for (int x = 1; x < points.GetLength(0) - 1; x++)
            {
                for (int y = 1; y < points.GetLength(1) - 1; y++)
                {
                    if (random.NextDouble() < mutationRate)
                    {
                        points[x, y] = new Point(
                            points[x, y].X + random.Next(-mutationIntensity, mutationIntensity),
                            points[x, y].Y + random.Next(-mutationIntensity, mutationIntensity));
                    }
                }
            }
        }

        public void Randomize(Random random)
        {
            foreach (TriangleDNA triangle in triangles)
            {
                triangle.Randomize(random);
            }
            for (int x = 1; x < points.GetLength(0) - 1; x++)
            {
                for (int y = 1; y < points.GetLength(1) - 1; y++)
                {
                    points[x, y] = new Point(
                        points[x, y].X + random.Next(-20, 20),
                        points[x, y].Y + random.Next(-20, 20));
                }
            }
        }

        TriangleMeshDNA IDNA<TriangleMeshDNA>.Copy()
        {
            throw new NotImplementedException();
        }
    }
}
