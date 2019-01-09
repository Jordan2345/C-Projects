using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolySim
{
    class Program
    {
        private static string[] buyableSpots = {"Mediterranean Avenue","Baltic Avenue","Reading Railroad","Oriental Avenue","Vermont Avenue","Connecticut Avenue","St. Charles Place","Electric Company",
                                     "States Avenue","Viginia Avenue","Pennsylvania Railroad","St. James Place","Tennessee Avenue","New York Avenue","Kentucky Avenue","Indiana Avenue","Illinois Avenue",
                                     "B&O railroad","Atlantic Avenue","Ventor Avenue","Water Works","Marvin Gardens","Pacific Avenue","North Carolina Avenue","Pennsylvania Avenue","Short line",
                                     "Park Place","Boardwalk"};
        private static string[] allSpots = {"Go","Mediterranean Avenue","Community Chest1","Baltic Avenue","Income Tax","Reading Railroad","Oriental Avenue","Chance1","Vermont Avenue","Connecticut Avenue","In Jail/Just Visiting","St. Charles Place","Electric Company",
                                     "States Avenue","Viginia Avenue","Pennsylvania Railroad","St. James Place","Community Chest2","Tennessee Avenue","New York Avenue","Free Parking","Kentucky Avenue","Chance2","Indiana Avenue","Illinois Avenue",
                                     "B&O railroad","Atlantic Avenue","Ventor Avenue","Water Works","Marvin Gardens","Go To Jail","Pacific Avenue","North Carolina Avenue","Community Chest3","Pennsylvania Avenue","Short line",
                                     "Chance3","Park Place","Luxury Tax","Boardwalk"};
        //create 28 empty strings
        private static string[] ownedSpots= Enumerable.Repeat(string.Empty, 28).ToArray();
        //create 40 ints to count the amount of times landed on a certain place
        private static int[] numTimesLanded = new int[40];
        private static int[] numCounter = new int[12];
        private static int ownedSpotsIndex = 0;
        private static int landedCounter = 0;
        private static int jailRolls = 0;
        private static Random dice = new Random();

        static void Main(string[] args)
        {
            //create a current index for allSpots
            int currentIndex = 0;
            Console.WriteLine("Currently at {0}",allSpots[currentIndex]);
            while(!GameOver())
            {
                int movement = Roll();
                //check to make sure I don't get an ArrayOutOfBounds Exception
                if(currentIndex+movement>allSpots.Length)
                {
                    int remainder = (currentIndex + movement) - 40;
                    currentIndex = remainder;
                }
                else
                {
                    currentIndex += movement;
                }
                //index 40 is 'Go'
                if (currentIndex == 40)
                    currentIndex = 0;
                else if(currentIndex==30)
                {
                    numTimesLanded[currentIndex]++;
                    landedCounter++;
                    bool inJail = true;
                    currentIndex = 10;
                    Console.WriteLine("You are in Jail");
                    while(inJail)
                    {
                        inJail = RollOutOfJail();

                    }
                }
                //Print current spot after the roll
                Console.WriteLine("After rolling a {0}, you are now at:  '{1}'",movement,allSpots[currentIndex]);
                numTimesLanded[currentIndex]++;
                landedCounter++;
                //Call method to check if it's a buyable spot or if it has already been bought
                CheckIfBuyable(currentIndex);
            }
            PrintResults();
            Console.Read();
        }
        public static int Roll()
        {
            int sumOfDice = 0;
            int die1, die2 = 0;
            die1 = dice.Next(1,7);
            die2 = dice.Next(1,7);

            if (die1 == die2)
                numCounter[11]++;
            sumOfDice = die1 + die2;

            //display sumOfDice just to check
            Console.WriteLine("Rolls a {0}\n---------------------------------------------",sumOfDice);
            AddToCounter(sumOfDice);
            return sumOfDice;
        }
        public static bool RollOutOfJail()
        {
            jailRolls++;
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Blue;
            int dice1 = dice.Next(1, 7);
            int dice2 = dice.Next(1, 7);
            Console.WriteLine("You rolled a {0} and a {1}", dice1, dice2);
            Console.ResetColor();
            int sumOfDice = 0;
            sumOfDice = dice1 + dice2;
            AddToCounter(sumOfDice);
            if (dice1 == dice2)
            {
                numCounter[11]++;
                return false;
            }

            return true;
        }
        public static void CheckIfBuyable(int index)
        {
            int spotIndex = 0;
            bool flag = false;
            bool newSpotIndex = false;
            string place = "";
            bool checkIfInBuyable = false;
            //check for the location in buyable spots
            foreach(string spotLanded in buyableSpots)
            {
                if(spotLanded.Contains(allSpots[index]))
                {
                    //if true, the spot landed on is a buyable spot
                    //get the index of the spot
                    spotIndex = Array.IndexOf(buyableSpots, allSpots[index]);
                    checkIfInBuyable = true;
                    newSpotIndex = true;
                }
            }
            if(checkIfInBuyable)
            {
                foreach (string ownedPlace in ownedSpots)
                {
                    //check if the place is already bought;
                    if (ownedPlace.Equals(buyableSpots[spotIndex]))
                    {
                        flag = true;
                        place = ownedPlace;
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("{0} is not a buyable spot",allSpots[index]);
                Console.ResetColor();
            }
            
            if(flag)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("{0} has already been bought",place);
                Console.ResetColor();
            }
            else if(!flag && newSpotIndex)
            {
                ownedSpots[ownedSpotsIndex] = buyableSpots[spotIndex];
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("You bought {0}",ownedSpots[ownedSpotsIndex]);
                Console.ResetColor();
                ownedSpotsIndex++;
            }
        }
        public static bool GameOver()
        {
            int count = 0;
            bool flag = true;
            //go and check if every spot in buyableSpots is also in ownedSpots
            foreach(string place in buyableSpots)
            {
                foreach(string owned in ownedSpots)
                {
                    if (owned.Equals(place))
                    {
                        count++;
                    }
                    continue;
                }
            }
            //if count==28, that means that all buyable spots have been bought
            if (count == 28)
                flag = true;
            else
                flag = false;
            return flag;
        }
        public static void PrintResults()
        {
            int total = 0;
            foreach (int count in numCounter)
                total += count;
            Console.WriteLine("\n-----------------------------------------------\nNumber of 2: {0}/{1} = %{2}", numCounter[0], total, 100 * (double)numCounter[0] / total);
            Console.WriteLine("Number of 3: {0}/{1} = %{2}", numCounter[1], total, 100 * (double)numCounter[1] / total);
            Console.WriteLine("Number of 4: {0}/{1} = %{2}", numCounter[2], total, 100 * (double)numCounter[2] / total);
            Console.WriteLine("Number of 5: {0}/{1} = %{2}", numCounter[3], total, 100 * (double)numCounter[3] / total);
            Console.WriteLine("Number of 6: {0}/{1} = %{2}", numCounter[4], total, 100 * (double)numCounter[4] / total);
            Console.WriteLine("Number of 7: {0}/{1} = %{2}", numCounter[5], total, 100 * (double)numCounter[5] / total);
            Console.WriteLine("Number of 8: {0}/{1} = %{2}", numCounter[6], total, 100 * (double)numCounter[6] / total);
            Console.WriteLine("Number of 9: {0}/{1} = %{2}", numCounter[7], total, 100 * (double)numCounter[7] / total);
            Console.WriteLine("Number of 10: {0}/{1} = %{2}", numCounter[8], total, 100*(double)numCounter[8] / total);
            Console.WriteLine("Number of 11: {0}/{1} = %{2}", numCounter[9], total, 100*(double)numCounter[9] / total);
            Console.WriteLine("Number of 12: {0}/{1} = %{2}", numCounter[10], total, 100*(double)numCounter[10] / total);
            Console.WriteLine("Number of doubles:  {0}/{1} = %{2}\n-----------------------------------------------\n", numCounter[11], total,100*(double)numCounter[11] / total);
            foreach(string place in ownedSpots)
            {
                
                Console.WriteLine("{0} has been bought",place);
            }
            Console.WriteLine("\n--------------------------------------------\n");
            double percentage = 0.0;
            foreach(string land in allSpots)
            {
                int indexOfLand = Array.IndexOf(allSpots, land);
                percentage += 100 * (double)numTimesLanded[indexOfLand] / landedCounter;
                if (indexOfLand == 1 || indexOfLand == 3)
                {
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("{0} was landed on {1} times = %{2} ", land, numTimesLanded[indexOfLand], 100 * (double)numTimesLanded[indexOfLand] / landedCounter);
                }
                else if (indexOfLand == 6 || indexOfLand == 8 || indexOfLand == 9)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("{0} was landed on {1} times = %{2} ", land, numTimesLanded[indexOfLand], 100 * (double)numTimesLanded[indexOfLand] / landedCounter);
                }
                else if (indexOfLand == 11 || indexOfLand == 13 || indexOfLand == 14)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("{0} was landed on {1} times = %{2} ", land, numTimesLanded[indexOfLand], 100 * (double)numTimesLanded[indexOfLand] / landedCounter);
                }
                else if (indexOfLand == 16 || indexOfLand == 18 || indexOfLand == 19)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("{0} was landed on {1} times = %{2} ", land, numTimesLanded[indexOfLand], 100 * (double)numTimesLanded[indexOfLand] / landedCounter);
                }
                else if (indexOfLand == 21 || indexOfLand == 23 || indexOfLand == 24)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("{0} was landed on {1} times = %{2} ", land, numTimesLanded[indexOfLand], 100 * (double)numTimesLanded[indexOfLand] / landedCounter);
                }
                else if (indexOfLand == 26 || indexOfLand == 27 || indexOfLand == 29)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("{0} was landed on {1} times = %{2} ", land, numTimesLanded[indexOfLand], 100 * (double)numTimesLanded[indexOfLand] / landedCounter);
                }
                else if (indexOfLand == 31 || indexOfLand == 32 || indexOfLand == 34)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("{0} was landed on {1} times = %{2} ", land, numTimesLanded[indexOfLand], 100 * (double)numTimesLanded[indexOfLand] / landedCounter);
                }
                else if (indexOfLand == 37 || indexOfLand == 39)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("{0} was landed on {1} times = %{2} ", land, numTimesLanded[indexOfLand], 100 * (double)numTimesLanded[indexOfLand] / landedCounter);
                }
                else
                {
                    Console.ResetColor();
                    Console.WriteLine("{0} was landed on {1} times = %{2} ", land, numTimesLanded[indexOfLand], 100 * (double)numTimesLanded[indexOfLand] / landedCounter);
                }
            }
            Console.ResetColor();
            Console.WriteLine("Total percentage:  %{0}",percentage);
            Console.WriteLine("The number of times landed on any place is:  {0}",landedCounter);
            Console.WriteLine("the number of rolls in jail was {0}",jailRolls);
        }
        private static void AddToCounter(int sum)
        {
            switch (sum)
            {
                case 2:
                    numCounter[0]++;
                    break;
                case 3:
                    numCounter[1]++;
                    break;
                case 4:
                    numCounter[2]++;
                    break;
                case 5:
                    numCounter[3]++;
                    break;
                case 6:
                    numCounter[4]++;
                    break;
                case 7:
                    numCounter[5]++;
                    break;
                case 8:
                    numCounter[6]++;
                    break;
                case 9:
                    numCounter[7]++;
                    break;
                case 10:
                    numCounter[8]++;
                    break;
                case 11:
                    numCounter[9]++;
                    break;
                case 12:
                    numCounter[10]++;
                    break;
            }
        }
    }
}
