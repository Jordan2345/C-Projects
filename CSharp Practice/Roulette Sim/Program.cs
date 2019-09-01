using System;
using System.IO;
namespace Roulette
{
    class Program
    {
        private static int[] numOccur = new int[39];
        private static string[] color = new string[39];
        private static int numRolls = 50000;
        public static void Main(string[] args)
        {
            Initialize();
            Roll();
            PrintResults();
            Console.ReadLine();
        }

        private static void Initialize()
        {
            for(int i=0;i<39;i++)
            {
                if (i == 0 || i == 1)
                    color[i] = "G";
                else if (i % 2 == 0)
                    color[i] = "B";
                else
                    color[i] = "R";
            }
        }
        private static void Roll()
        {
            for(int i= 0;i<numRolls;i++)
            {
                Random rand = new Random();
                int num = rand.Next(1, 40);
                //specials cases
                if(num==38)
                {
                    //This is green 00
                    numOccur[0]++;
                }
                else if(num==39)
                {
                    //This is green 0
                    numOccur[1]++;
                }
                else
                {
                    numOccur[num + 1]++;
                }
            }

        }
        private static void PrintResults()
        {
            int blackCount = 0;
            int redCount = 0;
            int greenCount = 0;
            string pathForGreen = @"c:\Users\Jordan23\Desktop\Programs\Python Files\Practice\RouletteDataGreen.txt";
            string path = @"c:\Users\Jordan23\Desktop\Programs\Python Files\Practice\RouletteData.txt";

            File.WriteAllText(pathForGreen, String.Empty);
            File.WriteAllText(path, String.Empty);

            for (int i = 0;i<numOccur.Length;i++)
            {
                //make sure there are no nulls and if there are, assign it a value of 0
                if (numOccur[i].Equals(null))
                    numOccur[i] = 0;
            }
            //first print out the special 0 cases
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("The number of {0} 00's rolled is:  {1}\t----------\t{2}%", color[0],numOccur[0],(double)(numOccur[0])/numRolls*100);
            Console.WriteLine("The number of {0} 0's rolled is:  {1}\t----------\t{2}%", color[1], numOccur[1], (double)(numOccur[1]) / numRolls * 100);
            greenCount = numOccur[0] + numOccur[1];

                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(pathForGreen))
                {
                    sw.WriteLine("-1,"+numOccur[0]);
                    sw.WriteLine("0,"+numOccur[1]);
                }
            //Continue on with the rest of the cases
            for (int i = 2; i<numOccur.Length;i++)
            {
                
                string colorName = color[i];
                if (colorName.Equals("R"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    redCount += numOccur[i];
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    blackCount += numOccur[i];
                }
                Console.WriteLine("The number of {0} {1}'s rolled is:  {2}\t----------\t{3}%",color[i],i-1,numOccur[i],(double)(numOccur[i])/numRolls*100);

                if(!File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(i - 1 + "," + numOccur[i]);

                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine(i - 1 + "," + numOccur[i]);
                    }
                }
                
            }
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("------------------------------------------\nThe total number of rolls were:  {0}\t----------\t{1}%", numRolls,(double)(numRolls)/numRolls*100);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("------------------------------------------\nThe total number of blacks rolled is:  {0}\t----------\t{1}%", blackCount,(double)(blackCount)/numRolls*100);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("------------------------------------------\nThe total number of reds rolled is:  {0}\t----------\t{1}%", redCount,(double)(redCount)/numRolls*100);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("------------------------------------------\nThe total number of greens rolled is:  {0}\t----------\t{1}%", greenCount,(double)(greenCount)/numRolls*100);

            FindMinAndMax();
        }
        private static void FindMinAndMax()
        {
            int min = Int32.MaxValue;
            int max = 0;
            int indexForMin= 0;
            int indexForMax = 0;
            int counter = 0;
            foreach(int num in numOccur)
            {
                if (num < min)
                {
                    min = num;
                    indexForMin = counter;
                }
                if(max<num)
                {
                    max = num;
                    indexForMax = counter;
                }

                counter++;
            }
            if(indexForMin == 0 )
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("-----------------------------\nThe least common number was the Green 00 with {0} occurences",min);
            }
            else if(indexForMin == 1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("------------------------------------------\nThe least common number was the Green 0 with {0} occurences", min);
            }
            else if(indexForMin % 2==0)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("------------------------------------------\nThe least common number was the Black {0} with {1} occurences", indexForMin - 1, min);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("------------------------------------------\nThe least common number was the Red {0} with {1} occurences", indexForMin - 1, min);
            }
            //---------------------------------------------------------------------------------------------------------------------------------------------------------
            if (indexForMax == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("-----------------------------\nThe most common number was the Green 00 with {0} occurences", max);
            }
            else if (indexForMax == 1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("------------------------------------------\nThe most common number was the Green 0 with {0} occurences", max);
            }
            else if (indexForMax % 2 == 0)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("------------------------------------------\nThe most common number was the Black {0} with {1} occurences", indexForMax - 1, max);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("------------------------------------------\nThe most common number was the Red {0} with {1} occurences", indexForMax - 1, max);
            }
        }
    }
}
