using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10257191_PRG2Assignment
{
    internal class PointCard
    {
        private int points;
        private int punchCard;
        private string tier;

        public int Points { get; set; }
        public int PunchCard { get; set; }
        public string Tier { get; set; }

        public PointCard() { }

        public PointCard(int points, int punchCard)
        {
            Points = points;
            PunchCard = punchCard;
            Tier = "Ordinary";
        }

        public void AddPoints(int amt)
        {
            Points += amt;
        }

        public void RedeemPoints(int amt)
        {
            Points -= amt;
        }

        public void Punch()
        {
            PunchCard += 1;
        }
    }
}
