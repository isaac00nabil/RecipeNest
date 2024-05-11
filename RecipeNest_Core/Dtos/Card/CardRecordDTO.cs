using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RecipeNest_Core.Helper.Enums.RecipeNestLookups;

namespace RecipeNest_Core.Dtos.Card
{
    public class CardRecordDTO
    {
        public int CardId { get; set; }
        public CardType Type { get; set; }
        public decimal Price { get; set; }
        public decimal Point { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime CreationDateTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
