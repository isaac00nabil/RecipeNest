using RecipeNest_Core.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeNest_Core.Models.Entites
{
    public class Login : ParentEntity
    {
        public int LoginId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ApiKey { get; set; }
        public virtual User? User { get; set; }
    }
}
