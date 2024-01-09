using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10257191_PRG2Assignment
{
    internal class Flavour
    {
        private string type;
        private bool premium;
        private int quantity;

        public string Type { get; set; }
        public bool Premium { get; set; }
        public int Quantity { get;set; }

        public Flavour() { }

        public Flavour(string type, bool premium, int qty)
        {
            Type = type;
            Premium = premium;
            Quantity = qty;
        }

        public override string ToString()
        {
            string extra =  "is not"; 
            if (Premium) { extra = "is"; }
            return $"{Quantity} {Type} Ice Cream {extra} Premium";
        }
    }
}
