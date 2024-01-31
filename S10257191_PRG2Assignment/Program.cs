// See https://aka.ms/new-console-template for more information
using S10257191_PRG2Assignment;

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Transactions;
using System.Xml.Schema;
using System.Globalization;

//==========================================================
// Student Number : S10241860
// Student Name : Joseph Kwek Jun Yan
// Partner Name : Vallimuthu Rishikesh
//==========================================================

//creating customer dictionary to store information of customers
Dictionary<int, Customer> CustomerDic = new Dictionary<int, Customer>();

//Create flavor dictionary to store information of the flavors
Dictionary<string, bool> FlavourDic = new Dictionary<string, bool>
{
    { "Vanilla", false },
    { "Chocolate", false },
    { "Strawberry", false },
    { "Durian", true },
    { "Ube", true },
    { "Sea Salt", true },
};

//Creating Queues for orders
Queue<Order> regularOrderQueue = new Queue<Order>();
Queue<Order> goldOrderQueue = new Queue<Order>();
int orderCount = 0;

//initializing Custoemr and Orders
InnitCustomer(CustomerDic);
//TO keep count of Order ID so that newly created orders can have continuous values
orderCount = InnitOrders(CustomerDic, orderCount);
orderCount++;

//Read from Custoemr.csv and append data into CustomerDIc
void InnitCustomer(Dictionary<int, Customer> cusDic)
{
    using (StreamReader sr = new StreamReader("customers.csv"))
    {
        string? s = sr.ReadLine(); // read the heading

        while ((s = sr.ReadLine()) != null)
        {
            string[] data = s.Split(',');
            //Data in file
            //Name, MemberId, DOB, MembershipStatus(Tier), MembershipPoints, PunchCard
            try
            {
                //Converting into Correct Data Types
                int id = Convert.ToInt32(data[1]);
                DateTime dob = Convert.ToDateTime(data[2]);
                //Adding Customer To Dic
                cusDic.Add(id, new Customer(data[0], id, dob));
                //Data for the PointCard object in Custoemr object
                cusDic[id].Rewards.Points = Convert.ToInt32(data[4]);
                cusDic[id].Rewards.PunchCard = Convert.ToInt32(data[5]);
                cusDic[id].Rewards.Tier =Convert.ToString(data[3]);
            }
            catch
            {
                //Most Likely Cause for an error
                Console.WriteLine("Invalid Data in csv files.");
            }
        }
    }
}

