using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeNest_Core.Dtos.Review
{
    public class ReviewRecordDTO
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public float Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreationDateTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
