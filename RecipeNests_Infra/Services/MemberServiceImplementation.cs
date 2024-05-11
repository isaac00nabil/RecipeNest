using Microsoft.EntityFrameworkCore;
using RecipeNest_Core.Dtos.Donation;
using RecipeNest_Core.Dtos.Review;
using RecipeNest_Core.IRepositories;
using RecipeNest_Core.IServices;
using RecipeNest_Core.Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RecipeNests_Infra.Services
{
    public class MemberServiceImplementation : IMemberServiceInterface
    {
        private readonly IMemberRepositoryInterface _memberRepository;
        public MemberServiceImplementation(IMemberRepositoryInterface memberRepository)
        {
            _memberRepository = memberRepository;
        }
        public async Task<string> CreateDonationDTO(CreateDonationDTO dto)
        {
            var createDonation = await _memberRepository.CreateDonationDTO(dto);
            if (createDonation == HttpStatusCode.OK)
            {
                throw new Exception("Make donation successful.");
            }
            else
            {
                throw new Exception("Donation operation failed.");
            }
        }

        public async Task<string> CreateReview(CreateOrUpdateReviewDTO dto)
        {
            if (dto.Comment.Length < 0 || dto.Comment.Length > 255)
            {
                throw new Exception("Comment length must be between 1 and 255 characters.");
            }

            if (dto.Rating < 0 || dto.Rating > 10)
            {
                throw new Exception("Rating must be between 0 and 10.");
            }

            var createReview = await _memberRepository.CreateReview(dto);
            if (createReview == HttpStatusCode.OK)
            {
                throw new Exception("Create a new comment is Successful.");
            }
            else
            {
                throw new Exception("Failed to create a new comment.");
            }
        }

        public async Task<string> DeleteAccount(int userId)
        {
            var deleteAccount = await _memberRepository.DeleteAccount(userId);
            if (deleteAccount == HttpStatusCode.OK)
            {
                throw new Exception($"Delete account with ID: {userId} Successful.");
            }

            else
            {
                throw new Exception($"User with ID: {userId} can't find it.");

            }
        }

        public async Task<string> UpdateReview(CreateOrUpdateReviewDTO dto)
        {
            var updateReview = await _memberRepository.UpdateReview(dto);
            if (updateReview == HttpStatusCode.NotFound)
            {
                throw new Exception($"Review with ID: {dto.ReviewId} can't find it.");
            }
            else if (updateReview == HttpStatusCode.OK)
            {
                throw new Exception($"Update review with ID: {dto.ReviewId} Successful.");

            }
            else
            {
                throw new Exception($"An error occurred while update review.");
            }
        }
    }
}
