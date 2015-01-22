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

namespace HW3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string fileData = "tsp_DeutschlandCities.txt";

        private Dictionary<string, string> coords;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            coords = new Dictionary<string, string>();

            getSentenceIndex();
        }

        // store data in a dictionary
        private void getSentenceIndex()
        {
            StreamReader reader = new StreamReader(fileData);

            // read input and safe them as lower case words
            string veryLongString = reader.ReadToEnd().ToLower();

            coords = veryLongString.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
               .Select(part => part.Split('\t'))
               .ToDictionary(split => split[0], split => split[1]);
        }
    }
}
