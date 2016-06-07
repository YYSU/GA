using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace GA
{
    class GA {
        
        // 亂數產生初始人口
        public GA(int size, String Key) {
            this.key = Key;
            PopulationSize = new String[size];
            NewPopulation = new String[size];
            Fitness = new int[size];
            NewFitness = new int[size];
            randomPopulation(value, PopulationSize);      
        }
        // 金鑰
        String key , conserve;
        public Random value = new Random();
        public String[] PopulationSize;
        public int[] Fitness; 
        public String[] NewPopulation;
        public int[] NewFitness;
        // parent chromosome
        public String[] chromosome = new String[2];
        public String NewChromosome;

        public void randomPopulation(Random value, String[] PopulationSize) {
            for (int i = 0; i < PopulationSize.Length; i++) {
                // 隨機產生值
                PopulationSize[i] = fill(Convert.ToString(value.Next(0, 65536), 2));
            }
            for (int i = 0; i < PopulationSize.Length; i++) {
                Fitness[i] = fitness(key, PopulationSize[i]);
            }
        }

        // 2進位要補滿的位置
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

        public static void bubbleSort(String[] PopulationSize, int[] list)
        {
            int n = list.Length;
            int temp; String tempstr;
            int Flag = 1; // 旗標
            int i;
            for (i = 1; i <= n - 1 && Flag == 1; i++)
            {    // 外層迴圈控制比較回數
                Flag = 0;
                for (int j = 1; j <= n - i; j++)
                {  // 內層迴圈控制每回比較次數            
                    if (list[j] < list[j - 1])
                    {  // 比較鄰近兩個物件，右邊比左邊小時就互換。	   

                        temp = list[j];
                        tempstr = PopulationSize[j];
                        list[j] = list[j - 1];
                        PopulationSize[j] = PopulationSize[j - 1];
                        list[j - 1] = temp;
                        PopulationSize[j - 1] = tempstr;
                        Flag = 1;
                    }
                }
            }
        }

        // 輪盤法
        public void Roulette_Wheel() {
            // 排序fitness
            bubbleSort(PopulationSize, Fitness);

            //開始進行天擇，分數越高的選到機率越高
            bool[] arr = new bool[17];
            for (int i = 0; i < 17; i++) {
                arr[i] = false;
            }

            //把出現的Fitness記錄起來
            for (int i = 0; i < PopulationSize.Length; i++) {
                if (i == 0) {
                    for (int j = 0; j < 17; j++) {
                        if (Fitness[i] == j) {
                            arr[j] = true;
                            continue;
                        }
                    }
                } else {
                    if (Fitness[i] != Fitness[i-1]) {
                        for (int j = 0; j < 17; j++) {
                            if (Fitness[i] == j) {
                                arr[j] = true;
                                continue;
                            }
                        }
                    }
                }
            }

            //總數
            ArrayList list = new ArrayList();
            int total = 0;
            for (int i = 0; i < 17; i++) {
                if (arr[i] == true) {
                    total += i;
                    list.Add(i);
                }
            }

            // 開始輪盤法進行天擇
            for (int i = 0; i < PopulationSize.Length; i++) {
                // 隨便射一個
                int shoot = value.Next(0, total + 1);

                //判斷射到哪個
                int WheelProb = (int)list[0];
                int NextString = 0;
                while (WheelProb < shoot)
                {
                    NextString++;
                    WheelProb += (int)list[NextString];
                }

                //找該 Fitness 區間
                int start = 0;
                int end;
                while (start <= PopulationSize.Length - 1)
                {
                    if (Fitness[start] < (int)list[NextString])
                    {
                        start++;
                    }
                    else
                    {
                        break;
                    }
                }
                end = start;
                while (end < PopulationSize.Length)
                {
                    if (Fitness[end] == (int)list[NextString])
                    {
                        end++;
                    }
                    else
                    {
                        end--;
                        break;
                    }
                }

                conserve = PopulationSize[value.Next(start, end)];
                NewPopulation[i] = conserve;       
            }

            
            for (int i = 0; i < NewPopulation.Length; i++)
            {
                NewFitness[i] = fitness(key, NewPopulation[i]);
               // Console.WriteLine("{0}: {1}", NewFitness[i], NewPopulation[i]);
            }
           // Console.WriteLine("--------------");

            Translate();
            bubbleSort(PopulationSize, Fitness);
        }

        // 交配
        public void One_Point(double PC, double MR) {
            double MutateRate;
            for (int i = 0; i < PopulationSize.Length; i++) {
                MutateRate = value.Next(0, 1);
                // 任取人口數中兩個
                chromosome[0] = PopulationSize[value.Next(0, PopulationSize.Length)];
                chromosome[1] = PopulationSize[value.Next(0, PopulationSize.Length)];
                //chromosome[0] = Choice();
                //chromosome[1] = Choice();

                if (MutateRate < MR)
                {
                    int random = value.Next(0, 16);
                    String front = chromosome[0].Substring(0, random);
                    String back = chromosome[1].Substring(random);
                    NewChromosome = front + back;
                    int Probability = value.Next(0, 1);
                    // 突變
                    if (Probability < PC)
                        NewChromosome = Mutate(NewChromosome);
                    NewPopulation[i] = NewChromosome;
                }
                else {
                    NewChromosome = fitness(key, chromosome[0]) > fitness(key, chromosome[1]) ? chromosome[0] : chromosome[1];
                    int Probability = value.Next(0, 1);
                    // 突變
                    if (Probability < PC)
                        NewChromosome = Mutate(NewChromosome);
                    NewPopulation[i] = NewChromosome;
                }
            }

            
            for (int i = 0; i < NewPopulation.Length; i++) {
                NewFitness[i] = fitness(key, NewPopulation[i]);
            }
            
            Translate();
            bubbleSort(PopulationSize, Fitness);

            int sum = 0;
            for (int i = 0; i < PopulationSize.Length; i++) {
                sum += Fitness[i];
            }
            Console.WriteLine("平均: {0}", sum / PopulationSize.Length);
        }

        // 突變
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

        // 把新人口傳回PopulationSzie
        public void Translate() {
            for (int i = 0; i < PopulationSize.Length; i++) {
                PopulationSize[i] = NewPopulation[i];
                Fitness[i] = NewFitness[i];
            }
        }

        public String Choice()
        {
            //開始進行天擇，分數越高的選到機率越高
            bool[] arr = new bool[17];
            for (int i = 0; i < 17; i++)
            {
                arr[i] = false;
            }

            //把出現的Fitness記錄起來
            for (int i = 0; i < PopulationSize.Length; i++)
            {
                if (i == 0)
                {
                    for (int j = 0; j < 17; j++)
                    {
                        if (Fitness[i] == j)
                        {
                            arr[j] = true;
                            continue;
                        }
                    }
                }
                else
                {
                    if (Fitness[i] != Fitness[i - 1])
                    {
                        for (int j = 0; j < 17; j++)
                        {
                            if (Fitness[i] == j)
                            {
                                arr[j] = true;
                                continue;
                            }
                        }
                    }
                }
            }

            //總數
            ArrayList list = new ArrayList();
            int total = 0;
            for (int i = 0; i < 17; i++)
            {
                if (arr[i] == true)
                {
                    total += i;
                    list.Add(i);
                }
            }


            // 隨便射一個
            int shoot = value.Next(0, total + 1);

            //判斷射到哪個
            int WheelProb = (int)list[0];
            int NextString = 0;
            while (WheelProb < shoot)
            {
                NextString++;
                WheelProb += (int)list[NextString];
            }

            //找該 Fitness 區間
            int start = 0;
            int end;
            while (start <= PopulationSize.Length - 1)
            {
                if (Fitness[start] < (int)list[NextString])
                {
                    start++;
                }
                else
                {
                    break;
                }
            }
            end = start;
            while (end < PopulationSize.Length)
            {
                if (Fitness[end] == (int)list[NextString])
                {
                    end++;
                }
                else
                {
                    end--;
                    break;
                }
            }
            conserve = PopulationSize[value.Next(start, end)];
            return conserve;
        }

        // 印出 每個的字串 及 適合度 
        public void Print() {
            for (int i = 0; i < PopulationSize.Length; i++) {
                Console.WriteLine("第{0}個 : {1}  , Fitness:{2},{3}", i+1, PopulationSize[i], Fitness[i], fitness(key, PopulationSize[i]));
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
            int times = 0;

            // 人口數
            GA ga = new GA(100, key);
            
            // 執行次數，突變率,  交配率
            int RunTime = 40;
            double PC = 0.3;
            double MR = 0.9;
        
            for (int i = 0; i < RunTime; i++) {
                Console.WriteLine("這是第 {0} 次 ", times+1);
                ga.Roulette_Wheel();
                ga.One_Point(PC,MR);
                ga.Print();
                times++;
            }  
            ga.Print();

            // key值 解答
            Console.WriteLine("Key值是: {0}", key);
            Console.Read();
        }

        // 2進位要補滿的位置
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
