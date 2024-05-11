using Microsoft.EntityFrameworkCore;
using RecipeNest_Core.Dtos.Donation;
using RecipeNest_Core.Dtos.Review;
using RecipeNest_Core.IRepositories;
using RecipeNest_Core.Models.Context;
using RecipeNest_Core.Models.Entites;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RecipeNests_Infra.Repositories
{
    public class MemberRepositoryImplementation : IMemberRepositoryInterface
    {
        private readonly RecipeNestDbContext _context;
        public MemberRepositoryImplementation(RecipeNestDbContext context)
        {
            _context = context;
        }
        public async Task<HttpStatusCode> CreateDonationDTO(CreateDonationDTO dto)
        {
            try
            {

                // إنشاء كائن Card وتعيين القيم
                var card = new Card
                {
                    Type = dto.Type,
                    Price = dto.Price,
                    PaymentMethod = dto.PaymentMethod
                };

                // إنشاء كائن Donation وتعيين القيم
                var donation = new Donation
                {
                    Card = card,
                };

                // إضافة الكائنات الجديدة إلى قاعدة البيانات
                _context.Cards.Add(card);
                _context.Donations.Add(donation);
                await _context.SaveChangesAsync();

                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while creating a new Donation");
                return HttpStatusCode.InternalServerError;
            }
        }

        public async Task<HttpStatusCode> CreateReview(CreateOrUpdateReviewDTO dto)
        {
            try
            {


                Review review = new Review
                {
                    Rating = dto.Rating,
                    Comment = dto.Comment,
                };

                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();

                return HttpStatusCode.OK;

            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while creating a new Review");
                return HttpStatusCode.InternalServerError;
            }

        }

        public async Task<HttpStatusCode> DeleteAccount(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);

                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();

                    return HttpStatusCode.OK;
                }
                else
                {
                    return HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while delete account");
                return HttpStatusCode.BadRequest;
            }

        }

        public async Task<HttpStatusCode> UpdateReview(CreateOrUpdateReviewDTO dto)
        {
            try
            {
                var review = await _context.Reviews.FirstOrDefaultAsync(r => r.ReviewId == dto.ReviewId);
                if (review == null)
                {
                    return HttpStatusCode.NotFound;
                }
                review.Rating = dto.Rating;
                review.Comment = dto.Comment;

                _context.Update(review);
                await _context.SaveChangesAsync();
                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while update review");
                return HttpStatusCode.BadRequest;
            }
        }
    }
}
