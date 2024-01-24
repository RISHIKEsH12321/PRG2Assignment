using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10257191_PRG2Assignment
{
    internal class Customer
    {
        //Properties
        public string Name { get; set; } 
        public int MemberId { get; set; }
        public DateTime DOB { get; set; }
        public Order CurrentOrder { get; set; }
        public List<Order> OrderHistory { get; set; } = new List<Order>();
        public PointCard Rewards { get; set; }

        //Constructor
        public Customer() { }
        public Customer(string name, int memberId, DateTime dob)
        {
            Name = name;
            MemberId = memberId;
            DOB = dob;
            OrderHistory = new List<Order>();
        }

        //Methods
        
        public Order MakeOrder()
        {
            Order newOrder = new Order();
            CurrentOrder = newOrder;
            return newOrder;
        }
        public bool IsBirthday()
        {
            if (DOB == DateTime.Now)
            {
                return true;
            }
            else { return false; }
        }

        public override string ToString()
        {
            return base.ToString();
        }

    }
}
