using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeNest_Core.Helper.Enums
{
    public static class RecipeNestLookups 
    {
        public enum PaymentMethod
        {
            Visa = 100,
            PayPal,
            Bitcoin
        }
        public enum ErrorCode
        {
            Successful,
            NoError,
            UserNameErroe,
            EmailError,
            PasswordError,
            NotNullError,
            UserTypeError,
            LoginError
        }
        public enum CardType
        {
            Gold = 10,
            Silver,
            Bronze,
            Platinum,
            Diamond
        }


    }
}
