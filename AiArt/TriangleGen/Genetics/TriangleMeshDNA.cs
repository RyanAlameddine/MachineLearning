using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticAlgo;

namespace TriangleGen.Genetics
{
    class TriangleMeshDNA : IDNA<TriangleMeshDNA>
    {
        public static int mutationIntensity = 20;
        public static bool quadsOnly = true;

        TriangleDNA[] triangles;
        Point[,] points;

        readonly int width; readonly int height;
        int horizontalSlices; 
        int verticalSlices;

        /// <summary>
        /// Generates a mesh of triangles which spans an image of width/height.
        /// The distance between verticies is equal to width/horizontalSlices
        /// and the distance between horizontal verts is equal to height/verticalSlices.
        /// </summary>
        public TriangleMeshDNA(int width, int height, int horizontalSlices, int verticalSlices)
        {
            this.width = width;
            this.height = height;
            this.horizontalSlices = horizontalSlices;
            this.verticalSlices = verticalSlices;

            Generate();
        }

        void Generate()
        {
            points = new Point[horizontalSlices + 1, verticalSlices + 1];

            //Divides the image into a grid and places a vertex at the intersection between each slice
            int sliceWidth = width / horizontalSlices;
            int sliceHeight = height / verticalSlices;
            for (int x = 0; x < horizontalSlices + 1; x++)
            {
                for (int y = 0; y < verticalSlices + 1; y++)
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
                    tris.AddLast(new TriangleDNA((new Point(x, y), new Point(x + 1, y), new Point(x + 1, y + 1))));
                    tris.AddLast(new TriangleDNA((new Point(x, y), new Point(x, y + 1), new Point(x + 1, y + 1))));
                }
            }
            triangles = tris.ToArray();
        }

        /// <summary>
        /// Splits the current set of tris into four times the amount of tris
        /// </summary>
        public void Split()
        {
            horizontalSlices *= 2;
            verticalSlices *= 2;
            Point[,] newPoints = new Point[horizontalSlices + 1, verticalSlices + 1];

            //Generate new points from matching or averaging old points
            int oX = 0;
            int oY = 0;
            for (int x = 0; x < horizontalSlices + 1; x++)
            {
                for (int y = 0; y < verticalSlices + 1; y++)
                {
                    bool oYB = oY % 2 == 0;
                    bool oXB = oX % 2 == 0;

                    if (oYB && oXB)
                    {
                        //existing point
                        newPoints[x, y] = points[x / 2, y / 2];
                    }
                    else if (oXB)
                    {
                        //Register the new point in the middle of a vertical line
                        newPoints[x, y] = Average(points[x / 2, y / 2], points[x / 2, y / 2 + 1]);
                    }
                    else if (oYB)
                    {
                        //Register the new point in the middle of a horizontal line
                        newPoints[x, y] = Average(points[x / 2, y / 2], points[x / 2 + 1, y / 2]);
                    }
                    else
                    {
                        //Register the new point in the middle of the adjacent four points
                        newPoints[x, y] = Average(Average(points[x / 2, y / 2], points[x / 2, y / 2 + 1]), Average(points[x / 2 + 1, y / 2], points[x / 2 + 1, y / 2 + 1]));
                    }
                    oY++;
                }
                oX++;
                oY++;
            }

            LinkedList<TriangleDNA> tris = new LinkedList<TriangleDNA>();
            int i = 0;

            //Recursively split tris into quads
            GenQuad(2, horizontalSlices/2, 0, 0, tris);

            void GenQuad(int s, int width, int x, int y, LinkedList<TriangleDNA> tris)
            {
                if(s >= verticalSlices)
                {
                    GenTris(ref i, x, y, tris);
                    return;
                }
                int newS = s * 2;
                GenQuad(newS, width / 2, x, y, tris);
                GenQuad(newS, width / 2, x, y + width, tris);
                GenQuad(newS, width / 2, x + width, y, tris);
                GenQuad(newS, width / 2, x + width, y + width, tris);
            }

            points = newPoints;
            triangles = tris.ToArray();
        }

        void GenTris(ref int i, int x, int y, LinkedList<TriangleDNA> tris)
        {
            Color c1;
            Color c2;

            int tIndex = i;
            int bIndex = i + 1;
            c1 = triangles[tIndex].Color;
            c2 = triangles[bIndex].Color;
            if (quadsOnly)
            {
                c2 = c1;
            }

            //tIndex+= 2;
            //bIndex++;
            //c1 = Color.FromArgb((int) (255 * ((float) tIndex/triangles.Length)), (int) (255 * ((float) tIndex / triangles.Length)), (int) (255 * ((float) tIndex / triangles.Length)));
            //c2 = Color.FromArgb((int) (255 * ((float) bIndex / triangles.Length)), (int) (255 * ((float)bIndex / triangles.Length)), (int) (255 * ((float) bIndex / triangles.Length)));

            //top left
            tris.AddLast(new TriangleDNA((new Point(x, y), new Point(x + 1, y), new Point(x + 1, y + 1))));
            tris.Last.Value.Color = c1;
            tris.AddLast(new TriangleDNA((new Point(x, y), new Point(x, y + 1), new Point(x + 1, y + 1))));
            tris.Last.Value.Color = c2;

            //bottom left
            tris.AddLast(new TriangleDNA((new Point(x, y + 1), new Point(x + 1, y + 1), new Point(x + 1, y + 2))));
            tris.Last.Value.Color = c2;
            tris.AddLast(new TriangleDNA((new Point(x, y + 1), new Point(x, y + 2), new Point(x + 1, y + 2))));
            tris.Last.Value.Color = c2;

            //top right
            tris.AddLast(new TriangleDNA((new Point(x + 1, y), new Point(x + 2, y), new Point(x + 2, y + 1))));
            tris.Last.Value.Color = c1;
            tris.AddLast(new TriangleDNA((new Point(x + 1, y), new Point(x + 1, y + 1), new Point(x + 2, y + 1))));
            tris.Last.Value.Color = c1;

            //bottom right
            tris.AddLast(new TriangleDNA((new Point(x + 1, y + 1), new Point(x + 2, y + 1), new Point(x + 2, y + 2))));
            tris.Last.Value.Color = c1;
            tris.AddLast(new TriangleDNA((new Point(x + 1, y + 1), new Point(x + 1, y + 2), new Point(x + 2, y + 2))));
            tris.Last.Value.Color = c2;
            i += 2;
        }

