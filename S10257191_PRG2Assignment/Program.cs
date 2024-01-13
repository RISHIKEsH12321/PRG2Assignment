// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
//==========================================================
// Student Number : S10241860
// Student Name : Joseph Kwek Jun Yan
// Partner Name : Vallimuthu Rishikesh
//==========================================================

void DisplayMenu()
{
    Console.WriteLine("Ice Cream Shop Management System");
    Console.WriteLine("======================================\n" +
                      "[1] List all customers\n" +
                      "[2] List all current orders\n" +
                      "[3] Register a new customer\n" +
                      "[4] Create a customer’s order\n" +
                      "[5] Display order details of a customer\n" +
                      "[6] Modify order details\n" +
                      "[7] Process an order and checkout\n" +
                      "[8] Display monthly charged amounts breakdown & total charged amounts for the year\n" +
                      "[0] Exit");
}
while (true)
{
    DisplayMenu();
    Console.Write("Enter your option: ");
    int opt = Convert.ToInt32(Console.ReadLine());
    if (opt == 0)
    {
        break;
    }
    //Question 1
    else if (opt == 1)
    {

    }
    //Question 2

    //Question 3

    //Question 4

    //Question 5
    //Question 6

    //Advance Question 1
    //Advance Question 2

}





