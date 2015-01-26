﻿using System;
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
        private Point[] coords;

        private bool generateRandom = true;

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
            coords = new Point[coordinates.Count()];

            for (int i = 0; i < coordinates.Count(); i++)
            {
                //double value = Convert.ToDouble(coordinates[i][1]);

                int xCoord = (int)Convert.ToDouble(coordinates[i][1]);
                int yCoord = (int)Convert.ToDouble(coordinates[i][2]);
                coords[i] = new Point(xCoord, yCoord);

                //xCoords[i] = Convert.ToDouble(coordinates[i][1]);
                //yCoords[i] = Convert.ToDouble(coordinates[i][2]);
                //xCoords[i] = double.Parse(coordinates[i][1], CultureInfo.InvariantCulture);
                //xCoords[i] = double.Parse(coordinates[i][2], CultureInfo.InvariantCulture);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Brush brush = new SolidBrush(Color.Red);
            Brush fontBrush = new SolidBrush(Color.Black);
            Font drawFont = new Font("Arial", 20);


            int recSize = 20;

            for (int i = 0; i < coords.Length; i++)
            {
                Point stringPoint = new Point(coords[i].X + recSize, coords[i].Y - (int)drawFont.SizeInPoints);
                g.FillRectangle(brush, (float)coords[i].X - (recSize / 2), (float)coords[i].Y - (recSize / 2), recSize, recSize);
                g.DrawString((i+1).ToString(), drawFont, fontBrush, stringPoint);
            }              
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Pen pen = new Pen(Color.Black);

            Point[] rndList = new Point[coords.Length];

            if(generateRandom)
            {
                Random rnd = new Random();

                for (int i = 0; i < rndList.Length; ++i)
                {
                    int newOrder = rnd.Next(coords.Length);
                    rndList[i] = coords[newOrder];
                }
             

            }
            // points müssen dann noch sortiert weden
            g.DrawPolygon(pen, coords);
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
