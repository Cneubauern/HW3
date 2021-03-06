﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Drawing;

namespace HW3
{
    class SimulatedAnnealing
    {
        private string filePath;
        private List<int> currentOrder = new List<int>();
        private List<int> nextOrder = new List<int>();
        private double[,] distances;
        private Random random = new Random();
        private double shortestDistance = 0;
        Dictionary<Point[], double> sortedCoords = new Dictionary<Point[], double>();
        private Form1 form1;
        int i, j = 0;
        Point[] route;
        Point nullPoint = new Point(0, 0);

        public SimulatedAnnealing(Form1 form1)
        {
            // TODO: Complete member initialization
            this.form1 = form1;
        }

        public double ShortestDistance
        {
            get
            {
                return shortestDistance;
            }
            set
            {
                shortestDistance = value;
            }
        }

        public string FilePath
        {
            get
            {
                return filePath;
            }
            set
            {
                filePath = value;
            }
        }

        public List<int> CitiesOrder
        {
            get
            {
                return currentOrder;
            }
            set
            {
                currentOrder = value;
            }
        }

        public Point[] getOrder(Point[] coords)
        {
            Point[] order = new Point[coords.Length];
            for (int i = 0; i < coords.Length; i++)
                order[i] = coords[currentOrder[i]];
            return order;
        }

        /// <summary>
        /// Load cities from the text file representing the adjacency matrix
        /// </summary>
        private void LoadCities(Point[] coords)
        {
            distances = new double[coords.Length, coords.Length];

            for (int i = 0; i < coords.Length; i++)
            {
                for (int j = 0; j < coords.Length; j++)
                {
                    distances[i, j] = computeDistance(coords[i], coords[j]);
                    //for sorting later (no edge twice and no edges from P1 to P1 etc.)
                    if ((i < j))
                    {
                        Point[] edge = { coords[i], coords[j] };
                        sortedCoords.Add(edge, distances[i, j]);
                    }
                }
                currentOrder.Add(i);
            }

            if (currentOrder.Count < 1)
                throw new Exception("No cities to order.");
        }

        private double computeDistance(Point i, Point j){
            double distance = 0;
            int dX = i.X - j.X;
            int dY = i.Y - j.Y;

            distance += Math.Sqrt((dX * dX) + (dY * dY));
            return distance;
        }

        /// <summary>
        /// Calculate the total distance which is the objective function
        /// </summary>
        /// <param name="order">A list containing the order of cities</param>
        /// <returns></returns>
        private double GetTotalDistance(List<int> order)
        {
            double distance = 0;

            for (int i = 0; i < order.Count - 1; i++)
            {
                distance += distances[order[i], order[i + 1]];
            }

            if (order.Count > 0)
            {
                distance += distances[order[order.Count - 1], 0];
            }

            return distance;
        }

        /// <summary>
        /// Get the next random arrangements of cities
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private List<int> GetNextArrangement(List<int> order)
        {
            List<int> newOrder = new List<int>();

            for (int i = 0; i < order.Count; i++)
                newOrder.Add(order[i]);

            //we will only rearrange two cities by random
            //starting point should be always zero - so zero should not be included

            int firstRandomCityIndex = random.Next(1, newOrder.Count);
            int secondRandomCityIndex = random.Next(1, newOrder.Count);

            int dummy = newOrder[firstRandomCityIndex];
            newOrder[firstRandomCityIndex] = newOrder[secondRandomCityIndex];
            newOrder[secondRandomCityIndex] = dummy;

            return newOrder;
        }

        private void sortEdges()
        {
            Dictionary<Point[],double> sorted = new Dictionary<Point[],double>();
            //List<KeyValuePair<Point[], double>> sorted = (from kv in sortedCoords orderby kv.Value select kv).ToList();
            foreach (KeyValuePair<Point[], double> author in sortedCoords.OrderBy(key => key.Value))
            {
                sorted.Add(author.Key,author.Value);
            }
            sortedCoords.Clear();
            sortedCoords = sorted;
        }

        public void addFirstEdge()
        {
            route[i] = sortedCoords.ElementAt(j).Key[0];
            i++;
            route[i] = sortedCoords.ElementAt(j).Key[1];
            i++;
            sortedCoords.Remove(sortedCoords.ElementAt(j).Key);
        }

        private void checkPoint(int i)
        {
            if ((!route.Contains(sortedCoords.ElementAt(j).Key[i])) || (sortedCoords.ElementAt(j).Key[i].Equals(nullPoint)))
            {
                route[i] = sortedCoords.ElementAt(j).Key[i];
                i++;
                sortedCoords.Remove(sortedCoords.ElementAt(j).Key);
                j = 0;
            }
            else
                sortedCoords.Remove(sortedCoords.ElementAt(j).Key);
        }

        public Point[] MyAnneal(Point[] coords)
        {
            route = new Point[coords.Length];

            LoadCities(coords);
            sortEdges();
            addFirstEdge();

            while (i<coords.Count())
            {
                if (route[i-1].Equals(sortedCoords.ElementAt(j).Key[0]))
                {
                    // if first point is last on the route: check if second point is not in the list already (add it, if it's not)
                    checkPoint(1);
                }
                else if (route[i-1].Equals(sortedCoords.ElementAt(j).Key[1]))
                {
                    // if second point is last on the route: check if first point is not in the list already (add it, if it's not)
                    checkPoint(0);
                }
                else
                    j++;
            }

            return route;
        }

        /// <summary>
        /// Annealing Process
        /// </summary>
        public void Anneal(Point[] coords)
        {
            int iteration = -1;

            double temperature = form1.getInitialTemperature();
            double deltaDistance = 0;
            double coolingRate = form1.getCoolingRate();
            double absoluteTemperature = 0.00001;

            LoadCities(coords);

            double distance = GetTotalDistance(currentOrder);

            while (temperature > absoluteTemperature)
            {
                nextOrder = GetNextArrangement(currentOrder);

                deltaDistance = GetTotalDistance(nextOrder) - distance;

                //if the new order has a smaller distance
                //or if the new order has a larger distance but satisfies Boltzman condition then accept the arrangement
                if ((deltaDistance < 0) || (distance > 0 && Math.Exp(-deltaDistance / temperature) > random.NextDouble()))
                {
                    for (int i = 0; i < nextOrder.Count; i++)
                        currentOrder[i] = nextOrder[i];

                    distance = deltaDistance + distance;
                }

                //cool down the temperature
                temperature *= coolingRate;

                iteration++;
            }

            shortestDistance = distance;
        }

    }
}
