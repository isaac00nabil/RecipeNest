using RecipeNest_Core.Dtos.Donation;
using RecipeNest_Core.Dtos.FoodSection;
using RecipeNest_Core.Dtos.Review;
using RecipeNest_Core.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RecipeNest_Core.Helper.Enums.RecipeNestLookups;

namespace RecipeNest_Core.IServices
{
    public interface IAdminServiceInterface
    {
        Task<string> CreateNewFoodSection(CreateOrUpdateFoodSectionDTO dto);
        Task<DonationRecordDTO> GetDonationRecordById(int donationId);
        Task<List<DonationRecordDTO>> GetDonationRecordByType(CardType cardType);
        Task<List<UserRecordDTO>> GetAllMember();
        Task<ReviewRecordDTO> GetReviewRecordById(int reviewId);
        Task<List<ReviewRecordDTO>> GetAllReviewRecord();
        Task<string> UpdateFoodSection(CreateOrUpdateFoodSectionDTO dto);
        Task<string> DeleteFoodSection(int? foodSectionId);
    }
}
