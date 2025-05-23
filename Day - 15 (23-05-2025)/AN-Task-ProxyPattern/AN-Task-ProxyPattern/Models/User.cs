using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AN_Task_ProxyPattern.Models
{
    public class User
    {
        public string UserName { get; set; }
        public string Role { get; set; }

        public User(string UserName, string Role)
        {
            this.UserName = UserName;
            this.Role = Role.ToLower();
        }
    }
}
