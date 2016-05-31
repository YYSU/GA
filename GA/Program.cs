using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace GA
{
    class GA {
        public GA(int size,String Key) {
            this.key = Key;
            PopulationSize = new String[size];
            randomPopulation(value, PopulationSize);
        }
        //金鑰
        String key;
        public Random value = new Random();
        public String[] PopulationSize;
        //parent
        String[] chromosome = new String[2];
        public void randomPopulation(Random value, String[] PopulationSize) {
            for (int i = 0; i < PopulationSize.Length; i++) {
                //隨機產生值
                PopulationSize[i] = fill(Convert.ToString(value.Next(0, 65536), 2)); 
            }
        }

        //2進位要補滿的位置
        static String fill(String str)
        {
            if (str.Length < 16) {
                int less = 16 - str.Length;
                for (int i = 0; i < less; i++) {
                    str = "0" + str;
                }
            }
            return str;
        }
        static int fitness(String Key, String str)
        {
            int sum = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (Key[i] == str[i])
                {
                    sum++;
                }
            }
            return sum;
        }
        //競爭法,還沒寫
        public void Tournament() {
            Console.WriteLine("{0}   {1}", chromosome[0], chromosome[1]);
        }
        //輪盤法
        public String[] Roulette_Wheel() {
            //0 ~ 16
            bool[] fitness_array = new bool[17];
            int Total = 0;
            ArrayList arraylist = new ArrayList();
            //bool 陣列初值設成 False
            for (int i = 0; i < fitness_array.Length; i++) {
                fitness_array[i] = false;
            }
            //有出現過的fitness 就改成 true
            for (int i = 0; i < PopulationSize.Length; i++) {
                fitness_array[fitness(key, PopulationSize[i])] = true;
            }
            //把出現過的fitness 加入 arraylist
            for (int i = 0; i < fitness_array.Length; i++) {
                if (fitness_array[i] == true) {
                    arraylist.Add(i);
                    Total += i;
                }
            }
            int[] parent ={value.Next(0, Total + 1), value.Next(0, Total + 1)};
            for(int i = 0; i < 2; i++) {
                int wherefitness = 0;
                for(int j = 0; j < arraylist.Count; j++) {
                    wherefitness += (int)arraylist[j];
                    if (parent[i] <= wherefitness) {
                        parent[i] = (int)arraylist[j];
                        break;
                    }
                }
            }
            //Console.WriteLine("{0}   {1}", parent[0],parent[1]);
            // 挑選parent
            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < PopulationSize.Length; j++) {
                    if (fitness(key, PopulationSize[j]) == parent[i]) {
                        chromosome[i] = PopulationSize[j];
                    }
                }
            }
            //Console.WriteLine("{0}   {1}", chromosome[0], chromosome[1]);
            return chromosome;
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            // 隨機產生key值
            Random Value = new Random();
            int KeyValue = Value.Next(0, 65536);
            String key = fill(Convert.ToString(KeyValue, 2));
            

            //人口數亂數產生
            GA ga = new GA(100, key);
            ga.Roulette_Wheel();

            int RunTime = 50;


            //key值 解答
            Console.WriteLine("Key值是: {0}", key);
       
        
            

            Console.Read();
        }

        //2進位要補滿的位置
        static String fill(String str)
        {
            if (str.Length < 16) {
                int less = 16 - str.Length;
                for (int i = 0; i < less; i++) {
                    str = "0" + str;
                }
            }
            return str;
        }
        static int fitness(String Key, String str)
        {
            int sum = 0;
            for (int i = 0; i < str.Length; i++) {
                if (Key[i] == str[i]) {
                    sum++;
                }
            }
            return sum;
        } 

    }


}
