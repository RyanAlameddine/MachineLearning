using GeneticAlgo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TriangleGen.Genetics;

namespace TriangleGen
{
    public partial class TriangleGenForm : Form
    {
        const int populationCount = 1;
        const float mutationRate = .01f;
        const float top = .8f;
        const float bottom = .1f;

        GeneticPopulation<TriangleMeshDNA> population;

        public TriangleGenForm()
        {
            InitializeComponent();
            timer.Start();
        }

        bool running = false;
        private void RunButton_Click(object sender, EventArgs e)
        {
            running = !running;
        }

        DirectBitmap sourceImage;
        private void OpenImageButton_Click(object sender, EventArgs e)
        {
            var result = OpenFileDialog.ShowDialog();
            if (result != DialogResult.OK && result != DialogResult.Yes) return;

            var stream = OpenFileDialog.OpenFile();


            RunButton.Enabled = true;
            var img = Image.FromStream(stream);
            float multiplier = 100f / img.Height;
            ImagePreview.Image = ResizeImage(img, (int) (img.Width * multiplier), 100);
            sourceImage = new DirectBitmap(new Bitmap(ImagePreview.Image));

            int picWidth = ImagePreview.Image.Width;
            int picHeight = ImagePreview.Image.Height;

            population = new GeneticPopulation<TriangleMeshDNA>(populationCount,
                () => new TriangleMeshDNA(picWidth, picHeight, 1, 1));
            population.Randomize(new Random());
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out var r))
            {
                GeneratedImagePreview.Image = population.Population[r].GenerateImage();
            }
            else
            {
                GeneratedImagePreview.Image = population.Population[population.Population.Length-1].GenerateImage();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (running)
            {

                Random random = new Random();

                int count = 1;
                for (int i = 0; i < count; i++)
                {
                    if (split)
                    {
                        Split();
                    }

                    double fitness = double.MinValue;
                    population.RunGeneration(random,
                        (dna) =>
                        {
                            double f = dna.CalculateFitness(sourceImage, dna.GenerateImage());
                            if (f > fitness) fitness = f;
                            return f;
                        }, mutationRate, top, bottom);

                    Debug.WriteLine($"{population.Generation} => {fitness}");
                    textBox1_TextChanged(this, null);
                }
            }
        }

        private void Split()
        {
            split = false;
            foreach(var pop in population.Population)
            {
                pop.Split();
            }
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        bool split = false;
        private void SplitButton_Click(object sender, EventArgs e)
        {
            split = true;
        }
    }
}