//Read from Order.csv and add to CustomerDic's order histories
int InnitOrders(Dictionary<int, Customer> cusDic, int orderCount)
{
    using (StreamReader sr = new StreamReader("orders.csv"))
    {
        string? s = sr.ReadLine(); // read the heading

        while ((s = sr.ReadLine()) != null)
        {
            string?[] data = s.Split(',');
            //Data and their indexes
            //Id[0], MemberId[1], TimeReceived[2], TimeFulfilled[3], Option[4], Scoops[5],
            //Dipped[6], WaffleFlavour[7],
            //Flavour1[8], Flavour2[9], Flavour3[10],
            //Topping1[11], Topping2[12], Topping3[13], Topping4[14]

            try
            {
                //Converting to Correct Data Types
                int OrderId = Convert.ToInt32(data[0]);
                orderCount = OrderId;
                int Memid = Convert.ToInt32(data[1]);
                DateTime Recieved = Convert.ToDateTime(data[2]);               
                DateTime fulfilled = Convert.ToDateTime(data[3]);
                string option = data[4];
                int scoops = Convert.ToInt32(data[5]);
                //Converted dipped in object creation
                string? waffleFlavour = data[7];       
                
                //Creating teh falvours
                List<Flavour> fList = new List<Flavour>();
                //Loop through the data array for the indexes that contain flavour data
                for (int i = 8; i <= 10; i++)
                {                    
                    if (!string.IsNullOrEmpty(data[i])) //IsNullOrEmpty to check if ther eis a flavour or not in that data spot
                    {
                        bool inFlist = false;
                        //Checks if flavour is already in the list so that there are no duplicated list Vanilla(1), Vanilla(1)
                        foreach (Flavour f in fList)
                        {
                            if (f.Type == data[i])
                            {
                                inFlist = true;
                                f.Quantity++;
                                break;
                            }
                        }
                        //If flavour does not already exsist, we add a that flavour object to the list
                        if (!inFlist)
                        {
                            Flavour flavour = new Flavour(data[i], FlavourDic[data[i]], 0);
                            flavour.Quantity++;
                            fList.Add(flavour);
                        }                                               
                    }
                }

                //Same thing for topping as flavour
                List<Topping> tList = new List<Topping>();
                for (int i = 11; i < 15; i++)
                {
                    if (data[i] != "")
                    {
                        tList.Add(new Topping(data[i]));
                    }
                }
                //Creating the IceCream object with the data read
                IceCream ic;
                //Downcasts based on the option of the icecream
                //and appends appropriate data based on icecream option
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
                //Creating the order obeject that will be put into orderhistory                            
                Order pOrder = new Order(OrderId, Recieved);
                pOrder.TimeFulfilled = fulfilled;
                
                bool check = true;
                //Checks if the Order Id of the current line of csv already exsists in the customers
                //OrderHistory

                //If it does, we add the current IceCream to the IceCream list of the order History
                foreach (Order order in cusDic[Memid].OrderHistory)
                {
                    if (order.Id == OrderId)
                    {
                        order.IceCreamList.Add(ic);                        
                        check = false;
                        break;
                    }
                }
                //If it doesn't we add the ice order and append the order to the order history
                if (check)
                {
                    pOrder.IceCreamList.Add(ic);
                    cusDic[Memid].OrderHistory.Add(pOrder);
                }
                //Keep count of the orderID for future orders
                orderCount = OrderId;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);                
            }   
        }
    }
    //Checks if the order data fulfilled is a birthday to apply discount
    double IsBirthDay(Order order, int day, int month)
    {
        double discount = 0;
        if (order.TimeFulfilled.HasValue && order.TimeFulfilled.Value.Day == day && order.TimeFulfilled.Value.Month == month)
        {
            foreach (IceCream ic in order.IceCreamList)
            {
                if (ic.CalculatePrice() > discount)
                {
                    discount = ic.CalculatePrice();
                }
            }
        }
        return discount;
    }
    //Calculation the final total for the orders
    foreach (Customer c in cusDic.Values)
    {
        foreach (Order o in c.OrderHistory)
        {
            o.FinalTotal = o.CalculateTotal() - IsBirthDay(o,c.DOB.Day,c.DOB.Month);
        }
    }
    return orderCount;
}

