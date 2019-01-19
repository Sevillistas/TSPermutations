using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSPermutations
{
    class Program
    {
        public static int[] Factorials = { 1, 2, 6, 24, 120, 720, 5040, 40320, 362880, 3628800 };

        public static List<string> permutations = new List<string>();

        public static int LengthOfPerm;

        public const int N = 720; //from Factorials[]

        public static string PERM = "123456";

        public static List<List<int>> Costs = new List<List<int>>(Factorials[5]); //size of Matrix of Travelling costs
        static void FindPermutations(string word, List<string> p)
        {
            byte[] array = Encoding.Default.GetBytes(word.ToCharArray());
            array = array.OrderBy(x => x).ToArray();
            while (true)
            {
                int i = NarayanaNextPerm(array);
                if (i == 0)
                {
                    break;
                }
                p.Add(Encoding.Default.GetString(array));
            }
        }

        public static void Initialize()
        {
            for (int i = 0; i < N; i++)
            {
                Costs.Add(new List<int>());
                for (int j = 0; j < N; j++)
                {
                    Costs[i].Add(0);
                }
            }
        }
        static int FindWeight()
        {
            for (int i = 0; i < permutations.Count; i++)
            {
                for (int j = 0; j < permutations.Count; j++)
                {
                    for (int l = 0; l < LengthOfPerm; l++)
                    {
                        if (permutations[j].StartsWith(permutations[i].Substring(l)))
                        {
                            Costs[i][j] = l;
                        }
                    }
                }
            }
            return 0;
        }

        public static int FindShortestWay()
        {
            List<int> l = new List<int>();
            List<int> indexes = new List<int>();
            int way = 0;
            Console.WriteLine();
            for (int i = 0; i < Costs.Count; i++)
            {
                int costWay = 0;
                if (i == 0)
                {
                    IndexOfMin(Costs[0], indexes);
                    costWay = Min(Costs[indexes[0]]);
                    way += costWay;
                    PERM += permutations[indexes[0]].Substring((permutations[indexes[0]].Length) - costWay);
                    Costs[0][indexes[i]] = 0;
                }
                else
                {
                    IndexOfMin(Costs[indexes[i - 1]], indexes);
                    costWay = Min(Costs[indexes[i - 1]]);
                    way += costWay;
                    PERM += permutations[indexes[i]].Substring((permutations[indexes[i]].Length) - costWay);
                }
                for (int j = 0; j < Costs.Count; j++)
                {
                    if (i == 0)
                    {
                        Costs[j][i] = 0;
                    }
                    else
                    {
                        Costs[j][indexes[i - 1]] = 0;
                    }
                }
                //Console.WriteLine(permutations[indexes[i]]);
                //Console.WriteLine();
                //PrintCosts();
                //Console.WriteLine();
            }
            return way;
        }

        public static int Min(List<int> list)
        {
            int min = list.Max();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] <= min && list[i] > 0)
                {
                    min = list[i];
                }
            }
            return min;
        }

        public static int IndexOfMin(List<int> list, List<int> indexes)
        {
            List<int> ind = new List<int>();
            indexes.Add(list.IndexOf(Min(list)));
            return indexes[0];
        }

        public static void PrintCosts()
        {
            for (int i = 0; i < Costs.Count; i++)
            {
                for (int j = 0; j < Costs.Count; j++)
                {
                    Console.Write(Costs[i][j]);
                }
                Console.WriteLine();
            }
        }

        static int NarayanaNextPerm(byte[] a)
        {
            int i, k, t;
            byte tmp;
            int n = a.Length;
            for (k = n - 2; (k >= 0) && (a[k] >= a[k + 1]); k--) ;
            if (k == -1)
                return 0;
            for (t = n - 1; (a[k] >= a[t]) && (t >= k + 1); t--) ;
            tmp = a[k]; a[k] = a[t]; a[t] = tmp;
            for (i = k + 1; i <= (n + k) / 2; i++)
            {
                t = n + k - i;
                tmp = a[i]; a[i] = a[t]; a[t] = tmp;
            }
            return i;
        }
        static void Main(string[] args)
        {
            Initialize();
            LengthOfPerm = PERM.Length;
            permutations.Add(PERM);
            Console.WriteLine("Начальная перестановка: "+PERM);
            FindPermutations(PERM, permutations);
            FindWeight();
            Stopwatch clock = new Stopwatch();
            clock.Start();
            Console.WriteLine("Длина кратчайшей перестановки: {0}", (FindShortestWay()+LengthOfPerm).ToString());
            clock.Stop();
            Console.WriteLine("\nСуперперестановка: " + PERM + "\n\nЗатраченное время: {0}", clock.Elapsed);
            Console.ReadLine();
        }
    }
}
