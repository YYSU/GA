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
            NewPopulation = new String[size];
            randomPopulation(value, PopulationSize);
        }
        //金鑰
        String key;
        public Random value = new Random();
        public String[] PopulationSize;
        public String[] NewPopulation;
        //parent
        String[] chromosome = new String[2];
        public String NewChromosome;
        
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
            for (int i = 0; i < str.Length; i++){
                if (Key[i] == str[i]){
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
        public void Roulette_Wheel() {
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
            int[] parent = {value.Next(0, Total + 1), value.Next(0, Total + 1)};
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
            
        }
        //交配法
        public void One_Point(double PC) {
            for (int i = 0; i < PopulationSize.Length; i++) {            
                int random = value.Next(0, 16);
                String front = chromosome[0].Substring(0, random);
                String back = chromosome[1].Substring(random);
                NewChromosome = front + back;
                int Probability = value.Next(0, 1);
                if (Probability < PC) {
                    //突變
                    NewChromosome = Mutate(NewChromosome);
                }
                NewPopulation[i] = NewChromosome;
            }
        }
        //突變
        public String Mutate(String Chromosome) {
            int random = value.Next(0, 16);
            if (Chromosome[random] == '0') {
                String fron = Chromosome.Substring(0, random);
                String bac = Chromosome.Substring(random + 1);
                Chromosome = fron + 1 + bac;
            } else {
                String fron = Chromosome.Substring(0, random);
                String bac = Chromosome.Substring(random + 1);
                Chromosome = fron + 0 + bac;
            }
            return Chromosome;
        }

        //把新人口傳回PopulationSzie
        public void Translate() {
            for (int i = 0; i < PopulationSize.Length; i++) {
                PopulationSize[i] = NewPopulation[i];
            }
        }

        //印出 每個的字串 及 適合度 
        public void Print() {
            for (int i = 0; i < PopulationSize.Length; i++) {
                Console.WriteLine("第{0}個 : {1}  , Fitness:{2}", i+1, PopulationSize[i], fitness(key, PopulationSize[i]));
            }
            Console.WriteLine();
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
            Console.WriteLine("初始化:...");
            ga.Print();
            int times = 0;
            //執行次數，突變率，交配率(?)
            int RunTime = 100;
            double PC = 0.4;
            double PM = 0.9;

            for (int i = 0; i < RunTime; i++) {
                //Console.WriteLine("這是第 {0} 次 ", times);
                ga.Roulette_Wheel();
                ga.One_Point(PC);
                ga.Translate();
                //ga.Print();
                times++;
            }
            ga.Print();

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
        
    }
}
