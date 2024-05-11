using RecipeNest_Core.Dtos.Account;
using RecipeNest_Core.Dtos.Dish;
using RecipeNest_Core.Dtos.Donation;
using RecipeNest_Core.Dtos.FoodSection;
using RecipeNest_Core.Dtos.Review;
using RecipeNest_Core.Dtos.User;
using RecipeNest_Core.Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static RecipeNest_Core.Helper.Enums.RecipeNestLookups;

namespace RecipeNest_Core.IRepositories
{
    public interface IAdminRepositoryInterface
    {

        Task<HttpStatusCode> CreateNewFoodSection(CreateOrUpdateFoodSectionDTO dto);
        Task<DonationRecordDTO> GetDonationRecordById(int donationId);
        Task<List<DonationRecordDTO>> GetDonationRecordByType(CardType cardType);
        Task<List<UserRecordDTO>> GetAllMember();
        Task<ReviewRecordDTO> GetReviewRecordById(int reviewId);
        Task<List<ReviewRecordDTO>> GetAllReviewRecord();
        Task<HttpStatusCode> UpdateFoodSection(CreateOrUpdateFoodSectionDTO dto);
        Task<HttpStatusCode> DeleteFoodSection(int? foodSectionId );

    }
}
