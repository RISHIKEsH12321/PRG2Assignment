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

void ListCustomers()
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
        }
    }
    Console.WriteLine();
}

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
        ListCustomers();
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
        //Customer newCustomer = new Customer(name,id,dob);

        //Creating a new pointcard object 
        PointCard newCard = new PointCard();

        //Appending new data to customers.csv file
        using (StreamWriter sw = new StreamWriter("customers.csv",true))
        {
            sw.WriteLine(name + "," + id + "," + dob);
        }
        Console.WriteLine("New customer added!");
    }
    //Question 4 (Joseph)
    ListCustomers();
    Console.Write("Enter customer Id: ");
    int custId = Convert.ToInt32(Console.ReadLine());

    //Question 5
    //Question 6

    //Advance Question 1
    //Advance Question 2

}





