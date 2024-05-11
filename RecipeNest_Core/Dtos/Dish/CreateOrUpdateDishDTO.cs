using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeNest_Core.Dtos.Dish
{
    public class CreateOrUpdateDishDTO
    {
        public int? DishId { get; set; }
        public string? DishImagePath { get; set; }
        public int FoodSectionId {  get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Steps { get; set; }
        public List<string> Ingredients { get; set; }
    }
}
