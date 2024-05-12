using Microsoft.EntityFrameworkCore;
using RecipeNest_Core.Dtos.Donation;
using RecipeNest_Core.Dtos.FoodSection;
using RecipeNest_Core.Dtos.Review;
using RecipeNest_Core.Dtos.User;
using RecipeNest_Core.IRepositories;
using RecipeNest_Core.Models.Context;
using RecipeNest_Core.Models.Entites;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static RecipeNest_Core.Helper.Enums.RecipeNestLookups;


namespace RecipeNests_Infra.Repositories
{
    public class AdminRepositoryImplementation : IAdminRepositoryInterface
    {
        private readonly RecipeNestDbContext _context;
        public AdminRepositoryImplementation(RecipeNestDbContext context)
        {
            _context = context;
        }
        public async Task<HttpStatusCode> CreateNewFoodSection(CreateOrUpdateFoodSectionDTO dto)
        {
            try
            {
                dto.Name = dto.Name.Trim().ToLower();
                var existingSection = await _context.FoodSections.FirstOrDefaultAsync(u => u.Name == dto.Name);
                if (existingSection != null)
                {
                    return HttpStatusCode.Found;
                }

                FoodSection foodSection = new FoodSection
                {
                    Name = dto.Name,
                    Description = dto.Description,
                };

                await _context.AddAsync(foodSection);
                await _context.SaveChangesAsync();

                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while creating a new food section");
                return HttpStatusCode.BadRequest;
            }
        }

        public async Task<HttpStatusCode> DeleteFoodSection(int? foodSectionId)
        {
            var foodSection = await _context.FoodSections.FindAsync(foodSectionId);
            if (foodSection != null)
            {
                _context.FoodSections.Remove(foodSection);
                await _context.SaveChangesAsync();

                return HttpStatusCode.OK;
            }
            else
            {
                return HttpStatusCode.NotFound;
            }
        }

        public async Task<List<UserRecordDTO>> GetAllMember()
        {

            var userRecord = await _context.Users.ToListAsync();
            if (userRecord?.Count > 0)
            {
                var result = userRecord.Select(u => new UserRecordDTO
                {
                    UserId = u.UserId,
                    ProfileImagePath = u.ProfileImagePath,
                    Name = u.FirstName,
                    Email = u.Email,
                    IsAdmin = u.IsAdmin,
                    IsDeleted = u.IsDeleted,
                    CreationDateTime = u.CreationDateTime
                }).ToList();
                return result;
            }
            else
            {
                return new List<UserRecordDTO>();
            }

        }

        public async Task<List<ReviewRecordDTO>> GetAllReviewRecord()
        {
            var reviewRecord = await _context.Reviews.ToListAsync();
            if (reviewRecord?.Count > 0)
            {
                var result = reviewRecord.Select(r => new ReviewRecordDTO
                {
                    ReviewId = r.ReviewId,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreationDateTime = r.CreationDateTime,
                    IsDeleted = r.IsDeleted,
                }).ToList();
                return result;
            }
            else
            {
                return null;
            }

        }

        public async Task<DonationRecordDTO> GetDonationRecordById(int donationId)
        {
            var donationRecord = await _context.Donations.FirstOrDefaultAsync(d => d.DonationId == donationId);
            if (donationRecord != null)
            {
                var result = await (from d in _context.Donations
                                    join c in _context.Cards
                                    on d.DonationId equals c.CardId
                                    where d.DonationId == donationId
                                    select new DonationRecordDTO
                                    {
                                        DonationId = d.DonationId,
                                        CardId = c.CardId,
                                        Price = c.Price,
                                        Point = c.Point,
                                        Type = c.Type,
                                        PaymentMethod = c.PaymentMethod,
                                        CreationDateTime = d.CreationDateTime,
                                        IsDeleted = d.IsDeleted,
                                    }).FirstOrDefaultAsync();
                return result;
            }
            else
            {
                return null;
            }
        }



        public async Task<List<DonationRecordDTO>> GetDonationRecordByType(CardType cardType)
        {
            var donationRecord = await _context.Cards.FirstOrDefaultAsync(d => d.Type == cardType);
            if (donationRecord != null)
            {
                var result = await (from d in _context.Donations
                                             join c in _context.Cards
                                             on d.Card.CardId equals c.CardId
                                             where c.Type == cardType
                                             select new DonationRecordDTO
                                             {
                                                 DonationId = d.DonationId,
                                                 CardId = c.CardId,
                                                 Price = c.Price,
                                                 Point = c.Point,
                                                 Type = c.Type,
                                                 PaymentMethod = c.PaymentMethod,
                                                 CreationDateTime = d.CreationDateTime,
                                             }).ToListAsync();

                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<ReviewRecordDTO> GetReviewRecordById(int reviewId)
        {
            var reviewRecord = await _context.Reviews.FirstOrDefaultAsync(r=>r.ReviewId == reviewId);
            if (reviewRecord != null)
            {
                var result = await (from d in _context.Reviews
                                    join c in _context.Users
                                    on d.ReviewId equals c.UserId
                                    where d.ReviewId == reviewId
                                    select new ReviewRecordDTO
                                    {
                                        ReviewId = d.ReviewId,
                                        UserId = c.UserId,
                                        Rating = d.Rating,
                                        Comment = d.Comment,
                                        CreationDateTime = d.CreationDateTime,
                                        IsDeleted = d.IsDeleted,
                                    }).FirstOrDefaultAsync();
                return result;
            }
            else
            {
                return null;
            }
        }


        public async Task<HttpStatusCode> UpdateFoodSection(CreateOrUpdateFoodSectionDTO dto)
        {
            try
            {
                var foodSection = await _context.FoodSections.FirstOrDefaultAsync(f => f.FoodSectionId == dto.FoodSectionId);
                if (foodSection == null)
                {
                    return HttpStatusCode.NotFound; // Food section not found
                }

                // Check if the new name already exists for another section
                var existingFoodSectionWithName = await _context.FoodSections.AnyAsync(f => f.Name == dto.Name && f.FoodSectionId != dto.FoodSectionId);
                if (existingFoodSectionWithName)
                {
                    return HttpStatusCode.Found; // Name already exists
                }

                // Update the food section
                foodSection.Name = dto.Name;
                foodSection.Description = dto.Description;

                _context.Update(foodSection);
                await _context.SaveChangesAsync();
                return HttpStatusCode.OK; // Updated successfully
            }
            catch (DbUpdateException)
            {
                return HttpStatusCode.ExpectationFailed; // Database update exception
            }
            catch (Exception)
            {
                return HttpStatusCode.InternalServerError; // Other unexpected exceptions
            }
        }

    }
}

