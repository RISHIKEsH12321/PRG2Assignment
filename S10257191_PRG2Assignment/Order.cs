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
        private double finalTotal;

        public int Id { get; set; }
        public DateTime TimeRecieved { get; set; }
        public DateTime? TimeFulfilled { get; set; }

        public List<IceCream> IceCreamList { get; set; }
        = new List<IceCream>();

        public double FinalTotal { get; set; } //Final total after Birthday discount & Point Redemption
        public Order() { }

        public Order(int id, DateTime recieved)
        {
            Id = id;
            TimeRecieved = recieved;
        }

        public void ModifyIceCream(int id)
        {
            //Dictionaries of Data for Scoop Flavour, Topping and Waffle Flavour
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

            //Data Validation Method
            //Takes in a list of allowable values and a prompt for asking input
            int DataValidationInt(string prompt, List<int> listOfValues)
            {
                int option; //Return value
                while (true)
                {
                    try
                    {
                        Console.Write(prompt);
                        option = Convert.ToInt32(Console.ReadLine());
                        while (!(listOfValues.Contains(option))) //If list doesn't contain input
                        {
                            Console.WriteLine("Invalid Input.");
                            Console.Write(prompt);
                            option = Convert.ToInt32(Console.ReadLine());
                        }
                        break;
                    }
                    catch (Exception ex)//If cannot convert to int
                    {
                        Console.WriteLine(ex.Message); 
                    }
                }
                return option;
            }

            //Methods for each option

            //Change IceCream Option
            IceCream Option1(IceCream modifyingIceCream)
            {
                int ChangeIceCreamTypeOption = 0; //Input
                while (true)
                {
                    if (modifyingIceCream is Cup)//Changes Cup to other Cone or Waffle
                    {
                        Console.WriteLine(
                            "You currently have a Cup Ice Cream.\r\n" +
                            "Options to Change:\r\n" +
                            "[1] Cone\r\n" +
                            "[2] Waffle");
                        ChangeIceCreamTypeOption = DataValidationInt("Enter your Option: ", new List<int> { 1, 2 });
                        if (ChangeIceCreamTypeOption == 1)//Need extra data of dipped
                        {
                            bool dip = false;
                            Console.Write("Do you want a Chocolate Dipped Cone (Y/N): ");
                            string DippingOption = Console.ReadLine().ToUpper();
                            while (DippingOption != "Y" && DippingOption != "N")//Data Validation
                            {
                                Console.WriteLine("Invlid Option.");
                                Console.Write("Do you want a Chocolate Dipped Cone(Y/N): ");
                                DippingOption = Console.ReadLine().ToUpper();
                            }
                            if (DippingOption == "Y")
                            {
                                dip = true;
                            }
                            //Create Cone Object and return it
                            Cone cone = new Cone("Cone", modifyingIceCream.Scoop, modifyingIceCream.Flavours, modifyingIceCream.Toppings, dip);
                            return cone;
                        }
                        else //Need extra data on waffle falvour
                        {
                            Console.WriteLine("Waffle Flavour Options:");
                            //Dispaly all waffle falvours
                            foreach (KeyValuePair<int, string> flavour in WaffleFlavourDic)
                            {
                                Console.WriteLine($"[{flavour.Key}] {flavour.Value}");
                            }
                            //Get input and validate it
                            int WaffleFlavourOption = DataValidationInt("Enter your waffle flavour option: ",
                                                        new List<int> { 1, 2, 3, 4 });               
                            //Create Waffel Object & Return it
                            Waffle waffle = new Waffle("Waffle", modifyingIceCream.Scoop, modifyingIceCream.Flavours, modifyingIceCream.Toppings, WaffleFlavourDic[WaffleFlavourOption]);
                            return waffle;
                        }
                    }
                    else if (modifyingIceCream is Cone)
                    {                        
                        //Display Options   
                        Console.WriteLine(
                            "You currently have a Cone Ice Cream.\r\n" +
                            "Options to Change:\r\n" +
                            "[1] Cup\r\n" +
                            "[2] Waffle");
                        //Input and data validation
                        ChangeIceCreamTypeOption = ChangeIceCreamTypeOption = DataValidationInt("Enter your Option: ",
                            new List<int> { 1, 2 });
                        if (ChangeIceCreamTypeOption == 1)
                        {
                            //No Extra input required
                            Cup cup = new Cup("Cup", modifyingIceCream.Scoop, modifyingIceCream.Flavours, modifyingIceCream.Toppings);
                            return cup;
                        }
                        else//Asking waffle flavour
                        {
                            Console.WriteLine("Waffle Flavour Options:");
                            foreach (KeyValuePair<int, string> flavour in WaffleFlavourDic)
                            {
                                Console.WriteLine($"[{flavour.Key}] {flavour.Value}");
                            }                            
                            int WaffleFlavourOption = DataValidationInt("Enter your waffle flavour option: ",
                                                        new List<int> { 1, 2, 3, 4 });                            
                            Waffle waffle = new Waffle("Waffle", modifyingIceCream.Scoop, modifyingIceCream.Flavours, modifyingIceCream.Toppings, WaffleFlavourDic[WaffleFlavourOption]);
                            return waffle;
                        }

                    }
                    else if (modifyingIceCream is Waffle)
                    {
                        //Display Options
                        Console.WriteLine(
                            "You currently have a Waffle Ice Cream.\r\n" +
                            "Options to Change:\r\n" +
                            "[1] Cup\r\n" +
                            "[2] Cone");
                        //Get Input & Validate it
                        ChangeIceCreamTypeOption = ChangeIceCreamTypeOption = DataValidationInt("Enter your Option: ",
                            new List<int> { 1, 2 });                        
                        if (ChangeIceCreamTypeOption == 1)
                        {
                            //No extra data needed 
                            Cup cup = new Cup("Cup", modifyingIceCream.Scoop, modifyingIceCream.Flavours, modifyingIceCream.Toppings);
                            return cup;
                        }
                        //Need to ask for dipped choice
                        else if (ChangeIceCreamTypeOption == 2)
                        {
                            bool dip = false;
                            Console.Write("Do you want a Chocolate Dipped Cone (Y/N): ");
                            string DippingOption = Console.ReadLine();
                            while (DippingOption != "Y" || DippingOption != "N")
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

            //Change Flavour Qty
            void Option2(IceCream modifyingIceCream)
            {
                int count = 1; //Keep count of flavours
                //Display Flavours and their qty
                Console.WriteLine("Current Flavours in your Ice Cream: ");
                foreach (Flavour f in modifyingIceCream.Flavours)
                {
                    Console.WriteLine($"[{count}] {f.Type}: {f.Quantity}");
                    count++;
                }
                Console.WriteLine();
                while (true)
                {
                    try
                    {
                        //Display Options
                        Console.WriteLine(
                        "Options:\r\n" +
                        "[1] Add Scoops\r\n" +
                        "[2] Substract/Remove Scoops\r\n"
                        );
                        //Get input and check if its correct
                        int changeScoopOption = DataValidationInt("Enter your Option: ", new List<int> { 1,2 });
                        switch (changeScoopOption)
                        {
                            case 1://Add Scoops
                                int AddScoopNum = DataValidationInt("Enter how many Scoops to add: "
                                    , new List<int> { 1, 2, 3 }); // Cannot add 0 or more than 3
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
                                        //Get input of flavour
                                        int ScoopFlavourOption = DataValidationInt("Enter which flavour the scoops should be added: ",
                                            new List<int> { 1, 2, 3, 4, 5, 6 });
                                        //Check the nummber of Scoops if the new scoop(s) is added to the current one
                                        if (modifyingIceCream.Scoop + AddScoopNum <= 3) //If its 3 or less => allow adding of scoops
                                        {
                                            bool check = false; //Check if flavour is already in Flavour list                                            
                                            foreach (Flavour flavour in modifyingIceCream.Flavours)
                                            {
                                                //If in list => Quantity += Num of Scoops inputted 
                                                if (flavour.Type == FlavourDic[ScoopFlavourOption])
                                                {
                                                    flavour.Quantity += AddScoopNum;
                                                    check = true;
                                                    modifyingIceCream.Scoop += AddScoopNum;
                                                }
                                            }
                                            //If not present in flavour list
                                            //Create Flavor object and add it to the ice cream
                                            if (!check)
                                            {
                                                if (ScoopFlavourOption > 3)//Premium Flavours
                                                {
                                                    modifyingIceCream.Flavours.Add(new Flavour(FlavourDic[ScoopFlavourOption], true, AddScoopNum));
                                                    modifyingIceCream.Scoop += AddScoopNum;
                                                }
                                                else//Regular Flavours
                                                {
                                                    modifyingIceCream.Flavours.Add(new Flavour(FlavourDic[ScoopFlavourOption], false, AddScoopNum));
                                                    modifyingIceCream.Scoop += AddScoopNum;
                                                }
                                            }
                                            break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }
                                break;

                            case 2://Remove Scoops
                                while (true)
                                {
                                    try
                                    {
                                        count = 1;
                                        List<int> dataValidationListFlavours = new List<int>();//All possible inputs list
                                        //Re-Display All Flavours
                                        Console.WriteLine("Current Flavours in your Ice Cream: ");
                                        foreach (Flavour flavour in modifyingIceCream.Flavours)
                                        {
                                            Console.WriteLine($"[{count}] {flavour.Type}: {flavour.Quantity}");
                                            dataValidationListFlavours.Add(count); //Adding possible inputs
                                            count++;
                                            
                                        }
                                        Console.WriteLine();
                                        
                                        //Console.Write("Enter the option of the flabour scoop you wish to remove: ");

                                        //Asking for input of flavour to remoce scoops
                                        int RemoveScoopOption = DataValidationInt("Enter the option of the flavour scoop you wish to remove: ",
                                            dataValidationListFlavours); 
                                        //while (RemoveScoopOption > modifyingIceCream.Flavours.Count || RemoveScoopOption < 1)
                                        //{
                                        //    Console.WriteLine("Enter a valid Option.");
                                        //    Console.Write("Enter the option of the flabour scoop you wish to remove: ");
                                        //    RemoveScoopOption = Convert.ToInt16(Console.ReadLine());
                                        //}
                                        //Console.Write("Enter how many scoops you wish to remove: ");

                                        int RemoveScoopNum = DataValidationInt("Enter how many scoops you wish to remove: ",
                                            new List<int> { 1, 2 }); //Cannot be 3 beacuse Ice Cream needs at least 1 scoops

                                        //while (RemoveScoopNum > modifyingIceCream.Flavours[RemoveScoopOption - 1].Quantity)
                                        //{
                                        //    Console.WriteLine("You cannot remove more than the number of scoops in the falbour.");
                                        //    Console.Write("Enter how many scoops you wish to remove: ");
                                        //    RemoveScoopNum = Convert.ToInt16(Console.ReadLine());
                                        //}
                                        
                                        //Decrease the Flavour Qty of that option
                                        modifyingIceCream.Flavours[RemoveScoopOption - 1].Quantity -= RemoveScoopNum;
                                        //Decrease the scoops by the number of scoops removed
                                        modifyingIceCream.Scoop -= RemoveScoopNum;
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

            //Change Flavour of Scoops
            void Option3(IceCream modifyingIceCream)
            {
                int count = 1;
                //Display Current Flavours
                Console.WriteLine("Current Flavours:");
                foreach (Flavour f in modifyingIceCream.Flavours)
                {
                    Console.WriteLine($"[{count}] {f.Type}: {f.Quantity}");
                    count++;
                }
                Console.WriteLine();
                while (true)
                {
                    try
                    {
                        List<int> dataVal = new List<int>();
                        for (int i = 1; i < count; i++) { dataVal.Add(i); }
                        int changeFlavourOption = DataValidationInt("Enter which flavour you want to change:",
                            dataVal);
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
                        int ToFlavaour = DataValidationInt("Enter which flavour you want to change to: ",
                            new List<int> { 1, 2, 3, 4, 5, 6 });

                        bool checkIfFlavourExsists = true;
                        //Check if the flavour that we are trying to change to exsists
                        //If it exsists, we add the qty of the changing flavour and remove that flavour
                        for (int i = 0; i < modifyingIceCream.Flavours.Count; i++)
                        {
                            if (modifyingIceCream.Flavours[i].Type == FlavourDic[ToFlavaour])
                            {
                                checkIfFlavourExsists = false;
                                modifyingIceCream.Flavours[i].Quantity += modifyingIceCream.Flavours[changeFlavourOption - 1].Quantity;
                                modifyingIceCream.Flavours.RemoveAt(changeFlavourOption - 1);
                                break;
                            }
                        }
                        //If it doesn't exsist, we simply add change the type and Premium value(if applicable) of the flavour.
                        if (checkIfFlavourExsists)
                        {
                            if (ToFlavaour > 3)
                            {
                                modifyingIceCream.Flavours[changeFlavourOption - 1].Type = FlavourDic[ToFlavaour];
                                modifyingIceCream.Flavours[changeFlavourOption - 1].Premium = true;
                            }
                            else
                            {
                                modifyingIceCream.Flavours[changeFlavourOption - 1].Type = FlavourDic[ToFlavaour];
                                modifyingIceCream.Flavours[changeFlavourOption - 1].Premium = false;
                            }
                        }
                        
                        //modifyingIceCream.Flavours[changeFlavourOption--].Type = FlavourDic[ToFlavaour];
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(1);
                        //Console.WriteLine("Invalid Option. Enter Options Again.");
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            //Change Toppings
            void Option4(IceCream modifyingIceCream)
            {
                int count = 1;
                //Display Current Toppings
                Console.WriteLine("Current Toppings:");
                foreach (Topping topping in modifyingIceCream.Toppings)
                {
                    Console.WriteLine($"[{count}] {topping.Type}");
                    count++;
                }
                List<int> dataVal = new List<int>();
                for (int i = 1; i < count; i++) { dataVal.Add(i); }
                Console.WriteLine();
                while (true)
                {
                    try
                    {
                        //Display Options
                        Console.WriteLine(
                        "Options\r\n" +
                        "[1] Add Toppings\r\n" +
                        "[2] Remove Toppings\r\n");
                        int ToppingOption = DataValidationInt("Enter you Option: ",
                            new List<int> { 1, 2 });
                        
                        if (ToppingOption == 1)
                        {
                            //Cannot allow more than 4 toppings, so reject it
                            if (modifyingIceCream.Toppings.Count == 4)
                            {
                                Console.WriteLine("You cannot have more than 4 toppings.");
                            }
                            else
                            {
                                //Display Topping Options
                                foreach (KeyValuePair<int, string> kvp in ToppingDic)
                                {
                                    Console.WriteLine($"[{kvp.Key}] {kvp.Value}");
                                }
                                //Get Topping Option
                                int AddToppingOption = DataValidationInt("Enter what topping you want to add: ",
                                    new List<int> { 1, 2, 3, 4 });  
                                //Add Topping Object to IceCream's Topping List
                                modifyingIceCream.Toppings.Add(new Topping(ToppingDic[AddToppingOption]));
                            }
                        }
                        else if (ToppingOption == 2)
                        {
                            //Get which topping option they want to remove
                            int RemoveToppingOption = DataValidationInt("Enter what topping option you want to remove: ",
                                dataVal);      
                            //Remove that Topping
                            modifyingIceCream.Toppings.RemoveAt(RemoveToppingOption - 1);
                        }
                        break;
                    }
                    catch (Exception e) { Console.WriteLine(e.Message); }

                }
            }

            //Dipped Cone or Not
            void Option5Cone(Cone modifyingIceCream)
            {
                while (true)
                {
                    try
                    {
                        //Ask for input
                        Console.Write("Do you want a Chocolate Dipped Cone (Y/N): ");
                        string DippedConeReply = Console.ReadLine();
                        //Data Validation
                        while (DippedConeReply != "Y" || DippedConeReply != "N")
                        {
                            Console.WriteLine("Enter a valid option of 'Y' or 'N'.");
                            Console.Write("Do you want a Chocolate Dipped Cone (Y/N): ");
                            DippedConeReply = Console.ReadLine();
                        }
                        //If They Want then dipped is true
                        if (DippedConeReply == "Y")
                        {
                            modifyingIceCream.Dipped = true;
                        }
                        //Else its false
                        else { modifyingIceCream.Dipped = false; }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            
            //Waffle Flavour
            void Option5Waffle(Waffle modifyingIceCream)
            {
                //Display Current Waffle Flavour and Waffle Falvour Options
                Console.WriteLine($"Current waffle flavour is {modifyingIceCream.WaffleFlavour}");
                Console.WriteLine("Available waffle flavours:");
                foreach (KeyValuePair<int, string> kvp in WaffleFlavourDic)
                {
                    Console.WriteLine($"[{kvp.Key}] {kvp.Value}");
                }
                Console.WriteLine();

                while (true)
                {
                    try
                    {
                        //Ask for input
                        Console.Write("Enter option for waffle flavour you want to change to: ");
                        int WaffleFlavourOption = DataValidationInt("Enter option for waffle flavour you want to change to: ",
                            new List<int> { 1, 2, 3, 4 });
                        //Change Waffle Flavour
                        modifyingIceCream.WaffleFlavour = WaffleFlavourDic[WaffleFlavourOption];
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            
            //Indexing requires a minus one from the inputted id
            id--;
            //Create A variable to contain the IceCream that is to be changed
            IceCream modifyingIceCream = IceCreamList[id];

            //Different Promts for each type of IceCream
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

                        int ModifyingOption = DataValidationInt("Enter your Option: ",
                            new List<int> { 1, 2, 3, 4 }); 
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
                    int ModifyingOption = DataValidationInt("Enter your Option: ",
                            new List<int> { 1, 2, 3, 4, 5  });
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
                    }break;
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
                    int ModifyingOption = DataValidationInt("Enter your Option: ",
                            new List<int> { 1, 2, 3, 4, 5 });
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
            //Set the IceCream that was to be modified to the modified IceCream
            IceCreamList[id] = modifyingIceCream;
        }

        public void AddIceCream(IceCream iceCream)
        {
            IceCreamList.Add(iceCream);
        }

        public void DeleteIceCream(int id)
        {
            id--; //Index starts from 0
            IceCreamList.RemoveAt(id);

        }

        public double CalculateTotal()
        {
            double total = 0;
            foreach (IceCream iceCream in IceCreamList)
            {
                total += iceCream.CalculatePrice(); // Call price of all ice creams
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
            if (FinalTotal != 0)
            {
                result += $"Total: {FinalTotal: 0.00}";
            }
            else { result += $"Total: {CalculateTotal(): 0.00}"; }
            
            return result;
        }
    }
}
