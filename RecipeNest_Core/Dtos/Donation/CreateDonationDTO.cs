using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RecipeNest_Core.Helper.Enums.RecipeNestLookups;

namespace RecipeNest_Core.Dtos.Donation
{
    public class CreateDonationDTO
    {
        public CardType Type { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public float Price { get; set; }
        public float Point {  get; set; }

    }
}
