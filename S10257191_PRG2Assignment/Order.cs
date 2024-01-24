using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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
        = new List<IceCream>();
        public Order() { }

        public Order(int id, DateTime recieved)
        {
            Id = id;
            TimeRecieved = recieved;
        }

        public int DataValidationInt(string prompt, List<int> listOfValues)
        {
            int option;
            while (true)
            {                
                try
                {
                    Console.Write(prompt);
                    option = Convert.ToInt32(Console.ReadLine());
                    while (!(listOfValues.Contains(option)))
                    {
                        Console.WriteLine("Invalid Input.");
                        Console.Write(prompt);
                        option = Convert.ToInt32(Console.ReadLine());
                    }
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return option;                      
        }
        
        public void ModifyIceCream(int id)
        {
            Dictionary<int, string> FlavourDic = new Dictionary<int, string>
            {
            { 1, "Vanilla" },
            { 2, "Chocolate" },
            { 3, "Strawberry" },
            { 4, "Durian" },
            { 5, "Ube" },
            { 6, "Sea Salt" }
            };
            Dictionary<int, string> ToppingDic = new Dictionary<int, string>
            {
            { 1, "Sprinkles" },
            { 2, "Mochi" },
            { 3, "Sago" },
            { 4, "Oreos" }
            };
            Dictionary<int, string> WaffleFlavourDic = new Dictionary<int, string>
            {
            { 1, "Red Velvet" },
            { 2, "Charcoal" },
            { 3, "Pandan" },
            { 4, "Basic" }
            };

            IceCream Option1(IceCream modifyingIceCream)
            {
                int ChangeIceCreamTypeOption = 0;
                while (true)
                {
                    if (modifyingIceCream is Cup)
                    {
                        Console.WriteLine(
                            "You currently have a Cup Ice Cream.\r\n" +
                            "Options to Change:\r\n" +
                            "[1] Cone\r\n" +
                            "[2] Waffle");                        
                        List<int> values = new List<int>() { 1,2};
                        ChangeIceCreamTypeOption = DataValidationInt("Enter your Option: ", values);
                        
                        if (ChangeIceCreamTypeOption == 1)
                        {
                            bool dip = false;
                            Console.Write("Do you want a Chocolate Dipped Cone (Y/N): ");
                            string DippingOption = Console.ReadLine();
                            while (DippingOption != "Y" && DippingOption != "N")
                            {
                                Console.WriteLine("Invlid Option.");
                                Console.Write("Do you want a Chocolate Dipped Cone(Y / N): ");
                                DippingOption = Console.ReadLine();
                            }
                            if (DippingOption == "Y")
                            {
                                dip = true;
                            }
                            Cone cone = new Cone("Cone", modifyingIceCream.Scoop, modifyingIceCream.Flavours, modifyingIceCream.Toppings, dip);
                            return cone;
                        }
                        else
                        {
                            Console.WriteLine("Waffle Flavour Options:");
                            foreach(KeyValuePair<int,string> flavour in WaffleFlavourDic)
                            {
                                Console.WriteLine($"[{flavour.Key}] {flavour.Value}");
                            }
                            Console.Write("Enter your waffle flavour option: ");
                            int WaffleFlavourOption = Convert.ToInt16(Console.ReadLine());
                            while (WaffleFlavourOption < 1 || WaffleFlavourOption > WaffleFlavourDic.Count + 1)
                            {
                                Console.WriteLine("Invalid Option.");
                                Console.Write("Re-enter your waffle flavour option: ");
                                WaffleFlavourOption = Convert.ToInt16(Console.ReadLine());
                            }
                            Waffle waffle = new Waffle("Waffle", modifyingIceCream.Scoop, modifyingIceCream.Flavours,modifyingIceCream.Toppings, WaffleFlavourDic[WaffleFlavourOption]);
                            return waffle;
                        }
                    }
                    else if (modifyingIceCream is Cone)
                    {
                        Console.WriteLine(
                            "You currently have a Cone Ice Cream.\r\n" +
                            "Options to Change:\r\n" +
                            "[1] Cup\r\n" +
                            "[2] Waffle");
                        Console.Write("Enter your Option: ");
                        ChangeIceCreamTypeOption = Convert.ToInt16(Console.ReadLine());
                        while (ChangeIceCreamTypeOption != 1 && ChangeIceCreamTypeOption != 2)
                        {
                            Console.WriteLine("Invalid Option.");
                            Console.Write("Re-enter your Option: ");
                            ChangeIceCreamTypeOption = Convert.ToInt16(Console.ReadLine());
                        }
                        if (ChangeIceCreamTypeOption == 1)
                        {
                            Cup cup = new Cup("Cup", modifyingIceCream.Scoop, modifyingIceCream.Flavours, modifyingIceCream.Toppings);
                            return cup;
                        }
                        else
                        {
                            Console.WriteLine("Waffle Flavour Options:");
                            foreach (KeyValuePair<int, string> flavour in WaffleFlavourDic)
                            {
                                Console.WriteLine($"[{flavour.Key}] {flavour.Value}");
                            }
                            Console.Write("Enter your waffle flavour option: ");
                            int WaffleFlavourOption = Convert.ToInt16(Console.ReadLine());
                            while (WaffleFlavourOption < 1 && WaffleFlavourOption > WaffleFlavourDic.Count + 1)
                            {
                                Console.WriteLine("Invalid Option.");
                                Console.Write("Re-enter your waffle flavour option: ");
                                WaffleFlavourOption = Convert.ToInt16(Console.ReadLine());
                            }
                            Waffle waffle = new Waffle("Waffle", modifyingIceCream.Scoop, modifyingIceCream.Flavours, modifyingIceCream.Toppings, WaffleFlavourDic[WaffleFlavourOption]);
                            return waffle;
                        }

                    }
                    else if (modifyingIceCream is Waffle)
                    {
                        Console.WriteLine(
                            "You currently have a Waffle Ice Cream.\r\n" +
                            "Options to Change:\r\n" +
                            "[1] Cup\r\n" +
                            "[2] Cone");
                        Console.Write("Enter your Option: ");
                        ChangeIceCreamTypeOption = Convert.ToInt16(Console.ReadLine());
                        while (ChangeIceCreamTypeOption != 1 && ChangeIceCreamTypeOption != 2)
                        {
                            Console.WriteLine("Invalid Option.");
                            Console.Write("Re-enter your Option: ");
                            ChangeIceCreamTypeOption = Convert.ToInt16(Console.ReadLine());
                        }
                        if (ChangeIceCreamTypeOption == 1)
                        {
                            Cup cup = new Cup("Cup", modifyingIceCream.Scoop, modifyingIceCream.Flavours, modifyingIceCream.Toppings);
                            return cup;
                        }
                        else if (ChangeIceCreamTypeOption == 2)
                        {
                            bool dip = false;
                            Console.Write("Do you want a Chocolate Dipped Cone (Y/N): ");
                            string DippingOption = Console.ReadLine();
                            while (DippingOption != "Y" && DippingOption != "N")
                            {
                                Console.WriteLine("Invlid Option.");
                                Console.Write("Do you want a Chocolate Dipped Cone(Y / N): ");
                                DippingOption = Console.ReadLine();
                            }
                            if (DippingOption == "Y")
                            {
                                dip = true;
                            }
                            Cone cone = new Cone("Cone", modifyingIceCream.Scoop, modifyingIceCream.Flavours, modifyingIceCream.Toppings, dip);
                            return cone;
                        }


                    }
                }
                
            }

            void Option2(IceCream modifyingIceCream)
            {
                int count = 1;

                foreach (Flavour f in modifyingIceCream.Flavours)
                {
                    Console.WriteLine($"[{count}] {f}: {f.Quantity}");
                    count++;
                }
                while (true)
                {
                    try
                    {
                        Console.WriteLine(
                        "Options:\r\n" +
                        "[1] Add Scoops\r\n" +
                        "[2] Substract/Remove Scoops\r\n"
                        );
                        Console.Write("Enter your option: ");
                        int changeScoopOption = Convert.ToInt16(Console.ReadLine());
                        // Data Validation Needed
                        switch (changeScoopOption)
                        {
                            case 1:
                                Console.Write("Enter how many Scoops to add: ");
                                int AddScoopNum = Convert.ToInt16(Console.ReadLine());
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
                                while (true)
                                {
                                    try
                                    {
                                        Console.Write("Enter which flavour the scoops should be: ");
                                        int ScoopFlavourOption = Convert.ToInt16(Console.ReadLine());
                                        if (modifyingIceCream.Scoop + AddScoopNum <= 3)
                                        {
                                            bool check = false;
                                            foreach (Flavour flavour in modifyingIceCream.Flavours)
                                            {
                                                if (flavour.Type == FlavourDic[ScoopFlavourOption])
                                                {
                                                    flavour.Quantity++;
                                                    check = true;
                                                    modifyingIceCream.Scoop++;
                                                }
                                            }
                                            if (!check)
                                            {
                                                if (ScoopFlavourOption > 3)
                                                {
                                                    modifyingIceCream.Flavours.Add(new Flavour(FlavourDic[ScoopFlavourOption], true, 1));
                                                    modifyingIceCream.Scoop++;
                                                }
                                                else
                                                {
                                                    modifyingIceCream.Flavours.Add(new Flavour(FlavourDic[ScoopFlavourOption], false, 1));
                                                    modifyingIceCream.Scoop++;
                                                }

                                            }
                                            break;
                                        }
                                        else if (ScoopFlavourOption < 1 && ScoopFlavourOption > 6)
                                        {
                                            Console.WriteLine("Enter a Valid Flabour Option.");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Scoop Cannot have more than 3 Scoops.");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }

                                }
                                break;

                            case 2:
                                while (true)
                                {
                                    try
                                    {
                                        count = 1;
                                        foreach (Flavour flavour in modifyingIceCream.Flavours)
                                        {
                                            Console.WriteLine($"[{count}] {flavour.Type}: {flavour.Quantity}");
                                            count++;
                                        }
                                        Console.Write("Enter the option of the flabour scoop you wish to remove: ");
                                        int RemoveScoopOption = Convert.ToInt16(Console.ReadLine());
                                        while (RemoveScoopOption > modifyingIceCream.Flavours.Count || RemoveScoopOption < 1)
                                        {
                                            Console.WriteLine("Enter a valid Option.");
                                            Console.Write("Enter the option of the flabour scoop you wish to remove: ");
                                            RemoveScoopOption = Convert.ToInt16(Console.ReadLine());
                                        }
                                        Console.Write("Enter how many scoops you wish to remove: ");
                                        int RemoveScoopNum = Convert.ToInt16(Console.ReadLine());
                                        while (RemoveScoopNum > modifyingIceCream.Flavours[RemoveScoopOption - 1].Quantity)
                                        {
                                            Console.WriteLine("You cannot remove more than the number of scoops in the falbour.");
                                            Console.Write("Enter how many scoops you wish to remove: ");
                                            RemoveScoopNum = Convert.ToInt16(Console.ReadLine());

                                        }
                                        modifyingIceCream.Flavours[RemoveScoopOption - 1].Quantity -= RemoveScoopNum;
                                        break;
                                    }
                                    catch (FormatException ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }


                                break;
                        }
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Enter a valid Option.");
                    }
                }
            }

            void Option3(IceCream modifyingIceCream)
            {
                int count = 1;
                foreach (Flavour f in modifyingIceCream.Flavours)
                {
                    Console.WriteLine($"[{count}] {f}: {f.Quantity}");
                    count++;
                }

                while (true)
                {
                    try
                    {
                        Console.Write("Enter which flavour you want to change:");
                        int changeFlavourOption = Convert.ToInt16(Console.ReadLine());
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
                        Console.Write("Enter which flavour you want to change to: ");
                        int ToFlavaour = Convert.ToInt16(Console.ReadLine());
                        modifyingIceCream.Flavours[changeFlavourOption].Type = FlavourDic[ToFlavaour];
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Invalid Option. Enter Options Again.");
                    }
                }
            }

            void Option4(IceCream modifyingIceCream)
            {
                int count = 1;
                Console.WriteLine("Current Toppings:");
                foreach (Topping topping in modifyingIceCream.Toppings)
                {
                    Console.WriteLine($"[{count}] {topping.Type}");
                }
                while (true)
                {
                    try
                    {
                        Console.WriteLine(
                        "Options\r\n" +
                        "[1] Add Toppings\r\n" +
                        "[2] Remove Toppings\r\n");
                        Console.Write("Enter you Option: ");
                        int ToppingOption = Convert.ToInt16(Console.ReadLine());
                        while (ToppingOption != 1 || ToppingOption != 2)
                        {
                            Console.WriteLine("Invalid Option. Enter again.");
                            Console.Write("Enter you Option: ");
                            ToppingOption = Convert.ToInt16(Console.ReadLine());
                        }
                        if (ToppingOption == 1)
                        {
                            if (modifyingIceCream.Toppings.Count == 4)
                            {
                                Console.WriteLine("You cannot have more than 4 toppings.");
                            }
                            else
                            {
                                foreach (KeyValuePair<int, string> kvp in ToppingDic)
                                {
                                    Console.WriteLine($"[{kvp.Key}] {kvp.Value}");
                                }
                                Console.Write("Enter what topping you want to add: ");
                                int AddToppingOption = Convert.ToInt16(Console.ReadLine());
                                while (!ToppingDic.ContainsKey(AddToppingOption))
                                {
                                    Console.WriteLine("Enter a valid topping option.");
                                    Console.Write("Enter what topping you want to add: ");
                                    AddToppingOption = Convert.ToInt16(Console.ReadLine());
                                }
                                modifyingIceCream.Toppings.Add(new Topping(ToppingDic[AddToppingOption]));
                            }
                        }
                        else if (ToppingOption == 2)
                        {
                            Console.Write("Enter what topping option you want to remove: ");
                            int RemoveToppingOption = Convert.ToInt16(Console.ReadLine());
                            while (RemoveToppingOption > modifyingIceCream.Toppings.Count)
                            {
                                Console.WriteLine("Invalid Option.");
                                Console.Write("Enter what topping option you want to remove: ");
                                RemoveToppingOption = Convert.ToInt16(Console.ReadLine());
                            }
                            modifyingIceCream.Toppings.RemoveAt(RemoveToppingOption - 1);
                        }
                        break;
                    }
                    catch (Exception e) { Console.WriteLine(e.Message); }

                }
            }

            void Option5Cone(Cone modifyingIceCream)
            {
                while (true)
                {
                    try
                    {
                        if (modifyingIceCream.Dipped)
                        {
                            Console.Write("Enter 'Y' to not dip the cone in chocolate " +
                                "and 'N' to have a cone dipped in chocolate: ");
                            string DippedConeReply = Console.ReadLine();
                            while (DippedConeReply != "Y" && DippedConeReply != "N")
                            {
                                Console.WriteLine("Enter a valid option of 'Y' or 'N'.");
                                Console.Write("Re-enter 'Y' to not dip the cone in chocolate " +
                                "and 'N' to have a cone dipped in chocolate: ");
                                DippedConeReply = Console.ReadLine();
                            }
                            if (DippedConeReply == "Y")
                            {
                                modifyingIceCream.Dipped = false;
                            }
                        }
                        else
                        {

                            Console.Write("Enter 'Y' to have a cone dipped in chocolate " +
                                "and 'N' to not dip the cone in chocolate: ");
                            string DippedConeReply = Console.ReadLine();
                            while (DippedConeReply != "Y" || DippedConeReply != "N")
                            {
                                Console.WriteLine("Enter a valid option of 'Y' or 'N'.");
                                Console.Write("Re-enter 'Y' to have a cone dipped in chocolate " +
                                "and 'N' to not dip the cone in chocolate");
                                DippedConeReply = Console.ReadLine();
                            }
                            if (DippedConeReply == "N")
                            {
                                modifyingIceCream.Dipped = false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            void Option5Waffle(Waffle modifyingIceCream)
            {
                Console.WriteLine($"Current waffle flavour is {modifyingIceCream.WaffleFlavour}");
                Console.WriteLine("Available waffle flavours:");
                foreach (KeyValuePair<int,string> kvp in WaffleFlavourDic)
                {
                    Console.WriteLine($"[{kvp.Key}] {kvp.Value}");
                }
                while (true)
                {
                    try
                    {
                        Console.Write("Enter option for waffle flavour you want to change to: ");
                        int WaffleFlavourOption = Convert.ToInt16(Console.ReadLine());
                        while (!WaffleFlavourDic.ContainsKey(WaffleFlavourOption))
                        {
                            Console.WriteLine("Invalid Option.");
                            Console.Write("Enter option for waffle flavour you want to change to: ");
                            WaffleFlavourOption = Convert.ToInt16(Console.ReadLine());
                        }
                        modifyingIceCream.WaffleFlavour = WaffleFlavourDic[WaffleFlavourOption];
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                
            }

            IceCream modifyingIceCream = IceCreamList[id--];
            
            if (modifyingIceCream is Cup)
            {
                Console.WriteLine(
                    "Modification Options:\r\n" +
                    "[1] Option\r\n" +
                    "[2] Scoops\r\n" +
                    "[3] Flavours\r\n" +
                    "[4] Toppings\r\n"                    
                    );
                
                while (true)
                {
                    try
                    {
                        Console.Write("Enter your Option: ");
                        int ModifyingOption = Convert.ToInt16(Console.ReadLine());
                        switch (ModifyingOption)
                        {
                            case 1:
                                modifyingIceCream = Option1(modifyingIceCream);
                                break;
                            case 2:
                                Option2(modifyingIceCream);
                                break;
                            case 3:
                                Option3(modifyingIceCream);
                                break;
                            case 4:
                                Option4(modifyingIceCream);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
                }
            }
            else if (modifyingIceCream is Cone)
            {
                Cone modifyingCone = (Cone)modifyingIceCream;
                Console.WriteLine(
                    "Modification Options:\r\n" +
                    "[1] Option\r\n" +
                    "[2] Scoops\r\n" +
                    "[3] Flavours\r\n" +
                    "[4] Toppings\r\n" +
                    "[5] Dipped Cone\r\n" 
                    );
                while (true)
                {
                    Console.Write("Enter your Option: ");
                    int ModifyingOption = Convert.ToInt16(Console.ReadLine());
                    switch (ModifyingOption)
                    {
                        case 1:
                            modifyingIceCream = Option1(modifyingIceCream);
                            break;
                        case 2:
                            Option2(modifyingCone);
                            break;
                        case 3:
                            Option3(modifyingCone);
                            break;
                        case 4:
                            Option4(modifyingCone);
                            break;
                        case 5:
                            Option5Cone(modifyingCone);
                            break;
                    }
                    break;
                }
            }
            else if (modifyingIceCream is Waffle)
            {
                Waffle modifyingWaffle = (Waffle)modifyingIceCream;
                Console.WriteLine(
                    "Modification Options:\r\n" +
                    "[1] Option\r\n" +
                    "[2] Scoops\r\n" +
                    "[3] Flavours\r\n" +
                    "[4] Toppings\r\n" +
                    "[5] Waffle Flavour\r\n"
                    );
                while (true)
                {
                    Console.Write("Enter your Option: ");
                    int ModifyingOption = Convert.ToInt16(Console.ReadLine());
                    switch (ModifyingOption)
                    {
                        case 1:
                            modifyingIceCream = Option1(modifyingIceCream);
                            break;
                        case 2:
                            Option2(modifyingWaffle);
                            break;
                        case 3:
                            Option3(modifyingWaffle);
                            break;
                        case 4:
                            Option4(modifyingWaffle);
                            break;
                        case 5:
                            Option5Waffle(modifyingWaffle);
                            break;
                    }
                    break;
                }
            }
        }

        public void AddIceCream(IceCream iceCream)
        {
            IceCreamList.Add(iceCream);            
        }

        public void DeleteIceCream(int id)
        {
            id--;
            IceCreamList.RemoveAt(id);

        }

        public double CalculateTotal()
        {
            double total = 0;
            foreach (IceCream iceCream in IceCreamList)
            {
                total += iceCream.CalculatePrice();
            }
            return total;
        }
        public override string ToString()
        {
            string result = $"\nID: {Id,-5}\nTime Recieved: {TimeRecieved}\nTime Fulfilled: {TimeFulfilled}" +
                $"\nIce Cream(s):\n";
            foreach (IceCream ic in IceCreamList)
            {
                result += $"{ic}\n";
            }
            result += $"Total: {CalculateTotal(): 0.00}";
            return result;
        }
    }
}
