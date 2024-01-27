// See https://aka.ms/new-console-template for more information
using S10257191_PRG2Assignment;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Transactions;
using System.Globalization;
using System;


//==========================================================
// Student Number : S10241860
// Student Name : Joseph Kwek Jun Yan
// Partner Name : Vallimuthu Rishikesh
//==========================================================

//creating customer dictionary to store information of customers
Dictionary<int, Customer> CustomerDic = new Dictionary<int, Customer>();

//Create flavor dictionary to store information of the flavors
Dictionary<string, Flavour> FlavourDic = new Dictionary<string, Flavour>
{
    { "Vanilla", new Flavour("Vanilla", false, 0) },
    { "Chocolate", new Flavour("Chocolate", false, 0) },
    { "Strawberry", new Flavour("Strawberry", false, 0) },
    { "Durian", new Flavour("Durian", true, 0) },
    { "Ube", new Flavour("Ube", true, 0) },
    { "Sea Salt", new Flavour("Sea salt", true, 0) },
};

Queue<Order> regularOrderQueue = new Queue<Order>();
Queue<Order> goldOrderQueue = new Queue<Order>();
int orderCount = 0;

InnitCustomer(CustomerDic);
orderCount = InnitOrders(CustomerDic, orderCount);
orderCount++;

void InnitCustomer(Dictionary<int, Customer> cusDic)
{
    using (StreamReader sr = new StreamReader("customers.csv"))
    {
        string? s = sr.ReadLine(); // read the heading

        while ((s = sr.ReadLine()) != null)
        {
            string[] data = s.Split(',');
            //Name, MemberId, DOB, MembershipStatus(Tier), MembershipPoints, PunchCard
            try
            {
                int id = Convert.ToInt32(data[1]);
                DateTime dob = Convert.ToDateTime(data[2]);
                cusDic.Add(id, new Customer(data[0], id, dob));
                cusDic[id].Rewards = new PointCard();
                cusDic[id].Rewards.Points = Convert.ToInt32(data[4]);
                cusDic[id].Rewards.PunchCard = Convert.ToInt32(data[5]);
                cusDic[id].Rewards.Tier = data[3];
            }
            catch
            {
                Console.WriteLine("Invalid Data in csv files.");
            }
        }
    }
}
int InnitOrders(Dictionary<int, Customer> cusDic, int orderCount)
{
    using (StreamReader sr = new StreamReader("orders.csv"))
    {
        string? s = sr.ReadLine(); // read the heading

        while ((s = sr.ReadLine()) != null)
        {
            string?[] data = s.Split(',');
            //Id[0], MemberId[1], TimeReceived[2], TimeFulfilled[3], Option[4], Scoops[5],
            //Dipped[6], WaffleFlavour[7],
            //Flavour1[8], Flavour2[9], Flavour3[10],
            //Topping1[11], Topping2[12], Topping3[13], Topping4[14]

            try
            {
                int OrderId = Convert.ToInt32(data[0]);
                orderCount = OrderId;
                int Memid = Convert.ToInt32(data[1]);
                DateTime Recieved = Convert.ToDateTime(data[2]);
                DateTime? fulfilled;
                fulfilled = Convert.ToDateTime(data[3]);

                string option = data[4];
                int scoops = Convert.ToInt32(data[5]);
                //Converted dipped in object creation
                string? waffleFlavour = data[7];
                Flavour flavour1 = FlavourDic[data[8]];
                flavour1.Quantity++;
                Flavour? flavour2 = null;
                Flavour? flavour3 = null;
                if (data[9] == "")
                {
                    data[9] = null;
                }
                if (data[10] == "")
                {
                    data[10] = null;
                }
                if (data[9] == data[8]) { flavour1.Quantity++; }
                else
                {
                    if (flavour2 != null)
                    {
                        flavour2 = FlavourDic[data[9]];
                        if (data[10] == flavour2.Type)
                        {
                            flavour2.Quantity++;
                        }
                    }
                }
                if (data[10] == data[8]) { flavour1.Quantity++; }
                else
                {
                    if (flavour3 != null)
                    {
                        flavour3 = FlavourDic[data[10]];
                        flavour3.Quantity++;
                    }
                }
                List<Flavour> fList = new List<Flavour>();
                fList.Add(flavour1);
                if (flavour2 != null)
                {
                    flavour2.Quantity++;
                    fList.Add(flavour2);
                }
                if (flavour3 != null)
                {
                    flavour3.Quantity++;
                    fList.Add(flavour3);
                }
                List<Topping> tList = new List<Topping>();
                for (int i = 11; i < 15; i++)
                {
                    if (data[i] != "")
                    {
                        tList.Add(new Topping(data[i]));
                    }
                }
                IceCream ic;
                if (option == "Cup")
                {
                    ic = new Cup(option, scoops, fList, tList);
                }
                else if (option == "Cone")
                {
                    bool dipped = Convert.ToBoolean(data[6]);
                    ic = new Cone(option, scoops, fList, tList, dipped);
                }
                else
                {
                    ic = new Waffle(option, scoops, fList, tList, waffleFlavour);
                }
                                                
                Order pOrder = new Order(OrderId, Recieved);
                pOrder.TimeFulfilled = fulfilled;
                bool check = true;
                foreach (Order order in cusDic[Memid].OrderHistory)
                {
                    if (order.Id == OrderId)
                    {
                        order.IceCreamList.Add(ic);
                        check = false;
                        break;
                    }
                }
                if (check)
                {
                    pOrder.IceCreamList.Add(ic);
                    cusDic[Memid].OrderHistory.Add(pOrder);
                }
                orderCount = OrderId;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);                
            }   
        }
    }
    return orderCount;
}
int DataValidationInt(string prompt, List<int> listOfValues)
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
void DisplayMenu()
{
    Console.WriteLine();
    Console.WriteLine("Ice Cream Shop Management System");
    Console.WriteLine("===============================================\n" +
                      "[1] List all customers\n" +
                      "[2] List all current orders\n" +
                      "[3] Register a new customer\n" +
                      "[4] Create a customer’s order\n" +
                      "[5] Display order details of a customer\n" +
                      "[6] Modify order details\n" +
                      "[7] Process an order and checkout\n" +
                      "[8] Display monthly charged amounts breakdown\n" +
                      "    & total charged amounts for the year\n" +
                      "[0] Exit\n" +
                      "===============================================\n");
}

