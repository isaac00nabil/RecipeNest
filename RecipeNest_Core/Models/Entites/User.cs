using RecipeNest_Core.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeNest_Core.Models.Entites
{
    public class User : ParentEntity
    {
        public int UserId { get; set; }
        public string ProfileImagePath { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        //public bool IsGuest { get; set; }
        public virtual Login? Login { get; set; }
        public virtual ICollection<Donation> Donations { get; set; }
        public virtual ICollection<FoodSection> FoodSections { get; set; }
        public virtual ICollection<Dish> Dishes { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }

    }
}
