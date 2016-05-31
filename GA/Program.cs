using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA
{
    class GA {
        public GA(int size) {
            PopulationSize = new String[size];
            randomPopulation(value, PopulationSize);
        }
        public Random value = new Random();
        public String[] PopulationSize;
        public void randomPopulation(Random value, String[] PopulationSize) {
            for (int i = 0; i < PopulationSize.Length; i++) {
                //隨機產生值
                PopulationSize[i] = fill(Convert.ToString(value.Next(0, 65536), 2)); 
            }
        }
        //2進位要補滿的位置
        static String fill(String str)
        {
            if (str.Length < 16)
            {
                int less = 16 - str.Length;
                for (int i = 0; i < less; i++)
                {
                    str = "0" + str;
                }
            }
            return str;
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
            GA ga = new GA(100);
            int RunTime = 50;


            //key值 解答
            Console.WriteLine("{0}", key);
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
        static int fitness()
        {
            return 0;
        } 

    }


}
