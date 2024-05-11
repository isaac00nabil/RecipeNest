using RecipeNest_Core.Dtos.Account;
using RecipeNest_Core.Dtos.Dish;
using RecipeNest_Core.Dtos.Donation;
using RecipeNest_Core.Dtos.Login;
using RecipeNest_Core.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeNest_Core.IServices
{
    public interface ISharedServiceInterface
    {
        Task<string> CreateAccount(RegistrationDTO dto);
        Task<string> CreateDish(CreateOrUpdateDishDTO dto);
        Task<List<DonationRecordDTO>> GetAllDonationRecord();
        Task<UserRecordDTO> GetMemberByUsername(string username);
        Task<UserRecordDTO> GetMemberById(int memberId);
        Task<List<DishRecordDTO>> GetAllDishRecord();
        Task<DishRecordDTO> GetDishRecordById(int dishId);
        Task<DishRecordDTO> GetDishRecordByName(string dishName);
        Task<string> UpdateDish(CreateOrUpdateDishDTO dto, bool isAdmin = false);
        Task<string> UpdateAccount(UpdateProfileDTO dto);
        Task<string> DeleteDish(int dishId, bool isAdmin = false);
        Task<string> DeleteReview(int reviewId, bool isAdmin = false);
        Task<string> ResetPassword(ResetPasswordDTO dto);
        Task<string> Login(LoginRequestDTO dto);
        Task<string> Logout(int id);
    }
}
