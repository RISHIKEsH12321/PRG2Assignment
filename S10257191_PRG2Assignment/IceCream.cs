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
        public List<Topping> Toppings { get; set; }


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
            return $"{Option} {Scoop} {Flavours} {Toppings}";
        }
    }

    internal class Cup: IceCream
    {
        public Cup() { }

        public Cup(string option, int scoops, List<Flavour> flavours, List<Topping> toppings) 
            : base(option, scoops, flavours, toppings)
        {
            Option = "1";
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


            foreach(Flavour flavour in Flavours)
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

        public Cone() { }

        public Cone(string option, int scoops, List<Flavour> flavours, List<Topping> toppings, bool dip)
            : base(option, scoops, flavours, toppings)
        {
            Dipped = dip;
            Option = "2";
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
            return base.ToString();
        }
    }

    internal class Waffle : IceCream
    {
        private string waffleFlavour;

        public string WaffleFlavour { get; set; }

        public Waffle() { }

        public Waffle(string option, int scoops, List<Flavour> flavours, List<Topping> toppings, string waffle)
            : base(option, scoops, flavours, toppings)
        {
            WaffleFlavour = waffle;
            Option = "3";
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
}
