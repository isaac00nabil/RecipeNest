using RecipeNest_Core.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RecipeNest_Core.Helper.Enums.RecipeNestLookups;

namespace RecipeNest_Core.Models.Entites
{
    public class Card : ParentEntity
    {
        public int CardId { get; set; }
        public CardType Type { get; set; }
        public float Price { get; set; }
        public float Point { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public virtual Donation? Donation { get; set; }

    }
}
