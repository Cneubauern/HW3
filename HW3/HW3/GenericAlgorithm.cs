using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW3
{

    class GenericAlgorithm
    {
        List<KeyValuePair<int, Point>> cityList;

        public GenericAlgorithm()
        {
            cityList = new List<KeyValuePair<int, Point>>();
        }

        public Point[] getShortestPath(Point[] list, int[] cityIndeces)
        {
            createMergedList(list, cityIndeces);
            createPMX(cityIndeces);

            return null;
        }

        private void createMergedList(Point[] list, int[] cityIndeces)
        {
            for (int i = 0; i < cityIndeces.Length; i++)
            {
                KeyValuePair<int,Point> actCity = new KeyValuePair<int,Point>(cityIndeces[i],list[i]);
                cityList.Add(actCity);
            }
        }

        private Point[] createPMX(int[] cityindeces)
        {
            Random rng = new Random();

            int[] parent1 = (int[]) cityindeces.Clone();
            int[] parent2 = getRandomList(cityindeces);

            int cut1 = rng.Next(parent1.Length + 1);
            int cut2 = rng.Next(cut1, parent1.Length);

            int range = cut2 - cut1;

            int[] mainMappingSection1 = new int[range];
            int[] mainMappingSection2 = new int[range];


            Array.Copy(parent1, cut1, mainMappingSection1, 0, range);
            Array.Copy(parent2, cut1, mainMappingSection2, 0, range);

            int[] offspring1 = new int[parent1.Length];
            int[] offspring2 = new int[parent2.Length];

            Array.Copy(parent1, offspring1, parent1.Length);
            Array.Copy(parent2, offspring2, parent2.Length);

            Array.Copy(mainMappingSection1, 0, offspring1, cut1, range);
            Array.Copy(mainMappingSection2, 0, offspring2, cut1, range);

            for (int i = 0; i < parent1.Length; ++i)
            {
                if (i < cut1 && i > cut2 )
                {
                    int numberPos = Array.IndexOf(mainMappingSection1, offspring1[i]);
                    if (numberPos >= 0)
                    {
                    }
                }
            }



                return null;
        }

        private int[] getRandomList(int[] iList)
        {
            Random rng = new Random();
            int[] list = (int[]) iList.Clone();
            
            int n = iList.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                int value = list[k];
                list[k] = list[n];
                list[n] = value;
            }  

                return list;
        }

    }

}
