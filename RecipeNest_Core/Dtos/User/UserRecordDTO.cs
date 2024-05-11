using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeNest_Core.Dtos.User
{
    public class UserRecordDTO
    {
        public int UserId { get; set; }
        public string ProfileImagePath { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreationDateTime { get; set; }
    }
}
