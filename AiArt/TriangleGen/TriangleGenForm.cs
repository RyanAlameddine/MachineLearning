using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TriangleGen.Genetics;

namespace TriangleGen
{
    public partial class TriangleGenForm : Form
    {
        public TriangleGenForm()
        {
            InitializeComponent();
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            TriangleMeshDNA dna = new TriangleMeshDNA(500, 500, 10, 10);
        }
    }
}
