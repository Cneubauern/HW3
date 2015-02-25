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
        private Form1 form1;
        private static Point[] cityList;
        private int[] shortestPath;

        private List<KeyValuePair<int[], double>> elitism;
        private List<KeyValuePair<int[], double>> worstPath;
        private List<List<KeyValuePair<int[], double>>> generationsList;
        private List<KeyValuePair<int[], double>> actPopulation;

        double crossover = 0.7;
        double mutation;
        int populationSize;
        int generations;
        int genomeSize;

        public GA_PMX(Point[] coords, Form1 form1)
        {
            cityList = coords;
            this.form1 = form1;

            // bester Pfad je Generation
            elitism = new List<KeyValuePair<int[], double>>();
            //schlechtester Pfad je Generation
            worstPath = new List<KeyValuePair<int[], double>>();

            // Liste mit jeder Generation
            generationsList = new List<List<KeyValuePair<int[], double>>>();
            // Liste die die aktuelle Population enthaelt
            actPopulation = new List<KeyValuePair<int[], double>>();
        }

        public Point[] getShortestPath(int[] pointList)
        {
            shortestPath = new int[pointList.Length];
            crossover = form1.getCrossover();
            populationSize = form1.getPopultationSize();
            generations = form1.getGenerations();
            mutation = form1.getMutationRate();
            genomeSize = pointList.Length;
            
            runGA(pointList);

            List<KeyValuePair<int[], double>> sortedElitism = new List<KeyValuePair<int[], double>>(elitism);
            sortedElitism.Sort(CompareValues);

            int[] shortestList = sortedElitism[0].Key;
            Point[] pList = new Point[shortestList.Length];

            for (int i = 0; i < shortestList.Length; ++i)
            {
                pList[i] = cityList[shortestList[i]];
            }

            return pList;
        }

        private void runGA(int[] pointList)
        {
            Random rng = new Random();

            // erster Schritt (Init Population): bestimmte Anzahl an zufaelligen Routen erzeugen und Laengen berechnen
            actPopulation = createRandomPopulation(pointList);
            generationsList.Add(actPopulation);

            // je nach Anzahl an generations werden die nächsten Schritte wiederholt
            for (int generation = 1; generation < generations; ++generation )
            {
                actPopulation = new List<KeyValuePair<int[],double>>();
                KeyValuePair<int[], double> actBestPath = new KeyValuePair<int[],double> (null, Double.MaxValue);
                KeyValuePair<int[], double> actWorstPath = new KeyValuePair<int[],double>(null, 0);

                // vorherige Generation
                List<KeyValuePair<int[], double>> prvsGeneration = generationsList[generation - 1];

                int populationStartPos = 0;

                // falls elitism auf true gesetzt ist, wird die Beste Route aus der vorherigen Generation übernommen
                actPopulation.Add(elitism[generation - 1]);
                actWorstPath = actPopulation[0];
                populationStartPos = 1;
                
                // zweiter Schritt (Breed): jeweils zwei Routen auswählen und aus denen zwei neue Routen bestimmen. Das so oft wiederholen, bis die Populationsgröße erreicht wird.
                for (int i = populationStartPos; i < populationSize; i += 2)
                {
                    int[] child1;
                    int[] child2;

                    int[] rndParent1 = prvsGeneration[rng.Next(populationSize)].Key;
                    int[] rndParent2 = prvsGeneration[rng.Next(populationSize)].Key;

                    createPMX(rndParent1, rndParent2, out child1, out child2);

                    double child1Fitness = fitnessFunction(child1);
                    double child2Fitness = fitnessFunction(child2);

                    KeyValuePair<int[], double> child1Pair = new KeyValuePair<int[], double>(child1, child1Fitness);
                    KeyValuePair<int[], double> child2Pair = new KeyValuePair<int[], double>(child2, child2Fitness);

                    actPopulation.Add(child1Pair);
                    checkRouteValue(child1Pair, actBestPath, actWorstPath, out actBestPath, out actWorstPath);

                    if (actPopulation.Count < populationSize)
                    {
                        actPopulation.Add(child2Pair);
                        checkRouteValue(child2Pair, actBestPath, actWorstPath, out actBestPath, out actWorstPath);
                    }
                }

                generationsList.Add(actPopulation);
                elitism.Add(actBestPath);
                worstPath.Add(actWorstPath);

                Console.WriteLine("Generation " + generation + " finished");
            }

            Console.WriteLine("");
        }

        private void checkRouteValue(KeyValuePair<int[], double> route, KeyValuePair<int[], double> bestPathIn, KeyValuePair<int[], double> worstPathIn, out KeyValuePair<int[], double> bestPath, out KeyValuePair<int[], double> worstPath)
        {
            KeyValuePair<int[], double> newBestPath = bestPathIn;
            KeyValuePair<int[], double> newWorstPath = worstPathIn;

            if (route.Value < newBestPath.Value || newBestPath.Key == null)
                newBestPath = new KeyValuePair<int[],double>(route.Key, route.Value);

            if (route.Value > newWorstPath.Value)
                newWorstPath = new KeyValuePair<int[], double>(route.Key, route.Value);

            bestPath = newBestPath;
            worstPath = newWorstPath;
        }

        private List<KeyValuePair<int[], double>> createRandomPopulation(int[] pointList)
        {
            Random rng = new Random();
            List<KeyValuePair<int[], double>> rndPopulation = new List<KeyValuePair<int[], double>>();

            // die vom Benutzer gewaehlte Route wird auch uebernommen
            double sourceFitness = fitnessFunction(pointList);
            rndPopulation.Add(new KeyValuePair<int[], double>(pointList, sourceFitness));

            KeyValuePair<int[], double> actWorstPath = new KeyValuePair<int[], double>(pointList, sourceFitness);
            KeyValuePair<int[], double> actBestPath = new KeyValuePair<int[], double>(pointList, sourceFitness);

            // erzeugt je nach Populationsgroeße entsprechend viele zufällige Routen
            for (int i = 0; i < populationSize - 1; ++i)
            {
                int n = pointList.Length;
                int[] actGene = new int[n];
                Array.Copy(pointList, actGene, n);

                while (n > 1)
                {
                    n--;
                    int k = rng.Next(n + 1);
                    int value = actGene[k];
                    actGene[k] = actGene[n];
                    actGene[n] = value;
                }

                // berechnet die Fitness (laenge der Route) und fuegt die Route sowie deren Fitness in die entsprechenden Listen ein.
                double geneFitness = fitnessFunction(actGene);
                rndPopulation.Add(new KeyValuePair<int[], double>(actGene, geneFitness));

                // überprüft ob es der zurzeit beste oder schlechteste Pfad ist.
                if (geneFitness < actBestPath.Value)
                    actBestPath = new KeyValuePair<int[], double>(actGene, geneFitness);

                if(geneFitness > actWorstPath.Value)
                    actWorstPath = new KeyValuePair<int[], double>(actGene, geneFitness);
            }

            worstPath.Add(actWorstPath);
            elitism.Add(actBestPath);

            return rndPopulation;
        }

        public static double fitnessFunction(int[] values)
        {
            double dist = 0;
            int dX = 0;
            int dY = 0;

            for (int i = 1; i < values.Length - 1; ++i)
            {
                dX = cityList[(int)values[i - 1]].X - cityList[(int)(values[i])].X;
                dY = cityList[(int)values[i - 1]].Y - cityList[(int)(values[i])].Y;

                dist += Math.Sqrt((dX * dX) + (dY * dY));
            }

            dX = cityList[(int)values[values.Length - 1]].X - cityList[(int)values[0]].X;
            dY = cityList[(int)values[values.Length - 1]].Y - cityList[(int)values[0]].Y;

            dist += Math.Sqrt((dX * dX) + (dY * dY));

            return dist;
        }

        private void createPMX(int[] parentOne, int[] parentTwo, out int[] child1, out int[] child2)
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
                if (i < cut1 || i >= cut2 )
                {
                    //int numberPos = Array.IndexOf(mainMappingSection2, offspring1[i]);
                    
                    //if (numberPos >= 0)
                    //{
                    //    int mappingValue = mainMappingSection1[numberPos];
                    //    int numberPos2 = Array.IndexOf(mainMappingSection2, mappingValue);

                    //    if (numberPos2 >= 0)
                    //        offspring1[i] = mainMappingSection1[numberPos2];
                    //    else
                    //        offspring1[i] = mappingValue;
                    //}

                    int mappingValue = offspring1[i];
                    int numberPos = Array.IndexOf(mainMappingSection2, mappingValue);
                    
                    while(numberPos >= 0)
                    {
                        mappingValue = mainMappingSection1[numberPos];
                        numberPos = Array.IndexOf(mainMappingSection2, mappingValue);
                    }

                    offspring1[i] = mappingValue;
                }
            }

            for (int i = 0; i < parent2.Length; ++i)
            {
                if (i < cut1 || i >= cut2)
                {
                    /*int numberPos = Array.IndexOf(mainMappingSection1, offspring2[i]);

                    if (numberPos >= 0)
                    {
                       // offspring2[i] = mainMappingSection2[numberPos];

                        int mappingValue = mainMappingSection2[numberPos];
                        int numberPos2 = Array.IndexOf(mainMappingSection1, mappingValue);

                        if (numberPos2 >= 0)
                            offspring2[i] = mainMappingSection2[numberPos2];
                        else
                            offspring2[i] = mappingValue;
                    }*/

                    int mappingValue = offspring2[i];
                    int numberPos = Array.IndexOf(mainMappingSection1, offspring2[i]);

                    while (numberPos >= 0)
                    {
                        mappingValue = mainMappingSection2[numberPos];
                        numberPos = Array.IndexOf(mainMappingSection1, mappingValue);
                    }

                    offspring2[i] = mappingValue;
                }
            }

            child1 = offspring1;
            child2 = offspring2;
        }

        public static int CompareValues(KeyValuePair<int[], double> a, KeyValuePair<int[], double> b)
        {
            return a.Value.CompareTo(b.Value);
        }
    }
}
