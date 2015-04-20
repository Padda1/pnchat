using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PnChat.Models
{
    public class Usage
    {
        public int Year { get; set; }
        public int Sales { get; set; }
        public int Expenses { get; set; }

        public Usage()
        {
            Random random = new Random();
            Year = random.Next(0, 100);
            Sales = random.Next(0, 100);
            Expenses = random.Next(0, 100);
        }
    }
}