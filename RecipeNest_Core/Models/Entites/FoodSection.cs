using RecipeNest_Core.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeNest_Core.Models.Entites
{
    public class FoodSection : ParentEntity
    {
        public int FoodSectionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Dish> Dishes { get; set; }
        public virtual User? User { get; set; }
    }
}
