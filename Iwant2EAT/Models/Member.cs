using System;
using System.Web;

namespace Iwant2EAT.Models
{
    public class Member 
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public DateTime LastLogin { get; set; }
        public string LastIpAdr { get; set; }
    }
}
