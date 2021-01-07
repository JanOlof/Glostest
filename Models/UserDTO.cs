using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glostest.Models
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
    }
}