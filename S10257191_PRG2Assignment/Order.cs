using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace S10257191_PRG2Assignment
{
    internal class Order
    {
        private int id;
        private DateTime timeRecieved;
        private DateTime? timeFulfilled;
        private List<IceCream> iceCreamList;

        public int Id { get; set; }
        public DateTime TimeRecieved { get; set; }
        public DateTime? TimeFulfilled { get; set; }

        public List<IceCream> IceCreamList { get; set; }

        public Order() { }

        public Order(int id, DateTime recieved)
        {
            Id = id;
            TimeRecieved = recieved;
            IceCreamList = new List<IceCream>();
        }

        public void ModifyIceCream(int id)
        {

        }

        public void AddIceCream(IceCream iceCream)
        {
            IceCream newIceCream;
            void IceCreamType()
            {
                Console.WriteLine("Enter the option of the type of Ice Cream you want:");
                Console.WriteLine("[1] Cup\r\n[2] Cone\r\n[3] Waffle");
                while (true)
                {
                    try
                    {
                        Console.Write("Enter your Option Number: ");
                        if (Console.ReadLine() == "1")
                        {
                            newIceCream = new Cup();

                        }
                        else if (Console.ReadLine() == "2")
                        {
                            newIceCream = new Cone();
                        }
                        else if (Console.ReadLine() == "3")
                        {
                            newIceCream = new Waffle();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            IceCreamType();


            //Checks For Number of Scoops
            int scoops = 0;
            //while (true)
            //{
            //    Console.Write("Enter the number of Scoops(1/2/3): ");
            //    try
            //    {
            //        int num = Convert.ToInt16(Console.ReadLine());
            //        while (!(num == 1 || num == 2 || num ==3))
            //        {
            //            Console.Write("Enter the number of Scoops(1/2/3): ");
            //            num = Convert.ToInt16(Console.ReadLine());
            //        }
            //        break;

            //    }
            //    catch (Exception)
            //    {
            //        Console.WriteLine("The valid options are 1, 2 or 3.");
            //        Console.WriteLine("Enter a valid option.");
            //    }
            //}

            //Creating the Falvour
            while (true)
            {
                if (scoops == 3)
                {
                    break;
                }
                Console.WriteLine
                    (
                    "Regular Flavours" +
                    "\r\n[1] Vanilla" +
                    "\r\n[2] Chocolate" +
                    "\r\n[3] Strawberry" +
                    "\r\nPremium Flavours (+$2 per scoop)" +
                    "\r\n[4] Durian" +
                    "\r\n[5] Ube" +
                    "\r\n[6] Sea Salt"
                    );


                while (scoops < 3)
                {
                    Dictionary<string, string> FlavourDic = new Dictionary<string, string>();
                    FlavourDic.Add("1", "Vanilla");
                    FlavourDic.Add("2", "Chocolate");
                    FlavourDic.Add("3", "Strawberry");
                    FlavourDic.Add("4", "Durian");
                    FlavourDic.Add("5", "Ube");
                    FlavourDic.Add("6", "Sea Salt");

                    Console.Write("Enter Your Flavour Number: ");
                    string option = Console.ReadLine();
                    if (FlavourDic.ContainsKey(option))
                    {
                        Console.WriteLine($"Enter the number of Scoops of {FlavourDic[option]}: ");
                        try
                        {
                            int FlavourScoopNum = Convert.ToInt16(Console.ReadLine());
                            while (FlavourScoopNum > 3 && scoops > 3)
                            {
                                Console.WriteLine("You cannot have more than 3 scoops.");
                                Console.WriteLine($"Enter the number of Scoops of {FlavourDic[option]}: ");
                                FlavourScoopNum = Convert.ToInt16(Console.ReadLine());
                            }
                            scoops += FlavourScoopNum;
                            if (option == "4" || option == "5" || option == "6")
                            {
                                newIceCream.Flavours.Add(new Flavour(FlavourDic[option], true, FlavourScoopNum));
                            }
                            else
                            {
                                newIceCream.Flavours.Add(new Flavour(FlavourDic[option], false, FlavourScoopNum));
                            }

                        }
                        catch
                        {
                            Console.WriteLine("Invalid Number.");
                        }
                    }

                }

            }


            //Creating the Toppings
            while (true)
            {
                Console.WriteLine(
                    "Toppings (+$1 each)\r\n" +
                    "Sprinkles\r\n" +
                    "Mochi\r\n" +
                    "Sago\r\n" +
                    "Oreos\r\n");

                Console.Write("Enter the ");
            }
        }
    }
}


//Fuck
