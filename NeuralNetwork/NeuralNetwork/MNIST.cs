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
                Pixels[i] = new double[28 * 28];
                for (int x = 0; x < 28; x++)
                {
                    for (int y = 0; y < 28; y++)
                    {
                        double b = brImages.ReadByte();
                        Pixels[i][x + 28 * y] = b/255;
                    }
                }

                byte label = brLabels.ReadByte();
                Outputs[i] = new double[10];
                Outputs[i][label] = 1;
            }

            //average the pixels to reduce resolution

            trainImages.Close();
            brImages.Close();
            trainLabels.Close();
            brLabels.Close();
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
