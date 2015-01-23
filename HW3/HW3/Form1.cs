using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Globalization;

namespace HW3
{
    public partial class Form1 : Form
    {
        private string fileData = "tsp_DeutschlandCities.txt";

        private double[] xCoords;
        private double[] yCoords;

        Graphics g;

        public Form1()
        {
            InitializeComponent();
            getSentenceIndex();
            prepareGraphics();
        }

        // store data in a dictionary
        private void getSentenceIndex()
        {
            StreamReader reader = new StreamReader(fileData);

            // read input and safe them as lower case words
            string coordsRead = reader.ReadToEnd().ToLower();
            coordsRead = coordsRead.Replace('.', ',');

            List<string[]> coordinates = coordsRead.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(part => part.Split(' ')).ToList();
            coordinates.Remove(coordinates.First());

            xCoords = new double[coordinates.Count()];
            yCoords = new double[coordinates.Count()];

            for (int i = 0; i < coordinates.Count(); i++)
            {
                xCoords[i] = Convert.ToDouble(coordinates[i][1]);
                yCoords[i] = Convert.ToDouble(coordinates[i][2]);
                //xCoords[i] = double.Parse(coordinates[i][1], CultureInfo.InvariantCulture);
                //xCoords[i] = double.Parse(coordinates[i][2], CultureInfo.InvariantCulture);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Brush brush = new SolidBrush(Color.Red);
            
            for (int i = 0; i < xCoords.Count(); i++)
            {
                g.FillRectangle(brush, (float) xCoords[i], (float) yCoords[i], 10, 10);
            }              
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Pen pen = new Pen(Color.Red);
            Point[] nodes = new Point[xCoords.Count()];

            for (int i = 0; i < xCoords.Count(); i++)
            {
                nodes[i].X = (int)xCoords[i];
                nodes[i].Y = (int)yCoords[i];
            }

            // points müssen dann noch sortiert weden
            g.DrawPolygon(pen, nodes);
        }

        private void prepareGraphics()
        {
            g = pictureBox1.CreateGraphics();
            g.ScaleTransform(.3F, .3F);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
        }
    }
}
