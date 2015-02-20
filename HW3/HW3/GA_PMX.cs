using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using btl.generic;

namespace HW3
{

    class GA_PMX
    {
        static Point[] cityList;

        public GA_PMX(Point[] coords)
        {
            cityList = coords;
        }

        public double[] getShortestPath(double[] pointList)
        {
            double[] l = null;
            double crossover = 0.7;
            double mutation = 0.25;
            int populationSize = 100;
            int generations = 1000;
            int genomSize = pointList.Length;

            GA geneticAlgorithm = new GA(crossover, mutation, populationSize, generations, genomSize);
            geneticAlgorithm.FitnessFunction = new GAFunction(fitnessFunction);

            geneticAlgorithm.Go();

            return l;
        }

        public static double fitnessFunction(double[] values)
        {
            double dist = 0;
            int dX = 0;
            int dY = 0;

            for (int i = 0; i < values.Length - 1; ++i)
            {
                dX = cityList[(int)values[i]].X - cityList[(int)(values[i + 1])].X;
                dY = cityList[(int)values[i]].Y - cityList[(int)(values[i + 1])].Y;

                dist += Math.Sqrt((dX * dX) + (dY * dY));
            }

            dX = cityList[(int)values[values.Length - 1]].X - cityList[(int)values[0]].X;
            dY = cityList[(int)values[values.Length - 1]].Y - cityList[(int)values[0]].Y;

            dist += Math.Sqrt((dX * dX) + (dY * dY));


            return dist;
        }
    }
}
