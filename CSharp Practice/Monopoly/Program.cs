using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolySim
{
    class Program
    {
        private static string[] buyableSpots = {"Mediterranean Avenue","Baltic Avenue","Reading Railroad","Oriental Avenue","Vermont Avenue","Connecticut Avenue","St. Charles Place","Electric Company",
                                            "States Avenue","Virginia Avenue","Pennsylvania Railroad","St. James Place","Tennessee Avenue","New York Avenue","Kentucky Avenue","Indiana Avenue","Illinois Avenue",
                                            "B&O railroad","Atlantic Avenue","Ventor Avenue","Water Works","Marvin Gardens","Pacific Avenue","North Carolina Avenue","Pennsylvania Avenue","Short line",
                                            "Park Place","Boardwalk"};
        private static string[] allSpots = {"Go","Mediterranean Avenue","Community Chest1","Baltic Avenue","Income Tax","Reading Railroad","Oriental Avenue","Chance1","Vermont Avenue","Connecticut Avenue","In Jail/Just Visiting","St. Charles Place","Electric Company",
                                            "States Avenue","Virginia Avenue","Pennsylvania Railroad","St. James Place","Community Chest2","Tennessee Avenue","New York Avenue","Free Parking","Kentucky Avenue","Chance2","Indiana Avenue","Illinois Avenue",
                                            "B&O railroad","Atlantic Avenue","Ventor Avenue","Water Works","Marvin Gardens","Go To Jail","Pacific Avenue","North Carolina Avenue","Community Chest3","Pennsylvania Avenue","Short line",
                                            "Chance3","Park Place","Luxury Tax","Boardwalk"};
        private static string[] chestCards =  { "Advance to 'Go'. (Collect $200) ", "Bank error in your favor. Collect $200","Doctor's fees. Pay $50.", "From sale of stock you get $50.", "Get Out of Jail Free.",
                                                "Go to Jail. Go directly to jail. Do not pass Go, Do not collect $200.","Grand Opera Night. Collect $50 from every player for opening night seats.",
                                                "Holiday Fund matures. Collect $100.","Income tax refund. Collect $20.","Life insurance matures – Collect $100","Hospital Fees. Pay $50.","School fees. Pay $50.",
                                                "Receive $25 consultancy fee.","You are assessed for street repairs: Pay $40 per house and $115 per hotel you own.","You have won second prize in a beauty contest. Collect $10.",
                                                "You inherit $100."};
        private static string[] chanceCards = { "Advance to 'Go'. (Collect $200)", "Advance to Illinois Ave.", "Advance to St. Charles Place.", "Advance token to nearest Utility.", "Advance token to the nearest Railroad",
                                                "Bank pays you dividend of $50.","Get out of Jail Free.","Go Back Three 3 Spaces.","Go to Jail. Go directly to Jail.","Make general repairs on all your property: For each house pay $25, For each hotel pay $100.",
                                                "Pay poor tax of $15","Take a trip to Reading Railroad.","Take a walk on the Boardwalk. Advance token to Boardwalk. ","You have been elected Chairman of the Board. Pay each player $50.",
                                                "Your building and loan matures. Collect $150.","You win a crossword puzzle. Collect $50"};
        //create 28 empty strings
        private static string[] ownedSpots= Enumerable.Repeat(string.Empty, 28).ToArray();
        //create 40 ints to count the amount of times landed on a certain place
        private static int[] numTimesLanded = new int[40];
        private static int[] numCounter = new int[12];
        private static int ownedSpotsIndex,chestIndex,chanceIndex,jailRolls,landedCounter = 0;
        //create an arraylist that holds the get out of jail cards
        private static ArrayList getOutOfJailCards = new ArrayList();
        private static Random dice = new Random();

        static void Main(string[] args)
        {
            //create a current index for your piece
            int currentIndex = 0;
            bool inJail = false;
            Console.WriteLine("Currently at {0}",allSpots[currentIndex]);
            while (!GameOver())
            {
                int movement = Roll();
                //check to make sure I don't get an ArrayOutOfBounds Exception
                if (currentIndex + movement > allSpots.Length)
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
                //Print current spot after the roll
                Console.WriteLine("After rolling a {0}, you are now at:  '{1}'", movement, allSpots[currentIndex]);

                //check the chance and community cards if landed on
                //first check Community chest
                if (currentIndex == 2 || currentIndex == 17 || currentIndex == 33)
                {
                    //print out the result of the card
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(chestCards[chestIndex]);
                    //Check if the cards are movement based
                    if (chestIndex == 0)
                    {
                        //move to 'Go'
                        currentIndex = 0;
                        Console.WriteLine("You are now at {0}",allSpots[currentIndex]);
                    }
                    else if(chestIndex==4)
                    {
                        //add a 'Get out of Jail free' card
                        getOutOfJailCards.Add("Get Out");
                    }
                    else if (chestIndex == 5)
                    {
                        //go to jail
                        currentIndex = 10;
                        inJail = true;
                    }
                    //check to make sure chestIndex does not exceed chestCard length
                    if (chestIndex == 15)
                    {
                        //reset the cards
                        chestIndex = 0;
                    }
                    else
                        chestIndex++;
                    Console.ResetColor();
                }
                //now check the chance cards
                else if(currentIndex==7 || currentIndex==22 || currentIndex==36)
                {
                    //print out the result of the card
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(chanceCards[chanceIndex]);
                    //Check the movement based cards
                    if(chanceIndex==0)
                    {
                        //move to 'Go'
                        currentIndex = 0;
                        Console.WriteLine("You are now at {0}",allSpots[currentIndex]);
                    }
                    else if(chanceIndex==1)
                    {
                        //go to Illinois avenue
                        currentIndex = 24;
                        Console.WriteLine("You are now at {0}",allSpots[currentIndex]);
                    }
                    else if(chanceIndex==2)
                    {
                        //go to St. Charles
                        currentIndex = 11;
                        Console.WriteLine("You are now at {0}",allSpots[currentIndex]);
                    }
                    else if(chanceIndex==3)
                    {
                        //go to nearest util
                        if(currentIndex==0 || currentIndex < 20)
                        {
                            //go to electric company
                            currentIndex = 12;
                            Console.WriteLine("You are now at {0}",allSpots[currentIndex]);
                        }
                        else
                        {
                            //go to water works
                            currentIndex = 28;
                            Console.WriteLine("You are now at {0}",allSpots[currentIndex]);
                        }
                    }
                    else if(chanceIndex==4)
                    {
                        //go to nearest railroad
                        if(currentIndex<=9)
                        {
                            //go to Reading Railroad
                            currentIndex = 5;
                            Console.WriteLine("You are now at {0}",allSpots[currentIndex]);
                        }
                        else if(currentIndex<=19)
                        {
                            //go to Pennsylvania Railroad
                            currentIndex = 15;
                            Console.WriteLine("You are now at {0}",allSpots[currentIndex]);
                        }
                        else if(currentIndex<=29)
                        {
                            //go to B&O Railroad
                            currentIndex = 25;
                            Console.WriteLine("You are now at {0}",allSpots[currentIndex]);
                        }
                        else
                        {
                            //go to Short Line RailRoad
                            currentIndex = 35;
                            Console.WriteLine("You are now at {0}",allSpots[currentIndex]);
                        }
                    }
                    else if(chanceIndex==6)
                    {
                        //add a 'Get out of Jail free' card
                        getOutOfJailCards.Add("Get Out");
                    }
                    else if(chanceIndex==7)
                    {
                        //go back 3 spaces
                        //check to make sure it is not negative
                        if (currentIndex - 3 < 0)
                        {
                            int remainder = Math.Abs(currentIndex - 3);
                            currentIndex = 40 - remainder;
                        }
                        else
                            currentIndex -= 3;
                        Console.WriteLine("You are now at {0}",allSpots[currentIndex]);
                    }
                    else if(chanceIndex==8)
                    {
                        //go to jail
                        inJail = true;
                        currentIndex = 10;
                        Console.WriteLine("You are now at {0}",allSpots[currentIndex]);
                    }
                    else if(chanceIndex==11)
                    {
                        //go to Reading Railroad
                        currentIndex = 5;
                        Console.WriteLine("You are now at {0}",allSpots[currentIndex]);
                    }
                    else if(chanceIndex==12)
                    {
                        //go to Boardwalk
                        currentIndex = 39;
                        Console.WriteLine("You are now at {0}",allSpots[currentIndex]);
                    }
                    //Now do checks to make cure chanceIndex does not exceed chanceCards length
                    if (chanceIndex == 15)
                    {
                        //reset the cards
                        chanceIndex = 0;
                    }
                    else
                        chanceIndex++;
                    Console.ResetColor();
                }
                if (currentIndex == 30 || inJail)
                {
                    numTimesLanded[currentIndex]++;
                    landedCounter++;
                    inJail = true;
                    currentIndex = 10;
                    Console.WriteLine("You are in Jail");
                    while (inJail)
                    {
                        if(getOutOfJailCards.Contains("Get Out"))
                        {
                            getOutOfJailCards.Remove("Get Out");
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("You used a get out of jail card.  You have {0} cards remaining",getOutOfJailCards.Count);
                            inJail = false;
                            Console.ResetColor();
                        }
                        else
                            inJail = RollOutOfJail();
                    }
                    Console.WriteLine("You are now out of jail.");
                }
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

            sumOfDice = die1 + die2;

            //display sumOfDice just to check
            Console.WriteLine("Rolls...\n---------------------------------------------");
            AddToCounter(sumOfDice,die1,die2);
            return sumOfDice;
        }
        public static bool RollOutOfJail()
        {
            int sumOfDice = 0;
            jailRolls++;
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Blue;
            int dice1 = dice.Next(1, 7);
            int dice2 = dice.Next(1, 7);
            Console.WriteLine("You rolled a {0} and a {1}", dice1, dice2);
            Console.ResetColor();
            sumOfDice = dice1 + dice2;
            AddToCounter(sumOfDice,dice1,dice2);
            if (dice1 == dice2)
            {
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
        private static void AddToCounter(int sum,int dice1, int dice2)
        {
            if (dice1 == dice2)
                numCounter[11]++;
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