void ListCustomers()
{
    using (StreamReader sr = new StreamReader("customers.csv"))
    {
        string? s = sr.ReadLine();
        if (s != null)
        {
            string[] heading = s.Split(',');
            Console.WriteLine("{0,-10} {1,-10} {2,-15} {3,-20} {4,-20} {5,-10}",
                heading[0], heading[1], heading[2], heading[3], heading[4], heading[5]);
        }
        while ((s = sr.ReadLine()) != null)
        {
            string[] customer = s.Split(',');
            Console.WriteLine("{0,-10} {1,-10} {2,-15} {3,-20} {4,-20} {5,-10}",
                customer[0], customer[1], customer[2], customer[3], customer[4], customer[5]);
        }
    }
    Console.WriteLine();
}
IceCream CreateIceCream()
{
    //Creates IceCream object based on Option 
    IceCream IceCreamType()
    {
        Console.WriteLine("Enter the option of the type of Ice Cream you want:");
        Console.WriteLine("[1] Cup\r\n[2] Cone\r\n[3] Waffle");
        List<int> IceCreamOption = new List<int>() { 1, 2, 3 };

        while (true)
        {
            try
            {
                int OptionOption = DataValidationInt("Enter your Option Number: ", IceCreamOption);
                if (OptionOption == 1)
                {

                    Cup newIceCream = new Cup();

                    ScoopFlavour(newIceCream);
                    IceCreamTopping(newIceCream);
                    return newIceCream;
                }
                else if (OptionOption == 2)
                {
                    Cone newIceCream = new Cone();
                    ScoopFlavour(newIceCream);
                    IceCreamTopping(newIceCream);
                    ConeDipped(newIceCream);
                    return newIceCream;
                }
                else if (OptionOption == 3)
                {
                    Waffle newIceCream = new Waffle();
                    ScoopFlavour(newIceCream);
                    IceCreamTopping(newIceCream);
                    WaffleFlavour(newIceCream);
                    return newIceCream;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    //Creating the Falvour
    void ScoopFlavour(IceCream newIceCream)
    {
        int scoops = 0;
        while (scoops < 3)
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
            while (true)
            {
                try
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

                    int option = DataValidationInt("Enter Your Flavour Number: ", new List<int>() { 1, 2, 3, 4, 5, 6 });
                    if (FlavourDic.ContainsKey(option))
                    {
                        int FlavourScoopNum = DataValidationInt($"Enter the number of Scoops of {FlavourDic[option]}: "
                            , new List<int> { 1, 2, 3 });
                        while ((FlavourScoopNum + scoops) > 3)
                        {
                            Console.WriteLine("You cannot have more than 3 scoops per Ice Cream.");
                            FlavourScoopNum = DataValidationInt($"Enter the number of Scoops of {FlavourDic[option]}: "
                            , new List<int> { 1, 2, 3 });
                        }
                        scoops += FlavourScoopNum;
                        if (FlavourScoopNum != 0)
                        {
                            if (option == 4 || option == 5 || option == 6)
                            {
                                newIceCream.Flavours.Add(new Flavour(FlavourDic[option], true, FlavourScoopNum));
                            }
                            else
                            {
                                newIceCream.Flavours.Add(new Flavour(FlavourDic[option], false, FlavourScoopNum));
                            }
                        }
                        int AddMoreFlavourOption = DataValidationInt("Enter '1' to add more flavours and '0' To exit: ", new List<int> { 0, 1 });
                        if (AddMoreFlavourOption == 0 || scoops == 3)
                        {
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Enter a Vaild Option.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            break;
        }
        newIceCream.Scoop = scoops;
    }

    //Creating the Toppings
    void IceCreamTopping(IceCream newIceCream)
    {
        Console.WriteLine(
        "Toppings (+1 each)\r\n" +
        "[1]Sprinkles\r\n" +
        "[2]Mochi\r\n" +
        "[3]Sago\r\n" +
        "[4]Oreos\r\n" +
        "[5]Exit/None");

        Dictionary<int, Topping> ToppingsDic = new Dictionary<int, Topping>
        {
            { 1, new Topping("Sprinkles") },
            { 2, new Topping("Mochi") },
            { 3, new Topping("Sago") },
            { 4, new Topping("Oreos") }
        };

        while (newIceCream.Toppings.Count < 4)
        {
            int ToppingOption = DataValidationInt("Enter the option number for the topping you want:", new List<int> { 1, 2, 3, 4, 5 });
            if (ToppingOption == 1 || ToppingOption == 2 || ToppingOption == 3
                || ToppingOption == 4 || ToppingOption == 5)
            {
                if (ToppingOption == 5)
                {
                    break;
                }
                newIceCream.Toppings.Add(ToppingsDic[ToppingOption]);

                if (newIceCream.Toppings.Count == 4)
                {
                    Console.WriteLine("Max Toppings Amount Limit Reached.");
                    break;
                }
                else
                {
                    Console.WriteLine($"Number of toppings left: {4 - newIceCream.Toppings.Count}");
                }
            }
            else
            {
                Console.WriteLine("Enter a valid option.");
            }
        }
    }

    void ConeDipped(Cone newIceCream)
    {
        Console.Write("Do you want your Cone to be Dipped in Chocolate (Y/N): ");
        string reply = Console.ReadLine().ToUpper();
        while (reply != "Y" && reply != "N")
        {
            Console.WriteLine("Invalid input");
            Console.Write("Do you want your Cone to be Dipped in Chocolate (Y/N): ");
            reply = Console.ReadLine().ToUpper();
        }

        if (reply == "Y")
        {
            newIceCream.Dipped = true;
        }
        else if (reply == "N")
        {
            newIceCream.Dipped = false;
        }
    }

    void WaffleFlavour(Waffle newIceCream)
    {
        Dictionary<int, string> waffleFalvourDic = new Dictionary<int, string>
        {
            {1, "Basic" },
            {2, "Red Velvet" },
            {3, "Charcoal" },
            {4, "Pandan" }
        };
        Console.WriteLine(
            "Waffle Flavours:\n" +
            "[1] Basic\n" +
            "[2] Red Velvet (+$3)\n" +
            "[3] Charcoal (+$3)\n" +
            "[4] Pandan (+$3)");

        while (true)
        {
            try
            {
                int WaffleFlavourOption = DataValidationInt("Enter your Waffle Flavour Option: ", new List<int> { 1, 2, 3, 4 });
                newIceCream.WaffleFlavour = waffleFalvourDic[WaffleFlavourOption];
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    IceCream newIceCream = IceCreamType();
    //Returns IceCream
    return newIceCream;
}
void DisplayBreakDown(Dictionary<int, Customer> CustomerDic)
{
    Dictionary<int, double> monthDic = new Dictionary<int, double>
    {
        {1,0},
        {2,0},
        {3,0},
        {4,0},
        {5,0},
        {6,0},
        {7,0},
        {8,0},
        {9,0},
        {10,0},
        {11,0},
        {12,0}
    };

    while (true)
    {
        try
        {
            Console.Write("Enter the year: ");
            int year = Convert.ToInt32(Console.ReadLine());
            while (year < DateTime.MinValue.Year && year > DateTime.MaxValue.Year)
            {
                Console.WriteLine("Invalid Input.");
                Console.Write("Enter the year: ");
                year = Convert.ToInt32(Console.ReadLine());
            }

            foreach (var kvp in CustomerDic)
            {
                foreach (Order o in kvp.Value.OrderHistory)
                {
                    if (o.TimeRecieved.Year == year)
                    {
                        monthDic[o.TimeRecieved.Month] += o.CalculateTotal();
                    }
                }
            }
            if (monthDic.Values.Sum() == 0)
            {
                Console.WriteLine("There were no orders this year.");
            }
            else
            {
                foreach (var entry in monthDic)
                {
                    string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(entry.Key);
                    Console.WriteLine($"{monthName,-15}: ${entry.Value:0.00}");
                }
                Console.WriteLine($"Total: ${monthDic.Values.Sum():0.00}");
            }
            break;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    


}

while (true)
{
    DisplayMenu();
    int opt = DataValidationInt("Enter your option: ", new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 }); 
    if (opt == 0)
    {
        Console.WriteLine("Exiting...");
        break;
    }
    //Question 1 (Joseph)
    else if (opt == 1)
    {
        ListCustomers();
    }
    //Question 2
    else if (opt == 2)
    {
        int count = 0;
        Console.WriteLine("Golden Queue");
        Console.WriteLine("================================================");
        foreach (Order o in goldOrderQueue)
        {
            Console.WriteLine(o);
            count++;
        }
        Console.WriteLine("================================================");
        Console.WriteLine("Regular Queue");
        Console.WriteLine("================================================");
        foreach (Order o in regularOrderQueue)
        {
            Console.WriteLine(o);
            count++;
        }
        Console.WriteLine("================================================");
        if (count == 0)
        {
            Console.WriteLine("Currently, there are no unfulfilled orders.");
        }
    }
    //Question 3 (Joseph)
    else if (opt == 3)
    {
        string name;
        int id;
        DateTime dob;
        while (true)
        {
            //Retrieving Name
            Console.Write("Enter your name: ");
            name = Console.ReadLine();
            if (name == "")
            {
                Console.WriteLine("Please enter a name!");
                Console.WriteLine();
                continue;
            }
            break;
        }
        while (true)
        {
            try
            {
                //Retrieving ID
                Console.Write("Enter your id number:");
                id = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine();
                continue;
            }
            break;
        }
        while (true)
        {
            try
            {
                //Retrieving Date of Birth
                Console.Write("Enter your Date of Birth (dd/mm/yyyy) : ");
                dob = Convert.ToDateTime(Console.ReadLine());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                Console.WriteLine();
                continue;
            }
            break;
        }
        //Storing info in new customer object
        Customer newCustomer = new Customer(name, id, dob);

        //Creating a new pointcard object 
        PointCard newCard = new PointCard(0, 0);
        newCustomer.Rewards = newCard;
        //Appending new data to customers.csv file
        using (StreamWriter sw = new StreamWriter("customers.csv", true))
        {
            sw.WriteLine(name + "," + Convert.ToString(id) + "," + dob.ToString("dd/MM/yyyy") + "," + "Ordinary" + "," + newCard.Points + "," + newCard.PunchCard);
        }
        CustomerDic.Add(newCustomer.MemberId, newCustomer);
        Console.WriteLine("New customer added!");
    }
    //Question 4 (Joseph)
    else if (opt == 4)
    {
        ListCustomers();
        int custId;
        while (true)
        {
            try
            {
                Console.Write("Enter customer Id: ");
                custId = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine();
                continue;
            }

            if (CustomerDic.ContainsKey(custId))
            {
                Order newOrder = CustomerDic[custId].MakeOrder();                
                newOrder.Id = orderCount;
                orderCount++;
                newOrder.TimeRecieved = DateTime.Now;
                while (true)
                {
                    IceCream newIceCream = CreateIceCream();
                    newOrder.AddIceCream(newIceCream);                                              //Adding ice cream to order
                    Console.Write("Would you like to add another ice cream to the order? (Y/N): ");//Asking user if they make to make another ice 
                    string ans = Console.ReadLine().ToUpper();
                    while (ans != "Y" && ans!= "N")  
                    {
                        Console.WriteLine("Invalid input");
                        Console.Write("Would you like to add another ice cream to the order? (Y/N): ");
                        ans = Console.ReadLine().ToUpper();
                    }
                    
                    if (ans == "Y")
                    {
                        continue;
                    }   
                    else if (ans == "N")
                    {
                        CustomerDic[custId].CurrentOrder = newOrder;
                        if (CustomerDic[custId].Rewards.Tier == "Gold")
                        {
                            goldOrderQueue.Enqueue(CustomerDic[custId].CurrentOrder);
                        }
                        else
                        {
                            regularOrderQueue.Enqueue(CustomerDic[custId].CurrentOrder);
                        }
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("ID does not match with database");
                Console.WriteLine("Please enter the correct ID");
                Console.WriteLine();
                continue;
            }
            Console.WriteLine("Order added successfully!");
            break;
        }
 
    }

    //Question 5
    else if (opt == 5)
    {
        foreach (KeyValuePair<int, Customer> customer in CustomerDic)
        {
            Console.WriteLine($"{customer.Value.MemberId}: {customer.Value.Name}");
        }

        int id;
        Console.Write("Enter your Customer ID: ");
        id = Convert.ToInt32(Console.ReadLine());
        while (!CustomerDic.ContainsKey(id))
        {
            Console.WriteLine("Invalid Id.");
            Console.Write("Enter your Customer ID: ");
            id = Convert.ToInt32(Console.ReadLine());
        }
        foreach (Order order in CustomerDic[id].OrderHistory)
        {
            Console.WriteLine(order);
        }
        if (CustomerDic[id].CurrentOrder != null)
        {
            if (CustomerDic[id].CurrentOrder.IceCreamList.Count != 0)
            {
                Console.WriteLine(CustomerDic[id].CurrentOrder);
            }
        }
        
    }
    //Question 6
    else if (opt == 6)
{
    foreach (KeyValuePair<int, Customer> customer in CustomerDic)
    {
        Console.WriteLine($"{customer.Value.MemberId}: {customer.Value.Name}");
    }

    int id;
    Console.Write("Enter your Customer ID: ");
    id = Convert.ToInt32(Console.ReadLine());
    while (!CustomerDic.ContainsKey(id))
    {
        Console.WriteLine("Invalid Id.");
        Console.Write("Enter your Customer ID: ");
        id = Convert.ToInt32(Console.ReadLine());
    }

    if (CustomerDic[id].CurrentOrder != null)
    {
        Order CurrentOrder = CustomerDic[id].CurrentOrder;


        Console.WriteLine(
            "Modify Order Details:\r\n" +
            "[1] Choose Ice Cream to Modify\r\n" +
            "[2] Add New Ice Cream\r\n" +
            "[3] Remove Ice Cream from your Order");

        int Option6Option;

        while (true)
        {
            try
            {
                Console.Write("Enter your option: ");
                Option6Option = DataValidationInt("Enter your option: ",
                    new List<int> { 1, 2, 3 });
                switch (Option6Option)
                {
                    case 1:
                        int count = 1;
                        foreach (IceCream ic in CurrentOrder.IceCreamList)
                        {
                            Console.WriteLine($"[{count}]: \n{ic}");
                            count++;
                        }
                        Console.Write("Enter which Ice Cream you want to change: ");
                        int ChangeIceCreamID = Convert.ToInt16(Console.ReadLine());
                        CurrentOrder.ModifyIceCream(ChangeIceCreamID);
                        break;
                    case 2:
                        IceCream NewIceCream = CreateIceCream();
                        CurrentOrder.AddIceCream(NewIceCream);
                        Console.WriteLine(NewIceCream);
                        break;

                    case 3:
                        foreach (IceCream ic in CurrentOrder.IceCreamList)
                        {
                            Console.WriteLine(ic);
                        }
                        Console.Write("Enter which Ice Cream you want to Delete: ");
                        int DeleteIceCreamID = Convert.ToInt16(Console.ReadLine());
                        CurrentOrder.DeleteIceCream(DeleteIceCreamID);
                        break;
                }
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
    else
    {
        Console.WriteLine("You do not have an order to modify. Place an order first.");
    }
}
    //Advance Question 1
    //Advance Question 2
    else if (opt == 8)
    {
        DisplayBreakDown(CustomerDic);
    }
}