        Point Average(Point first, Point second)
        {
            return new Point((first.X + second.X) / 2, (first.Y + second.Y) / 2);
        }

        public void CrossoverFrom(Random random, TriangleMeshDNA other)
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

                void CopyPoint(Point indicies)
                {
                    points[indicies.X, indicies.Y] = other.points[indicies.X, indicies.Y];
                }
            }
        }

        public void Mutate(Random random, float mutationRate)
        {
            for(int i = 0; i < triangles.Length; i++)
            {
                triangles[i].Mutate(random, mutationRate);
                if(quadsOnly && i % 2 == 1)
                {
                    triangles[i].Color = triangles[i - 1].Color;
                }
            }
            for (int x = 0; x < points.GetLength(0); x++)
            {
                for (int y = 0; y < points.GetLength(1); y++)
                {
                    if (random.NextDouble() < mutationRate)
                    {
                        points[x, y] = new Point(
                            points[x, y].X + random.Next(-mutationIntensity, mutationIntensity),
                            points[x, y].Y + random.Next(-mutationIntensity, mutationIntensity));
                    }
                    //Snap edges to edge
                    if (x == 0)
                    {
                        points[x, y] = new Point(0, points[x, y].Y);
                    }
                    else
                    {
                        if (x == points.GetLength(0) - 1)
                        {
                            points[x, y] = new Point(width, points[x, y].Y);
                        }
                        //Clamp points to surroundings
                        if(points[x, y].X < points[x - 1, y].X)
                        {
                            points[x, y].X = points[x - 1, y].X;
                        }
                    }
                    if (y == 0)
                    {
                        points[x, y] = new Point(points[x, y].X, 0);
                    }
                    else
                    {
                        if (y == points.GetLength(1) - 1)
                        {
                            points[x, y] = new Point(points[x, y].X, height);
                        }
                        //Clamp points to surroundings
                        if (points[x, y].Y < points[x, y - 1].Y)
                        {
                            points[x, y].Y = points[x, y - 1].Y;
                        }
                    }
                    if(x != 0 && y != 0)
                    {
                        if(points[x, y].X < points[x - 1, y - 1].X)
                        {
                            points[x, y].X = points[x - 1, y - 1].X;
                        }
                        if (points[x, y].Y < points[x - 1, y - 1].Y)
                        {
                            points[x, y].Y = points[x - 1, y - 1].Y;
                        }
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

        public Bitmap GenerateImage()
        {
            return GenerateImage(width, height);
        }

        public Bitmap GenerateImage(int width, int height)
        {

            var image = new Bitmap(width, height);
            using Graphics gfx = Graphics.FromImage(image);
            //gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.;
            foreach (var triangle in triangles)
            {
                (Point one, Point two, Point three) = triangle.Verticies;
                gfx.FillPolygon(new SolidBrush(triangle.Color), new Point[] { i2V(one), i2V(two), i2V(three) });
            }

            //Indicies of Point to Point
            Point i2V(Point indicies)
            {
                Point p = points[indicies.X, indicies.Y];
                p = new Point(p.X * width / this.width, p.Y * height / this.height);
                return p;
            }
            return image;
        }

        public double CalculateFitness(DirectBitmap source, Bitmap generated)
        {
            double fitness = 0;
            BitmapData bmd1 = generated.LockBits(new Rectangle(0, 0, generated.Width, generated.Height), ImageLockMode.ReadOnly,
                                             PixelFormat.Format24bppRgb);
            for (int x = 0; x < source.Width; x++)
            {
                for (int y = 0; y < source.Height; y++)
                {

                    Color src = source.GetPixel(x, y);
                    Color gen = GetPixel(bmd1, x, y);
                    fitness = (fitness + ColourDistance(src, gen));
                }
            }
            generated.UnlockBits(bmd1);
            generated.Dispose();
            return -fitness;
        }
        private static unsafe Color GetPixel(BitmapData bmd, int x, int y)
        {
            byte* p = (byte*)bmd.Scan0 + y * bmd.Stride + 3 * x;
            return Color.FromArgb(p[2], p[1], p[0]);
        }

        private static double GetColorFitness(Color c1, Color c2)
        {
            double r = c1.R - c2.R;
            double g = c1.G - c2.G;
            double b = c1.B - c2.B;

            return r*r+g*g+b*b;
        }

        double ColourDistance(Color e1, Color e2)
        {
            long rmean = (e1.R + (long)e2.R) / 2;
            long r = e1.R - (long)e2.R;
            long g = e1.G - (long)e2.G;
            long b = e1.B - (long)e2.B;
            return (((512 + rmean) * r * r) >> 8) + 4 * g * g + (((767 - rmean) * b * b) >> 8);
        }
    }
}
