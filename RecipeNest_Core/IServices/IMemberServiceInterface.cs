using RecipeNest_Core.Dtos.Donation;
using RecipeNest_Core.Dtos.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeNest_Core.IServices
{
    public interface IMemberServiceInterface
    {
        Task<string> CreateDonationDTO(CreateDonationDTO dto);
        Task<string> CreateReview(CreateOrUpdateReviewDTO dto);
        Task<string> UpdateReview(CreateOrUpdateReviewDTO dto);
        Task<string> DeleteAccount(int userId);
    }
}
