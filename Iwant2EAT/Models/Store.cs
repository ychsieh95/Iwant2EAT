using System;
using System.Web;

namespace Iwant2EAT.Models
{
    public class Store
    {
        public string Name { get; set; }

        public string Branch { get; set; }

        public string Phone { get; set; }

        public bool Sunday { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public string DayOff { get; set; }

        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }

        public string Address { get; set; }

        public string Introduction { get; set; }

        public string ImageUrl { get; set; }

        public string Creater { get; set; }

        public string Guid { get; set; }
    }
}
