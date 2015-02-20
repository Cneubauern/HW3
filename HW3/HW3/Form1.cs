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
        private Point[] coords;
        SimulatedAnnealing problem;

        private List<int> cityIndeces;

        Graphics g;

        public Form1()
        {
            cityIndeces = new List<int>();

            InitializeComponent();
            getSentenceIndex();
            prepareGraphics();
            problem = new SimulatedAnnealing(this);
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

            for (int i = 0; i < coordinates.Count() - 1; i++)
            {
                int xCoord = (int)Convert.ToDouble(coordinates[i][1]);
                int yCoord = (int)Convert.ToDouble(coordinates[i][2]);
                coords[i] = new Point(xCoord, yCoord);
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

            if (routeInput.Text != "")
                rndList = getUserRoute(routeInput.Text);
            else
                rndList = coords;

            if (radioButton1.Checked)
            {
                problem.Anneal(rndList);
                rndList = problem.getOrder(rndList);
                distanceOutputLabel.Text = problem.ShortestDistance + "";
            }
            else if (radioButton2.Checked)
            {
                rndList = problem.MyAnneal(rndList);
                computeDistance(rndList);
                
            }
            else if (radioButton3.Checked)
            {
                GA_PMX ga_PMX = new GA_PMX(coords);
                ga_PMX.getShortestPath(cityIndeces.ToArray());

            }
            else if (radioButton4.Checked)
            {
                rndList = getRandomList(rndList);
                computeDistance(rndList);
            }
                // points müssen dann noch sortiert weden
            g.DrawPolygon(pen, rndList);
        }

        private void computeDistance(Point[] list)
        {
            double dist = 0;

            for (int i = 0; i < list.Length - 1; ++i )
            {
                int dX = list[i].X - list[i + 1].X;
                int dY = list[i].Y - list[i + 1].Y;

                dist += Math.Sqrt((dX * dX) + (dY * dY));
            }

            distanceOutputLabel.Text = dist.ToString();
        }


        private Point[] getUserRoute(String text)
        {
            String tmpText = text.Replace(", ", ",");
            String[] valueList = tmpText.Split(',');
            Point[] list = new Point[valueList.Length];
            List<Point> tmpList = new List<Point>();

            for(int i = 0; i < valueList.Length; ++i)
            {
                if (Convert.ToInt32(valueList[i]) <= coords.Length)
                {
                   tmpList.Add(coords[Convert.ToInt32(valueList[i]) - 1]);
                   cityIndeces.Add(Convert.ToInt32(valueList[i]));
                }
            }

            return tmpList.ToArray();
        }

        private Point[] getRandomList(Point[] list)
        {
            Random rng = new Random();
         
            Point[] sList = (Point[])list.Clone();

            int n = list.Length;
            
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Point value = sList[k];
                sList[k] = sList[n];
                sList[n] = value;
            }

            Console.WriteLine("fertisch");
            return sList;

        }

        private void prepareGraphics()
        {
            g = pictureBox1.CreateGraphics();
            g.ScaleTransform(.3F, .3F);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            distanceOutputLabel.Text = "0";
            g.Clear(Color.White);
        }

        public double getInitialTemperature()
        {
            return Double.Parse(textBox1.Text); ;
        }

        public double getCoolingRate()
        {
            return Double.Parse(textBox2.Text);
        }
     }
}
