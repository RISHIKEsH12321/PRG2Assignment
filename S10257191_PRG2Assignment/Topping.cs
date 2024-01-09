using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10257191_PRG2Assignment
{
    internal class Topping
    {
        private string type;
        public string Type { get; set; }

        public Topping() { }

        public Topping(string type)
        {
            Type = type;
        }

        public override string ToString()
        {
            return $"{Type} Topping";
        }
    }
}
