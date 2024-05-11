using RecipeNest_Core.Dtos.Donation;
using RecipeNest_Core.Dtos.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RecipeNest_Core.IRepositories
{
    public interface IMemberRepositoryInterface
    {
        Task<HttpStatusCode> CreateDonationDTO(CreateDonationDTO dto);
        Task<HttpStatusCode> CreateReview(CreateOrUpdateReviewDTO dto);
        Task<HttpStatusCode> UpdateReview(CreateOrUpdateReviewDTO dto);
        Task<HttpStatusCode> DeleteAccount(int userId);
    }
}
