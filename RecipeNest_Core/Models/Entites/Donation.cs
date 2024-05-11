using RecipeNest_Core.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeNest_Core.Models.Entites
{
    public class Donation : ParentEntity
    {
        public int DonationId { get; set; }
        public string Description { get; set; }
        public virtual Card? Card { get; set; }
        public virtual User? User { get; set; }

    }
}