//Data Validation method that works for integer inputs
//Takes in input promt and list of allowable values
int DataValidationInt(string prompt, List<int> listOfValues)
{
    int option;
    while (true)
    {
        try
        {
            //Asks for input and converts it into int 
            Console.Write(prompt);
            option = Convert.ToInt32(Console.ReadLine());
            //If the option is not inside the range of allowable values
            //Ask for input again
            while (!(listOfValues.Contains(option)))
            {
                Console.WriteLine("Invalid Input.");
                Console.Write(prompt);
                option = Convert.ToInt32(Console.ReadLine());
            }
            break;
        }
        catch//Most likely to be called when converting innput to int 
        {
            Console.WriteLine("Enter a whole number.");
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
    Console.WriteLine("{0,-10} {1,-10} {2,-15} {3,-20} {4,-20} {5,-10}",
                "Name", "MemberID", "DOB", "Membership Status", "Points", "Punchcard");
    foreach (Customer customer in CustomerDic.Values)
    {
        Console.WriteLine("{0,-10} {1,-10} {2,-15} {3,-20} {4,-20} {5,-10}",
                customer.Name, customer.MemberId, customer.DOB.ToString("dd/MM/yyyy"), customer.Rewards.Tier, customer.Rewards.Points, customer.Rewards.PunchCard);
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
        //Displays options 
        while (true)
        {
            try
            {
                //Creates object based on option and input the
                //appropritate data based on type like dipepd and waffle flavour

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
            if (scoops == 3)//Checks the numebr of scoops and stops the code if there are 3
            {
                Console.WriteLine("Ice Cream can only have 3 sscoops.");
                break;
            }
            //Display Options
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
                    //Flavour Dictionary
                    Dictionary<int, string> FlavourDic = new Dictionary<int, string>
                    {
                    { 1, "Vanilla" },
                    { 2, "Chocolate" },
                    { 3, "Strawberry" },
                    { 4, "Durian" },
                    { 5, "Ube" },
                    { 6, "Sea Salt" }
                    };
                    //Ask for input on flavour and the number of scoops it can have
                    int option = DataValidationInt("Enter Your Flavour Number: ", new List<int>() { 1, 2, 3, 4, 5, 6 });
                    int FlavourScoopNum = DataValidationInt($"Enter the number of Scoops of {FlavourDic[option]}: "
                            , new List<int> { 1, 2, 3 });
                    while ((FlavourScoopNum + scoops) > 3)//Checks if number of scoops is allowed after adding current scoops 
                    {
                        Console.WriteLine("You cannot have more than 3 scoops per Ice Cream.");
                        FlavourScoopNum = DataValidationInt($"Enter the number of Scoops of {FlavourDic[option]}: "
                        , new List<int> { 1, 2, 3 });
                    }
                    //Update Scoops amount
                    scoops += FlavourScoopNum;
                    //Craetes flavour object and adds it to falvour list in icecream
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
                    //Ask if user wants more 
                    int AddMoreFlavourOption = DataValidationInt("Enter '1' to add more flavours and '0' To exit: ", new List<int> { 0, 1 });
                    //Prints statement based on input and if scoops is 3
                    if (AddMoreFlavourOption == 0 || scoops == 3)
                    {
                        if (scoops == 3)
                        {
                            Console.WriteLine("Ice Cream can only have 3 sscoops.");                            
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
        }
        newIceCream.Scoop = scoops;
    }

    //Creating the Toppings
    void IceCreamTopping(IceCream newIceCream)
    {
        //Display Options
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
        //Checks if can add more topping
        while (newIceCream.Toppings.Count < 4)
        {
            int ToppingOption = DataValidationInt("Enter the option number for the topping you want:", new List<int> { 1, 2, 3, 4, 5 });
            if (ToppingOption == 5)//Break to exit topping selection
            {
                break;
            }
            //Adds topping to topping list in ice cream
            newIceCream.Toppings.Add(ToppingsDic[ToppingOption]);
            
            //Unable to add more toppings 
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
    }

    //If IceCream is a Cone, this method is called
    void ConeDipped(Cone newIceCream)
    {
        //Asks for input
        Console.Write("Do you want your Cone to be Dipped in Chocolate (Y/N): ");
        string reply = Console.ReadLine().ToUpper();
        //Data Validation
        while (reply != "Y" && reply != "N")
        {
            Console.WriteLine("Invalid input");
            Console.Write("Do you want your Cone to be Dipped in Chocolate (Y/N): ");
            reply = Console.ReadLine().ToUpper();
        }
        //Setting Dipped Value
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
        //Displays Options
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
                //Ask for input
                int WaffleFlavourOption = DataValidationInt("Enter your Waffle Flavour Option: ", new List<int> { 1, 2, 3, 4 });
                //Sets waffle flavour
                newIceCream.WaffleFlavour = waffleFalvourDic[WaffleFlavourOption];
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    //Created IceCream Object and runs necessacry methods
    IceCream newIceCream = IceCreamType();
    //Returns IceCream
    return newIceCream;
}
void DisplayBreakDown(Dictionary<int, Customer> CustomerDic)
{
    //Dictionary that has key(month value in int) and value(monthly charged amounts)
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
            //Input and data validation for year
            Console.Write("Enter the year: ");
            int year = Convert.ToInt32(Console.ReadLine());         
            while (year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year)
            {
                Console.WriteLine("Invalid Input.");
                Console.Write("Enter the year: ");
                year = Convert.ToInt32(Console.ReadLine());
            }
            //Loops through all orders for all customers 
            foreach (var kvp in CustomerDic)
            {                 
                foreach (Order o in kvp.Value.OrderHistory)
                {
                    //If the order fulfilled year is the same, we add the total of that orde to the month of the order
                    if (o.TimeFulfilled.HasValue && o.TimeFulfilled.Value.Year == year)
                    {
                        monthDic[o.TimeFulfilled.Value.Month] += o.FinalTotal;
                    }
                }
            }
            //Display nothing if there were no orders
            if (monthDic.Values.Sum() == 0)
            {
                Console.WriteLine("There were no orders this year.");
            }
            //Display breakdown amounts
            else
            {
                Console.WriteLine();
                Console.WriteLine($"{year} Breakdown:");
                foreach (var entry in monthDic)
                {
                    string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(entry.Key);
                    Console.WriteLine($"{monthName,-10}: ${entry.Value:0.00}");
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

string appendFlavour(IceCream ice)
{
    string order;
    if (ice.Scoop == 1)
    {
        order = "," + ice.Flavours[0].Type +",,";
        return order;
    }
    else if (ice.Scoop == 2)
    {
        if (ice.Flavours[0].Quantity == 2)
        {
            order = "," + ice.Flavours[0].Type + "," + ice.Flavours[0].Type + ",";
            return order;
        }
        else
        {
            order = "," + ice.Flavours[0].Type + "," + ice.Flavours[1].Type + ",";
            return order;
        }
    }
    else if (ice.Scoop == 3)
    {
        if (ice.Flavours.Count() == 3)
        {
            order = "," + ice.Flavours[0].Type + "," + ice.Flavours[1].Type + "," + ice.Flavours[2].Type;
            return order;
        }
        else if (ice.Flavours.Count() == 2)
        {
            if (ice.Flavours[0].Quantity == 2)
            {
                order = "," + ice.Flavours[0].Type + "," + ice.Flavours[0].Type + "," + ice.Flavours[1].Type;
                return order;
            }
            else
            {
                order = "," + ice.Flavours[0].Type + "," + ice.Flavours[1].Type + "," + ice.Flavours[1].Type;
                return order;
            }
        }
        else
        {
            order = "," + ice.Flavours[0].Type + "," + ice.Flavours[0].Type + "," + ice.Flavours[0].Type;
            return order; ;
        }
    }
    else
    {
        return ",,,";
    }

}
string appendToppings(IceCream ice)
{
    string order;
    if (ice.Toppings.Count() == 1)
    {
        order = "," + ice.Toppings[0].Type + ",,,";
        return order;
    }
    else if (ice.Toppings.Count() == 2)
    {
        order = "," + ice.Toppings[0].Type + "," + ice.Toppings[1].Type + ",,";
        return order;
    }
    else if (ice.Toppings.Count() == 3)
    {
        order = "," + ice.Toppings[0].Type + "," + ice.Toppings[1].Type + "," + ice.Toppings[2].Type + ",";
        return order;
    }
    else if (ice.Toppings.Count() == 4)
    {
        order = "," + ice.Toppings[0].Type + "," + ice.Toppings[1].Type + "," + ice.Toppings[2].Type + "," + ice.Toppings[3].Type;
        return order;
    }
    else
        return ",,,,";
}
void appendOrder(Customer cust)
{
    string order;
    using (StreamWriter sw = new StreamWriter("orders.csv", true))
    {
        sw.WriteLine();
        foreach (IceCream ice in cust.CurrentOrder.IceCreamList)
        {
            order = cust.CurrentOrder.Id + "," + cust.MemberId + "," + cust.CurrentOrder.TimeRecieved + "," + cust.CurrentOrder.TimeFulfilled;
            if (ice.Option == "Cup")
            {
                order += "," + ice.Option + "," + ice.Scoop + ",,";
                order += appendFlavour(ice);
                order += appendToppings(ice);

                sw.WriteLine(order);
            }
            else if (ice.Option == "Cone")
            {
                Cone c = (Cone)ice;
                order += "," + c.Option + "," + c.Scoop + "," + c.Dipped + ",";
                order += appendFlavour(ice);
                order += appendToppings(ice);

                sw.WriteLine(order);
            }
            else if (ice.Option == "Waffle")
            {
                Waffle w = (Waffle)ice;
                order += "," + w.Option + "," + w.Scoop + ",," + w.WaffleFlavour;
                order += appendFlavour(ice);
                order += appendToppings(ice);

                sw.WriteLine(order);
            }
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
    //Question 2 (Rishikesh)
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
        if (count == 0)//Confirmation if there are no orders
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
                while (CustomerDic.ContainsKey(id))
                {
                    Console.WriteLine("Customer with that ID already exists.");
                    Console.Write("Enter your id number:");
                    id = Convert.ToInt32(Console.ReadLine());
                }
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
                    while (ans != "Y" && ans != "N")
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

    //Question 5(Rishikesh)
    else if (opt == 5)
    {
        //Displays customer name and id
        foreach (KeyValuePair<int, Customer> customer in CustomerDic)
        {
            Console.WriteLine($"{customer.Value.MemberId}: {customer.Value.Name}");
        }
        //Input and validation
        int id;
        while (true)
        {
            try
            {
                Console.Write("Enter your Customer ID: ");
                id = Convert.ToInt32(Console.ReadLine());
                while (!CustomerDic.ContainsKey(id))
                {
                    Console.WriteLine("Invalid Id.");
                    Console.Write("Enter your Customer ID: ");
                    id = Convert.ToInt32(Console.ReadLine());
                }
                break;
            }
            catch
            {
                Console.WriteLine("Invalid Input.");
            }
        }
        int count = 0;
        //Prints past orders
        foreach (Order order in CustomerDic[id].OrderHistory)
        {
            Console.WriteLine(order);
            count++;
        }
        //Prints current order if there is one
        if (CustomerDic[id].CurrentOrder != null)
        {
            if (CustomerDic[id].CurrentOrder.IceCreamList.Count != 0)
            {
                Console.WriteLine(CustomerDic[id].CurrentOrder);
                count++;
            }
        }
        //Prints if there is no order to inform user
        if (count == 0) { Console.WriteLine("This Customer Has No Past Or Present Order."); }
    }
    //Question 6(Rishikesh)
    else if (opt == 6)
    {
        //Display Cusomers and thier IDs
        foreach (KeyValuePair<int, Customer> customer in CustomerDic)
        {
            Console.WriteLine($"{customer.Value.MemberId}: {customer.Value.Name}");
        }
        //ID input and validation
        int id;
        while (true)
        {
            try
            {
                Console.Write("Enter your Customer ID: ");
                id = Convert.ToInt32(Console.ReadLine());
                while (!CustomerDic.ContainsKey(id))
                {
                    Console.WriteLine("Invalid Id.");
                    Console.Write("Enter your Customer ID: ");
                    id = Convert.ToInt32(Console.ReadLine());
                }
                break;
            }
            catch
            {
                Console.WriteLine("Enter A Valid Customer ID.");
            }
        }        
        //Checks if chosen customer has a current order and break if there is none
        if (CustomerDic[id].CurrentOrder != null)
        {
            Order CurrentOrder = CustomerDic[id].CurrentOrder;

            //Display Options
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
                    //Input
                    Console.Write("Enter your option: ");
                    Option6Option = DataValidationInt("Enter your option: ",
                        new List<int> { 1, 2, 3 });
                    switch (Option6Option)
                    {
                        case 1:
                            int count = 1;
                            Console.WriteLine();
                            //Displays Current IceCreams
                            List<int> list = new List<int>();
                            foreach (IceCream ic in CurrentOrder.IceCreamList)
                            {
                                Console.WriteLine($"[{count}]: \n{ic}\n");
                                count++;
                            }
                            //Populate List for Data Validation
                            for (int i = 1; i < count; i++) { list.Add(i); }
                            //Ask for input
                            int ChangeIceCreamID = DataValidationInt("Enter which Ice Cream you want to change: ", list);
                            //Call Method in Order.cs
                            CurrentOrder.ModifyIceCream(ChangeIceCreamID);
                            break;
                        case 2:
                            //Create IceCream
                            IceCream NewIceCream = CreateIceCream();
                            //Add IceCream to the list of order
                            CurrentOrder.AddIceCream(NewIceCream);
                            //Display new IceCream
                            Console.WriteLine(NewIceCream);
                            break;

                        case 3:
                            if (CurrentOrder.IceCreamList.Count == 1)
                            {
                                Console.WriteLine("Cannot Remove IceCream as there cannot be 0 IceCreams in a order.");
                                break;
                            }
                            //Display IceCreams 
                            int count2 = 1;
                            List<int> list2 = new List<int>();
                            Console.WriteLine();
                            foreach (IceCream ic in CurrentOrder.IceCreamList)
                            {
                                Console.WriteLine($"[{count2}]: \n{ic}\n");
                                count2++;
                            }
                            //Populate List for Data Validation
                            for (int i = 1; i < count2; i++) { list2.Add(i); }
                            //Input and Validation
                            int DeleteIceCreamID = DataValidationInt("Enter which Ice Cream you want to Delete: ", list2);
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
    //Advance Question 1 (Joseph)
    else if (opt == 7)
    {
        if (goldOrderQueue.Count() > 0)
        {
            List<IceCream> list = (goldOrderQueue.Dequeue().IceCreamList);
            Console.WriteLine("Ice Cream order");
            foreach (int k in CustomerDic.Keys)
            {   
                if (CustomerDic[k].CurrentOrder == null)
                {
                    continue;
                }
                else if (CustomerDic[k].CurrentOrder.IceCreamList == list)
                {
                    Console.WriteLine(CustomerDic[k].CurrentOrder);
                    double total = CustomerDic[k].CurrentOrder.CalculateTotal();
                    Console.WriteLine("Membership Tier: {0}\n" +
                                      "Points: {1}", CustomerDic[k].Rewards.Tier, CustomerDic[k].Rewards.Points);
                    Console.WriteLine();
                    if (CustomerDic[k].IsBirthday() && !(total <= 0)) //Check whether it is the customers birthday
                    {                                                //and total cost is not less than or equal zero
                        double amt = 0;
                        foreach (IceCream iceCream in list)
                        {
                            if (iceCream.CalculatePrice() > amt)
                            {
                                amt = iceCream.CalculatePrice();
                            }
                        }
                        total -= amt;
                        Console.WriteLine("Happy Birthday!!!\n The most expensive ice cream in your order is on US!");
                        Console.WriteLine("Total: ${0:0.00}", total);
                        Console.WriteLine();
                    }
                    if (CustomerDic[k].Rewards.PunchCard == 10 && !(total <= 0)) //Check whether it is the customers has 10 punchcard
                    {                                                            //and total cost is not less than or equal zero
                        CustomerDic[k].Rewards.PunchCard = 0;
                        double amt = list[0].CalculatePrice();
                        total -= amt;
                        Console.WriteLine("You completed your punchcard\n" +
                                          "The first ice cream in your order is FREE!");
                        Console.WriteLine("Total: ${0:0.00}", total);
                        Console.WriteLine();
                    }
                    if (!(total <= 0))
                    {
                        Console.WriteLine("1 point = $0.02");
                        while (true)
                        {
                            try
                            {
                                Console.Write("How much points({0}) would you like to redeem (Enter '0' if you do not wish to redeem any): ", CustomerDic[k].Rewards.Points);
                                int points = Convert.ToInt32(Console.ReadLine());
                                if (points == 0) //End point redeeming prompt if user enter 0
                                {
                                    Console.WriteLine("Total: ${0:0.00}", total);                                  
                                    break;
                                }
                                else if (points < 0) //Prompt user again if input is less than 0
                                {
                                    Console.WriteLine("Please enter a valid integer between 1-{0}. '0' to not redeem any points", CustomerDic[k].Rewards.Points);
                                    continue;
                                }
                                else if (points > CustomerDic[k].Rewards.Points) //Prompt user again if input is more than customer current points
                                {
                                    Console.WriteLine("You currently {0} points. Not enough to redeem {1} points", CustomerDic[k].Rewards.Points, points);
                                    continue;
                                }
                                else //Redeem points
                                {
                                    CustomerDic[k].Rewards.Points -= points;
                                    double amt = points * 0.02;
                                    Console.WriteLine("You have redeem ${0} using {1} points", amt, points);
                                    total -= amt;
                                    Console.WriteLine("Total: ${0:0.00}", total);
                                    break;
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                continue;
                            }
                        }
                    }
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    int numOfIceCream = CustomerDic[k].CurrentOrder.IceCreamList.Count(); //Counting the number of ice cream in order
                    CustomerDic[k].Rewards.PunchCard = numOfIceCream; //Adding count to punchcard
                    if (CustomerDic[k].Rewards.PunchCard > 10)
                    {
                        CustomerDic[k].Rewards.PunchCard = 10; //Limit punchcard to 10 if exceeded 10
                    }
                    //Calculate the points earned and adding to customer points
                    double earnedPoints = Math.Floor(total * 0.72);
                    CustomerDic[k].Rewards.Points += Convert.ToInt32(earnedPoints);
                    //Adding time of fulfilled order
                    CustomerDic[k].CurrentOrder.TimeFulfilled = DateTime.Now;
                    appendOrder(CustomerDic[k]); //Appending currentorder to order.csv
                    //Creating Final Total
                    CustomerDic[k].CurrentOrder.FinalTotal = total;
                    //Adding the fulfilled order to order history
                    CustomerDic[k].OrderHistory.Add(CustomerDic[k].CurrentOrder);
                    //Chaging Current Order to Null
                    CustomerDic[k].CurrentOrder = null;
                    break;
                }
            }
        }
        else if (regularOrderQueue.Count() > 0)
        {
            List<IceCream> list = (regularOrderQueue.Dequeue().IceCreamList);
            Console.WriteLine("Ice Cream order");
            foreach (int k in CustomerDic.Keys)
            {
                if (CustomerDic[k].CurrentOrder == null)
                {
                    continue;
                }
                else if (CustomerDic[k].CurrentOrder.IceCreamList == list)
                {
                    Console.WriteLine(CustomerDic[k].CurrentOrder);
                    double total = CustomerDic[k].CurrentOrder.CalculateTotal();
                    Console.WriteLine("Membership Tier: {0}\n" +
                                      "Points: {1}", CustomerDic[k].Rewards.Tier, CustomerDic[k].Rewards.Points);
                    Console.WriteLine();
                    if (CustomerDic[k].IsBirthday() && !(total <= 0))
                    {
                        double amt = 0;
                        foreach (IceCream iceCream in list)
                        {
                            if (iceCream.CalculatePrice() > amt)
                            {
                                amt = iceCream.CalculatePrice();
                            }
                        }
                        total -= amt;
                        Console.WriteLine("Happy Birthday!!!\n The most expensive ice cream in your order is on US!");
                        Console.WriteLine("Total: ${0}", total);
                        Console.WriteLine();
                    }
                    if (CustomerDic[k].Rewards.PunchCard == 10 && !(total <= 0))
                    {
                        CustomerDic[k].Rewards.PunchCard = 0;
                        double amt = list[0].CalculatePrice();
                        total -= amt;
                        Console.WriteLine("You completed your punchcard\n The first ice cream in your order is FREE!");
                        Console.WriteLine("Total: ${0}", total);
                        Console.WriteLine();
                    }
                    if (CustomerDic[k].Rewards.Tier == "Sliver" && !(total <= 0))
                    {
                        Console.WriteLine("1 point = $0.02");
                        while (true)
                        {
                            try
                            {
                                Console.Write("How much points({0}) would you like to redeem (Enter '0' if you do not wish to redeem any): ", CustomerDic[k].Rewards.Points);
                                int points = Convert.ToInt32(Console.ReadLine());
                                if (points == 0) //End point redeeming prompt if user enter 0
                                {
                                    Console.WriteLine("Total: ${0}", total);
                                    break;
                                }
                                else if (points < 0) //Prompt user again if input is less than 0
                                {
                                    Console.WriteLine("Please enter a valid integer between 1-{0}. '0' to not redeem any points", CustomerDic[k].Rewards.Points);
                                    continue;
                                }
                                else if (points > CustomerDic[k].Rewards.Points) //Prompt user again if input is more than customer current points
                                {
                                    Console.WriteLine("You currently {0} points. Not enough to redeem {1} points", CustomerDic[k].Rewards.Points, points);
                                    continue;
                                }
                                else //Redeem points
                                {
                                    CustomerDic[k].Rewards.Points -= points;
                                    double amt = points * 0.02;
                                    Console.WriteLine("You have redeem ${0} using {1} points", amt, points);
                                    total -= amt;
                                    Console.WriteLine("Total: ${0}", total);
                                    break;
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                continue;
                            }
                        }
                    }
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    int numOfIceCream = CustomerDic[k].CurrentOrder.IceCreamList.Count(); //Counting the number of ice cream in order
                    CustomerDic[k].Rewards.PunchCard = numOfIceCream; //Adding count to punchcard
                    if (CustomerDic[k].Rewards.PunchCard > 10)
                    {
                        CustomerDic[k].Rewards.PunchCard = 10; //Limit punchcard to 10 if exceeded
                    }
                    //Calculate the points earned and adding to customer points
                    double earnedPoints = Math.Floor(total * 0.72);
                    CustomerDic[k].Rewards.Points += Convert.ToInt32(earnedPoints);
                    //Checking if eligible to promote customer membership status
                    if (CustomerDic[k].Rewards.Points >= 50 && CustomerDic[k].Rewards.Tier == "Ordinary" && CustomerDic[k].Rewards.Tier != "Gold")
                    {
                        CustomerDic[k].Rewards.Tier = "Silver";
                    }
                    else if (CustomerDic[k].Rewards.Points >= 100 && CustomerDic[k].Rewards.Tier == "Silver")
                    {
                        CustomerDic[k].Rewards.Tier = "Gold";
                    }
                    //Adding time of fulfilled order
                    CustomerDic[k].CurrentOrder.TimeFulfilled = DateTime.Now;
                    appendOrder(CustomerDic[k]); //Appending currentorder to order.csv
                    //Creating Final Total
                    CustomerDic[k].CurrentOrder.FinalTotal = total;
                    //Adding the fulfilled order to order history
                    CustomerDic[k].OrderHistory.Add(CustomerDic[k].CurrentOrder);
                    //Chaging Current Order to Null
                    CustomerDic[k].CurrentOrder = null;
                    break;
                }
            }
        }
        else
        {
            Console.WriteLine("No order to process");
        }
    }
    //Advance Question 2 (Rishikesh)
    else if (opt == 8)
    {
        DisplayBreakDown(CustomerDic);
    }

}