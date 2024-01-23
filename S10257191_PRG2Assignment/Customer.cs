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
        public List<Order> orderHistory { get; set; }
        public PointCard Rewards { get; set; }

        //Constructor
        public Customer() { }
        public Customer(string name, int memberId, DateTime dob)
        {
            Name = name;
            MemberId = memberId;
            DOB = dob;
            //CurrentOrder = new Order();
            orderHistory = new List<Order>();
        }

        //Methods
        //public Order MakeOrder()
        //{

        //}
        public bool IsBirthday()
        {
            return true;
        }

        public override string ToString()
        {
            return base.ToString();
        }

    }
}
