using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeNest_Core.Dtos.FoodSection
{
    public class FoodSectionRecordDTO
    {
        public int FoodSectionId { get; set; }
        public int AdminId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDateTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
