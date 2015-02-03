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
            else if (randomBox.Checked)
                rndList = getRandomList(coords.Length);
            else
                rndList = coords;

            if (radioButton1.Checked)
            {
                SimulatedAnnealing problem = new SimulatedAnnealing();
                problem.Anneal(coords);
                rndList = problem.getOrder(coords);
                distanceOutputLabel.Text = problem.ShortestDistance + "";
            }
            else if (radioButton2.Checked)
            {
                
            }
            else if (radioButton3.Checked)
            {

            }
            else if (radioButton4.Checked)
            {
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

            for(int i = 0; i < valueList.Length; ++i)
            {
                if (Convert.ToInt32(valueList[i])<=coords.Length)
                list[i] = coords[Convert.ToInt32(valueList[i]) - 1];
            }


            return list;
        }

        private Point[] getRandomList(int count)
        {
            Random rnd = new Random();
            int[] numbers = new int[count];
            Point[] list = new Point[count];

            int foundNumberCount = 0;

            while(foundNumberCount < count)
            {
                int rndN = rnd.Next(coords.Length);

                if (Array.IndexOf(numbers, rndN) < 0)
                {
                    numbers[foundNumberCount] = rndN;
                    list[foundNumberCount] = coords[rndN];
                    ++foundNumberCount;

                    Console.WriteLine("act count: " + foundNumberCount);
                }

                if (foundNumberCount == count - 1)
                    break;
            }

            Console.WriteLine("fertisch");
            return list;
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
    }
}
