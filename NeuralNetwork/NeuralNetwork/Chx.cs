using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NeuralNetwork
{
    class Chx
    {

        public double[][] Spaces;
        public double[][] Outputs;

        public double[][] TestSpaces;
        public double[][] TestOutputs;

        public Chx(string path)
        {
            string[] lines = File.ReadAllLines(path);
            int count = (int) (lines.Length * .75);
            int testCount = lines.Length - count;
            Outputs    = new double[count][];
            Spaces     = new double[count][];
            TestOutputs= new double[testCount][];
            TestSpaces = new double[testCount][];
            for (int i = 0; i < count; i++)
            {
                int x = 0;
                for (; lines[i][x] != ':'; x++) ;
                Outputs[i] = new double[] { (double.Parse(lines[i].Substring(0, x)) + 1000) / 2000 };
                x++;
                Spaces[i] = new double[33];
                int length = lines[i].Length - x;
                for (int j = 0; j < length; j++ )
                {
                    char c = lines[i][x + j];
                    if (c == 'O') Spaces[i][j] = -2;
                    if (c == 'o') Spaces[i][j] = -1;
                    if (c == ' ') Spaces[i][j] =  0;
                    if (c == 'x') Spaces[i][j] =  1;
                    if (c == 'X') Spaces[i][j] =  2;
                }
            }
            for(int i = 0; i < testCount; i++)
            {
                int x = 0;
                for (; lines[i + testCount][x] != ':'; x++) ;
                TestOutputs[i] = new double[] { (double.Parse(lines[i + testCount].Substring(0, x)) + 1000) / 2000 };
                x++;
                TestSpaces[i] = new double[33];
                int length = lines[i + testCount].Length - x;
                for (int j = 0; j < length; j++)
                {
                    char c = lines[i + testCount][x + j];
                    if (c == 'O') TestSpaces[i][j] = -2;
                    if (c == 'o') TestSpaces[i][j] = -1;
                    if (c == ' ') TestSpaces[i][j] = 0;
                    if (c == 'x') TestSpaces[i][j] = 1;
                    if (c == 'X') TestSpaces[i][j] = 2;
                }
            }
        }
    }
}

/*
 * 
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NeuralNetwork
{
    class MNIST
    {

        public double[][] Pixels =  new double[50][];
        public double[][] Outputs = new double[50][];

        public MNIST()
        {
            FileStream trainImages = new FileStream(@"MNIST\train-images.idx3-ubyte", FileMode.Open);
            FileStream trainLabels = new FileStream(@"MNIST\train-labels.idx1-ubyte", FileMode.Open);

            BinaryReader brLabels = new BinaryReader(trainLabels);
            BinaryReader brImages = new BinaryReader(trainImages);

            brImages.ReadInt32(); // discard magic
            int imageCount = brImages.ReadInt32();
            int rowCount = brImages.ReadInt32();
            int columnCount = brImages.ReadInt32();

            brLabels.ReadInt32(); //discard magic
            int labelCount = brLabels.ReadInt32();


            for (int i = 0; i < Pixels.Length; i++)
            {
                Pixels[i] = new double[9 * 9];
                for (int x = 0; x < 27; x++)
                {
                    for (int y = 0; y < 27; y++)
                    {
                        byte b = brImages.ReadByte();
                        Pixels[i][x/3 + 9 * y/3] =+ b;
                    }
                }

                byte label = brLabels.ReadByte();
                Outputs[i] = new double[10];
                Outputs[i][label] = 1;
            }

            foreach(double[] pixels in Pixels)
            {
                for(int i = 0; i < pixels.Length; i++)
                {
                    pixels[i] /= 9;
                }
            }

            trainImages.Close();
            brImages.Close();
            trainLabels.Close();
            brLabels.Close();
        }
    }
}
 
 */
