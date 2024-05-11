using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RecipeNest_Core.Helper.Enums.RecipeNestLookups;

namespace RecipeNest_Core.Dtos.Donation
{
    public class DonationRecordDTO
    {
        public int DonationId { get; set; }
        public string Description { get; set; }
        public int CardId { get; set; }
        public float Price { get; set; }
        public float Point { get; set; }
        public CardType Type { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime CreationDateTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
