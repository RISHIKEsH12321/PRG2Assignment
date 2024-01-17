// See https://aka.ms/new-console-template for more information
using S10257191_PRG2Assignment;

Console.WriteLine("Hello, World!");
//==========================================================
// Student Number : S10241860
// Student Name : Joseph Kwek Jun Yan
// Partner Name : Vallimuthu Rishikesh
//==========================================================

void DisplayMenu()
{
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

void ListCustomers(Dictionary<int, Customer> custDict)
{
    using (StreamReader sr = new StreamReader("customers.csv"))
    {
        string? s = sr.ReadLine();
        if (s != null)
        {
            string[] heading = s.Split(',');
            Console.WriteLine("{0,-10}  {1,-10}  {2,-10}",
                heading[0], heading[1], heading[2]);
        }
        while ((s = sr.ReadLine()) != null)
        {
            string[] customer = s.Split(',');
            Console.WriteLine("{0,-10}  {1,-10}  {2,-10}",
                customer[0], customer[1], customer[2]);
            custDict.Add(Convert.ToInt32(customer[1]), new Customer(customer[0],Convert.ToInt32(customer[1]), Convert.ToDateTime(customer[2])));
            
        }
    }
    Console.WriteLine();
}

IceCream CreateIceCream()
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
                while (true)
                {
                    Console.Write("Enter your Option Number: ");
                    if (Console.ReadLine() == "1")
                    {
                        IceCream newIceCream = new Cup();
                        break;
                    }
                    else if (Console.ReadLine() == "2")
                    {
                        IceCream newIceCream = new Cone();
                        break;
                    }
                    else if (Console.ReadLine() == "3")
                    {
                        IceCream newIceCream = new Waffle();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invliad Option.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
    IceCreamType();


    //Creating the Falvour
    int scoops = 0;
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

        while (true)
        {
            Dictionary<string, string> FlavourDic = new Dictionary<string, string>
            {
                { "1", "Vanilla" },
                { "2", "Chocolate" },
                { "3", "Strawberry" },
                { "4", "Durian" },
                { "5", "Ube" },
                { "6", "Sea Salt" }
            };

            Console.Write("Enter Your Flavour Number: ");
            string option = Console.ReadLine();
            if (FlavourDic.ContainsKey(option))
            {
                Console.WriteLine($"Enter the number of Scoops of {FlavourDic[option]}: ");
                try
                {
                    int FlavourScoopNum = Convert.ToInt16(Console.ReadLine());
                    while ((FlavourScoopNum + scoops) > 3)
                    {
                        Console.WriteLine("You cannot have more than 3 scoops per Ice Cream.");
                        Console.WriteLine($"Re-enter the number of Scoops for {FlavourDic[option]}: ");
                        FlavourScoopNum = Convert.ToInt16(Console.ReadLine());
                    }
                    scoops += FlavourScoopNum;
                    if (FlavourScoopNum != 0)
                    {
                        if (option == "4" || option == "5" || option == "6")
                        {
                            newIceCream.Flavours.Add(new Flavour(FlavourDic[option], true, FlavourScoopNum));
                        }
                        else
                        {
                            newIceCream.Flavours.Add(new Flavour(FlavourDic[option], false, FlavourScoopNum));
                        }
                    }
                    Console.Write("Enter 'Y' to add more flavours and 'N' To exit: ");
                    if (Console.ReadLine() == "N")
                    {
                        break;
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid Number.");
                }
            }
            else
            {
                Console.WriteLine("Enter a Vaild Option.");
            }
        }
    }


    //Creating the Toppings
    Console.WriteLine(
        "Toppings (+$1 each)\r\n" +
        "[1]Sprinkles\r\n" +
        "[2]Mochi\r\n" +
        "[3]Sago\r\n" +
        "[4]Oreos\r\n" +
        "[5]Exit/None");

    Dictionary<string, Topping> ToppingsDic = new Dictionary<string, Topping>
    {
        { "1", new Topping("Sprinkles") },
        { "2", new Topping("Mochi") },
        { "3", new Topping("Sago") },
        { "4", new Topping("Oreos") }
    };

    while (newIceCream.Toppings.Count < 4)
    {
        Console.Write("Enter the option number for the topping you want:");
        string ToppingOption = Console.ReadLine();
        if (ToppingOption == "1" || ToppingOption == "2" || ToppingOption == "3" || ToppingOption == "4" || ToppingOption == "5")
        {
            if (ToppingOption == "5")
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

    newIceCream.Scoop = scoops;
    /*
    if (newIceCream is Cone)
    {
        Console.WriteLine("Do you want your Cone to be Dipped in Chocolate (Y/N): ");
        string reply = Console.ReadLine();
        if (reply == "Y")
        {
            newIceCream.Dipped(true);
        }
        else
        {
            newIceCream.Dipped(false);
        }
    }
    else if (newIceCream is Waffle)
    {

    }*/


    //Reuturns IceCream
    return newIceCream;
}

Dictionary<int,Customer> custDict = new Dictionary<int,Customer>();

while (true)
{
    DisplayMenu();
    Console.Write("Enter your option: ");
    int opt = Convert.ToInt32(Console.ReadLine());
    if (opt == 0)
    {
        Console.WriteLine("Exiting...");
        break;
    }
    //Question 1 (Joseph)
    else if (opt == 1)
    {
        ListCustomers(custDict);
    }
    //Question 2

    //Question 3 (Joseph)
    else if (opt == 3)
    {
        //Retrieving information
        Console.Write("Enter your name: ");
        string name = Console.ReadLine();
        Console.Write("Enter your id number:");
        int id = Convert.ToInt32(Console.ReadLine());
        Console.Write("Enter your Date of Birth (dd/mm/yyyy) : ");
        DateTime dob = Convert.ToDateTime(Console.ReadLine());

        //Storing info in new customer object
        Customer newCustomer = new Customer(name,id,dob);

        //Creating a new pointcard object 
        PointCard newCard = new PointCard(0,0);

        //Appending new data to customers.csv file
        using (StreamWriter sw = new StreamWriter("customers.csv",true))
        {
            sw.WriteLine(name + "," + Convert.ToString(id) + "," + dob.ToString("dd/MM/yyyy") + "," + "Ordinary" + "," + newCard.Points + "," + newCard.PunchCard);
        }
        Console.WriteLine("New customer added!");
    }
    //Question 4 (Joseph)
    else if (opt == 4)
    {
        ListCustomers(custDict);
        Console.Write("Enter customer Id: ");
        int custId = Convert.ToInt32(Console.ReadLine());
        if (custDict.ContainsKey(custId))
        {

        }
        
    }

    //Question 5
    /*
    //Question 6
    else if (opt == 6)
{
    Console.Write("Enter your Customer ID: "); 
    string id = Console.ReadLine();
    Order CurrentOrder = customer1.CurrentOrder;

    
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
            Option6Option = Convert.ToInt16(Console.ReadLine());
            while (Option6Option != 1 || Option6Option != 2 || Option6Option != 3)
            {
                Console.WriteLine("Invlid Option");
                Console.Write("Re-enter your option: ");
                Option6Option = Convert.ToInt16(Console.ReadLine());
            }
            switch (Option6Option)
            {
                case 1:
                    foreach (Order order in CurrentOrder)
                    {
                        Console.WriteLine(order);
                    }
                    Console.Write("Enter which Ice Cream you want to change: ");
                    int ChangeIceCreamID = Convert.ToInt16(Console.ReadLine());
                    CurrentOrder.ModifyIceCream(ChangeIceCreamID);
                    break;
                case 2:
                    IceCream NewIceCream = CreateIceCream();
                    CurrentOrder.AddIceCream(NewIceCream);
                    break;

                case 3:
                    foreach (Order order in CurrentOrder)
                    {
                        Console.WriteLine(order);
                    }
                    Console.Write("Enter which Ice Cream you want to Delete: ");
                    int DeleteIceCreamID = Convert.ToInt16(Console.ReadLine());
                    CurrentOrder.DeleteIceCream(DeleteIceCreamID);
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }

}
*/
    //Advance Question 1
    //Advance Question 2

}





