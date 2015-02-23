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
        private Form1 form1;

        double crossover = 0.7;
        double mutation;
        int populationSize;
        int generations;
        int genomeSize;

        public GA_PMX(Point[] coords, Form1 form1)
        {
            cityList = coords;
            this.form1 = form1;
           
        }

        public double[] getShortestPath(int[] pointList)
        {
            double[] l = null;
            crossover = form1.getCrossover();
            populationSize = form1.getPopultationSize();
            generations = form1.getGenerations();
            mutation = form1.getMutationRate();
            genomeSize = pointList.Length;

           


          //  GA geneticAlgorithm = new GA(crossover, mutation, populationSize, generations, genomSize);
          //  geneticAlgorithm.FitnessFunction = new GAFunction(fitnessFunction);

          //  geneticAlgorithm.Go();

            return l;
        }

        public static double fitnessFunction(int[] values)
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

        private Point[] createPMX(int[] parentOne, int[] parentTwo)
       {
            Random rng = new Random();

            int[] parent1 = (int[]) parentOne.Clone();
            int[] parent2 = (int[]) parentTwo.Clone();

            int range = (int)(crossover*genomeSize);

            int cut1 = (genomeSize - range) / 2; 
            int cut2 = (genomeSize + range) / 2;


            int[] mainMappingSection1 = new int[range];
            int[] mainMappingSection2 = new int[range];

            Array.Copy(parent1, cut1, mainMappingSection1, 0, range);
            Array.Copy(parent2, cut1, mainMappingSection2, 0, range);

            int[] offspring1 = new int[parent1.Length];
            int[] offspring2 = new int[parent2.Length];

            Array.Copy(parent1, offspring1, parent1.Length);
            Array.Copy(parent2, offspring2, parent2.Length);

            Array.Copy(mainMappingSection1, 0, offspring2, cut1, range);
            Array.Copy(mainMappingSection2, 0, offspring1, cut1, range);

            for (int i = 0; i < parent1.Length; ++i)
            {
                if (i < cut1 || i > cut2 )
                {
                    int numberPos = Array.IndexOf(mainMappingSection2, offspring1[i]);
                    
                    if (numberPos >= 0)
                    {
                        offspring1[i] = mainMappingSection1[numberPos];
                    }
                }
            }

            for (int i = 0; i < parent2.Length; ++i)
            {
                    if (i < cut1 || i > cut2)
                {
                    int numberPos = Array.IndexOf(mainMappingSection1, offspring2[i]);

                    if (numberPos >= 0)
                    {
                        offspring2[i] = mainMappingSection2[numberPos];
                    }
                }
            }

            double distParent1 = fitnessFunction(parent1);
            double distParent2 = fitnessFunction(parent2);
            double distOffspring1 = fitnessFunction(offspring1);
            double distOffspring2 = fitnessFunction(offspring2);

            return null;
        }
        
    }
}
