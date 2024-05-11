using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeNest_Core.Models.Shared
{
    public class ParentEntity
    {
        public DateTime CreationDateTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
