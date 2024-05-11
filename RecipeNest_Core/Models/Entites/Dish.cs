using RecipeNest_Core.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeNest_Core.Models.Entites
{
    public class Dish : ParentEntity
    {
        public int DishId { get; set; }
        public string DishImagePath { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Steps { get; set; }
        public List<string> Ingredients { get; set; }
        public int? UserId { get; set; }
        public virtual User? User { get; set; }
        public virtual FoodSection? FoodSection { get; set; }


    }
}
