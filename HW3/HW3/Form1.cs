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

        public Form1()
        {
            InitializeComponent();
            getSentenceIndex();
        }

        // store data in a dictionary
        private void getSentenceIndex()
        {
            StreamReader reader = new StreamReader(fileData);

            // read input and safe them as lower case words
            string veryLongString = reader.ReadToEnd().ToLower();
            veryLongString = veryLongString.Replace('.', ',');

            List<string[]> coordinates = veryLongString.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(part => part.Split(' ')).ToList();
            xCoords = new double[coordinates.Count()];
            yCoords = new double[coordinates.Count()];

            for (int i = 1; i < coordinates.Count(); i++)
            {
                xCoords[i] = Convert.ToDouble(coordinates[i][1]);
                xCoords[i] = Convert.ToDouble(coordinates[i][2]);
                //xCoords[i] = double.Parse(coordinates[i][1], CultureInfo.InvariantCulture);
                //xCoords[i] = double.Parse(coordinates[i][2], CultureInfo.InvariantCulture);
            }
            Console.Write("fertig");
        }
    }
}
