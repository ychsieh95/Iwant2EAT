using System;
using System.Web;

namespace Iwant2EAT.Models
{
    public class Store
    {
        public string PicPath { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Introduction { get; set; }
        public DateTime OpeningTime { get; set; }
        public DateTime CloseTime { get; set; }
    }
}
