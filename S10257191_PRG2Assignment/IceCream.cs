using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10257191_PRG2Assignment
{
    internal abstract class IceCream
    {
        private string option;
        private int scoop;
        private List<Flavour> flavours;
        private List<Topping> toppings;

        public string Option { get; set; }
        public int Scoop { get; set; }
        public List<Flavour> Flavours { get; set; }
        = new List<Flavour>();
        public List<Topping> Toppings { get; set; }
        = new List<Topping>();

        public IceCream() { }

        public IceCream(string option, int scoop, List<Flavour> flavours, List<Topping> toppings)
        {
            Option = option;
            Scoop = scoop;
            Flavours = flavours;
            Toppings = toppings;
        }

        public abstract double CalculatePrice();

        public override string ToString()
        {
            string result = $"Option: {Option} \nNumber of Scoops: {Scoop}\nFlavours: ";
            string flavoursString = string.Join(", ", Flavours.Select(flavour => flavour.Type));
            result += flavoursString + "\n";

            // Check if there are toppings
            if (Toppings.Count > 0)
            {
                // Build string for Toppings
                string toppingsString = string.Join(", ", Toppings.Select(topping => topping.Type));

                // Append Toppings to the main result string
                result += $"Toppings: {toppingsString}\n";
            }
            return result;
        }
    }

    internal class Cup : IceCream
    {
        public Cup() { Option = "Cup"; }

        public Cup(string option, int scoops, List<Flavour> flavours, List<Topping> toppings)
            : base(option, scoops, flavours, toppings)
        {
            Option = "Cup";
        }

        public override double CalculatePrice()
        {
            double total = 0;

            switch (Scoop)
            {
                case 1:
                    total = 4.0;
                    break;
                case 2:
                    total = 5.5;
                    break;
                case 3:
                    total = 6.5;
                    break;
            }


            foreach (Flavour flavour in Flavours)
            {
                if (flavour.Premium)
                {
                    total += 2;
                }
            }

            total += Toppings.Count();

            return total;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    internal class Cone : IceCream
    {
        private bool dipped;

        public bool Dipped { get; set; }

        public Cone() { Option = "Cone";  }

        public Cone(string option, int scoops, List<Flavour> flavours, List<Topping> toppings, bool dip)
            : base(option, scoops, flavours, toppings)
        {
            Dipped = dip;
            Option = "Cone";
        }

        public override double CalculatePrice()
        {
            double total = 0;

            switch (Scoop)
            {
                case 1:
                    total = 4.0;
                    break;
                case 2:
                    total = 5.5;
                    break;
                case 3:
                    total = 6.5;
                    break;
            }


            foreach (Flavour flavour in Flavours)
            {
                if (flavour.Premium)
                {
                    total += 2;
                }
            }

            if (Dipped) { total += 2; }
            total += Toppings.Count();


            return total;
        }

        public override string ToString()
        {
            return $"{base.ToString()}Dipped: {dipped}\n";
        }
    }

    internal class Waffle : IceCream
    {
        private string waffleFlavour;

        public string WaffleFlavour { get; set; }

        public Waffle() { Option = "Waffle";  }

        public Waffle(string option, int scoops, List<Flavour> flavours, List<Topping> toppings, string waffle)
            : base(option, scoops, flavours, toppings)
        {
            WaffleFlavour = waffle;
            Option = "Waffle";
        }

        public override double CalculatePrice()
        {
            double total = 0;

            switch (Scoop)
            {
                case 1:
                    total = 4.0;
                    break;
                case 2:
                    total = 5.5;
                    break;
                case 3:
                    total = 6.5;
                    break;
            }

            if (WaffleFlavour == "Red velvet" || WaffleFlavour == "Charcoal" || WaffleFlavour == "Pandan")
            { total += 3; }
            else { waffleFlavour = "Basic"; }

            foreach (Flavour flavour in Flavours)
            {
                if (flavour.Premium)
                {
                    total += 2;
                }
            }

            total += Toppings.Count();

            return total;
        }

        public override string ToString()
        {
            return $"{base.ToString()}Waffle Falvour: {WaffleFlavour}";
        }

    }
}
