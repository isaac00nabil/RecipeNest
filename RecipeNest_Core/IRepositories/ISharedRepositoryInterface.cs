using RecipeNest_Core.Dtos.Account;
using RecipeNest_Core.Dtos.Dish;
using RecipeNest_Core.Dtos.Donation;
using RecipeNest_Core.Dtos.Login;
using RecipeNest_Core.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RecipeNest_Core.IRepositories
{
    public interface ISharedRepositoryInterface
    {
        Task<HttpStatusCode> CreateAccount(RegistrationDTO dto);
        Task<HttpStatusCode> CreateDish(CreateOrUpdateDishDTO dto);
        Task<List<DonationRecordDTO>> GetAllDonationRecord();
        Task<UserRecordDTO> GetMemberByUsername(string username);
        Task<UserRecordDTO> GetMemberById(int memberId);
        Task<List<DishRecordDTO>> GetAllDishRecord();
        Task<DishRecordDTO> GetDishRecordById(int dishId);
        Task<DishRecordDTO> GetDishRecordByName(string dishName);
        Task<HttpStatusCode> UpdateDish(CreateOrUpdateDishDTO dto, bool isAdmin = false);
        Task<HttpStatusCode> UpdateAccount(UpdateProfileDTO dto);
        Task<HttpStatusCode> DeleteDish(int dishId, bool isAdmin = false);
        Task<HttpStatusCode> DeleteReview(int reviewId, bool isAdmin = false);
        Task<HttpStatusCode> ResetPassword(ResetPasswordDTO dto);
        Task<HttpStatusCode> Login(LoginRequestDTO dto);
        Task<HttpStatusCode> Logout(int id);

    }
}
