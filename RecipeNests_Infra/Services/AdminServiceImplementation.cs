using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RecipeNest_Core.Dtos.Donation;
using RecipeNest_Core.Dtos.FoodSection;
using RecipeNest_Core.Dtos.Review;
using RecipeNest_Core.Dtos.User;
using RecipeNest_Core.Helper.Enums;
using RecipeNest_Core.IRepositories;
using RecipeNest_Core.IServices;
using RecipeNest_Core.Models.Context;
using RecipeNest_Core.Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace RecipeNests_Infra.Services
{
    public class AdminServiceImplementation : IAdminServiceInterface
    {
        private readonly IAdminRepositoryInterface _adminRepositoryInterface;
        public AdminServiceImplementation(IAdminRepositoryInterface adminRepositoryInterface)
        {
            _adminRepositoryInterface = adminRepositoryInterface;
        }
        public async Task<string> CreateNewFoodSection(CreateOrUpdateFoodSectionDTO dto)
        {
            var createFoodSection = await _adminRepositoryInterface.CreateNewFoodSection(dto);

            string cleanedDescription = dto.Description.Trim();
            string cleanedName = dto.Name.Trim();

            if (createFoodSection == HttpStatusCode.OK)
            {
                throw new Exception("Created new section successful.");
            }
            else
            {

                if (string.IsNullOrWhiteSpace(dto.Description) || string.IsNullOrWhiteSpace(dto.Name))
                {
                    throw new ArgumentException("Description and Name are required.");
                }

                if (!Regex.IsMatch(cleanedDescription, @"^\p{L}{3,255}$"))
                {
                    throw new ArgumentException("Descriptionmust be between 1 and 1000 characters and contain only letters.");
                }

                if (!Regex.IsMatch(cleanedName, @"^\p{L}{3,255}$"))
                {
                    throw new ArgumentException("Name of Section must be between 1 and 255 characters and contain only letters.");
                }
                if (createFoodSection == HttpStatusCode.Found)
                {
                    throw new Exception("Section Name is already exist.");
                }
                else
                {
                    throw new Exception("Unexpected error.");
                }
            }

        }

        public async Task<string> DeleteFoodSection(int? foodSectionId)
        {
            var DdeteSection = await _adminRepositoryInterface.DeleteFoodSection(foodSectionId);
            if (DdeteSection == HttpStatusCode.OK)
            {
                throw new Exception("Section has been deleted.");
            }
            else
            {
                throw new Exception("Section not found.");
            }

        }

        public async Task<List<UserRecordDTO>> GetAllMember()
        {
            var userRecord = await _adminRepositoryInterface.GetAllMember();
            if (userRecord?.Count > 0)
            {
                return userRecord;
            }
            else
            {
                throw new Exception("No any user found with");
            }
        }

        public async Task<List<ReviewRecordDTO>> GetAllReviewRecord()
        {
            var reviewRecord = await _adminRepositoryInterface.GetAllReviewRecord();
            if (reviewRecord?.Count > 0)
            {
                return reviewRecord;
            }
            else
            {
                throw new Exception("No reviews were found. Please try again later.");

            }
        }

        public async Task<DonationRecordDTO> GetDonationRecordById(int donationId)
        {
            var donationRecord = await _adminRepositoryInterface.GetDonationRecordById(donationId);
            if (donationRecord != null)
            {
                return donationRecord;
            }
            else
            {
                throw new Exception($"No donation was found with ID: {donationId}. Please check the ID and try again.");
            }
        }
        public async Task<List<DonationRecordDTO>> GetDonationRecordByType(RecipeNestLookups.CardType cardType)
        {
            var donationRecord = await _adminRepositoryInterface.GetDonationRecordByType(cardType);
            if (donationRecord != null)
            {
                return donationRecord;
            }
            else
            {
                throw new Exception("No donation were found. Please try again later.");
            }
        }

        public async Task<ReviewRecordDTO> GetReviewRecordById(int reviewId)
        {
            var reviewRecord = await _adminRepositoryInterface.GetReviewRecordById(reviewId);
            if (reviewRecord != null)
            {
                return reviewRecord;
            }
            else
            {
                return null;
            }
        }

        public async Task<string> UpdateFoodSection(CreateOrUpdateFoodSectionDTO dto)
        {
            var foodSection = await _adminRepositoryInterface.UpdateFoodSection(dto);

            switch (foodSection)
            {
                case HttpStatusCode.NotFound:
                    return "Food Section not found.";
                case HttpStatusCode.OK:
                    return "Food Section updated successfully.";
                case HttpStatusCode.Found:
                    return "Food Section is already exist.";
                default:
                    return $"Failed to update Food Section updated: {foodSection}";

            }
        }

    }
}
