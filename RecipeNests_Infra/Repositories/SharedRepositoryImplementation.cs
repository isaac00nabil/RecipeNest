using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using RecipeNest_Core.Dtos.Account;
using RecipeNest_Core.Dtos.Dish;
using RecipeNest_Core.Dtos.Donation;
using RecipeNest_Core.Dtos.Login;
using RecipeNest_Core.Dtos.User;
using RecipeNest_Core.IRepositories;
using RecipeNest_Core.Models.Context;
using RecipeNest_Core.Models.Entites;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RecipeNests_Infra.Repositories
{
    public class SharedRepositoryImplementation : ISharedRepositoryInterface
    {
        private readonly RecipeNestDbContext _context;
        public SharedRepositoryImplementation(RecipeNestDbContext context)
        {
            _context = context;
        }


        public async Task<HttpStatusCode> CreateAccount(RegistrationDTO dto)
        {

            try
            {
                dto.Email = dto.Email.Trim().ToLower();
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
                if (existingUser != null)
                {
                    return HttpStatusCode.Found;
                }

                if (dto.Password.Length < 8 || dto.Password.Length > 20)
                {
                    throw new Exception("Password must be between 8 and 20 characters.");
                }

                User user = new User();

                user.FirstName = dto.FirstName.ToLower();
                user.LastName = dto.LastName.ToLower();
                user.Email = dto.Email.ToLower();
                await _context.AddAsync(user);
                await _context.SaveChangesAsync();


                Login login = new Login();

                login.Username = dto.Email;
                login.Password = dto.Password;
                login.User = user;
                await _context.AddAsync(login);
                await _context.SaveChangesAsync();

                return HttpStatusCode.OK;
            }
            catch
            {
                return HttpStatusCode.BadRequest;
            }

        }

        public async Task<HttpStatusCode> CreateDish(CreateOrUpdateDishDTO dto)
        {
            try
            {
                var section = await _context.FoodSections.FindAsync(dto.FoodSectionId);
                if (section == null)
                {
                    return HttpStatusCode.NotFound;
                }



                Dish dish = new Dish()
                {
                    DishImagePath = dto.DishImagePath,
                    Name = dto.Name,
                    Description = dto.Description,
                    Steps = dto.Steps,
                    Ingredients = dto.Ingredients,

                };

                await _context.AddAsync(dish);
                await _context.SaveChangesAsync();

                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while creating a new dish");
                return HttpStatusCode.BadRequest;
            }

        }

        public async Task<HttpStatusCode> DeleteDish(int dishId, bool isAdmin = false)
        {

            var dish = await _context.Dishes.AnyAsync(d => d.DishId == dishId);
            if (dish != null)
            {

                _context.Remove(dish);
                await _context.SaveChangesAsync();

                return HttpStatusCode.OK;
            }
            else
            {
                return HttpStatusCode.NotFound;
            }
        }

        public async Task<HttpStatusCode> DeleteReview(int reviewId, bool isAdmin = false)
        {
            if (await _context.Reviews.AnyAsync(r => r.ReviewId == reviewId))
            {
                var review = await _context.Reviews.FindAsync(reviewId);

                _context.Remove(review);
                await _context.SaveChangesAsync();

                return HttpStatusCode.OK;

            }
            else
            {
                return HttpStatusCode.NotFound;
            }
        }

        public async Task<List<DishRecordDTO>> GetAllDishRecord()
        {
            try
            {
                var dishRecords = await _context.Dishes
                         .Select(d => new DishRecordDTO
                         {

                             DishId = d.DishId,
                             DishImagePath = d.DishImagePath,
                             Name = d.Name,
                             Description = d.Description,
                             Steps = d.Steps,
                             CreationDateTime = d.CreationDateTime,
                             IsDeleted = d.IsDeleted

                         })
                          .ToListAsync();
                return dishRecords;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while get dish record");
                return null;
            }



        }

        public async Task<List<DonationRecordDTO>> GetAllDonationRecord()
        {
            var donationRecords = await (from d in _context.Donations
                                         join c in _context.Cards
                                         on d.Card.CardId equals c.CardId
                                         select new DonationRecordDTO
                                         {
                                             DonationId = d.DonationId,
                                             Description = d.Description,
                                             CardId = c.CardId,
                                             Price = c.Price,
                                             Point = c.Point,
                                             Type = c.Type,
                                             PaymentMethod = c.PaymentMethod,
                                             CreationDateTime = d.CreationDateTime,
                                             IsDeleted = d.IsDeleted,
                                         }).ToListAsync();

            return donationRecords;
        }




        public async Task<DishRecordDTO> GetDishRecordById(int dishId)
        {
            var donationRecord = await _context.Dishes.FirstOrDefaultAsync(d => d.DishId == dishId);
            if (donationRecord != null)
            {
                var dishRecord = await (from d in _context.Dishes
                                        join fs in _context.FoodSections
                                        on d.FoodSection.FoodSectionId equals fs.FoodSectionId
                                        where d.DishId == dishId
                                        select new DishRecordDTO
                                        {
                                            DishId = d.DishId,
                                            Name = d.Name,
                                            Description = d.Description,
                                            FoodSectionId = fs.FoodSectionId,
                                            Steps = d.Steps,
                                            Ingredients = d.Ingredients,
                                            CreationDateTime = d.CreationDateTime,
                                            IsDeleted = d.IsDeleted,
                                        }).FirstOrDefaultAsync();

                return dishRecord;
            }
            else
            {
                return null;
            }
        }

        public async Task<DishRecordDTO> GetDishRecordByName(string dishName)
        {
            var donationRecord = await _context.Dishes.FirstOrDefaultAsync(d => d.Name == dishName);
            if (donationRecord != null)
            {
                var dishRecord = await (from d in _context.Dishes
                                        join fs in _context.FoodSections
                                        on d.FoodSection.FoodSectionId equals fs.FoodSectionId
                                        where d.Name == dishName
                                        select new DishRecordDTO
                                        {
                                            DishId = d.DishId,
                                            Name = d.Name,
                                            Description = d.Description,
                                            FoodSectionId = fs.FoodSectionId,
                                            Steps = d.Steps,
                                            Ingredients = d.Ingredients,
                                            CreationDateTime = d.CreationDateTime,
                                            IsDeleted = d.IsDeleted,
                                        }).FirstOrDefaultAsync();

                return dishRecord;
            }
            else
            {
                return null;
            }
        }

        public async Task<UserRecordDTO> GetMemberById(int memberId)
        {
            var userRecord = await _context.Users.FirstOrDefaultAsync(u => u.UserId == memberId);
            if (userRecord != null)
            {
                var userDto = new UserRecordDTO
                {
                    UserId = userRecord.UserId,
                    ProfileImagePath = userRecord.ProfileImagePath,
                    Name = userRecord.FirstName + " " + userRecord.LastName,
                    Email = userRecord.Email,
                    IsAdmin = userRecord.IsAdmin,
                    CreationDateTime = userRecord.CreationDateTime,
                    IsDeleted = userRecord.IsDeleted,
                };
                return userDto;
            }
            else
            {
                return null;
            }
        }

        public async Task<UserRecordDTO> GetMemberByUsername(string username)
        {
            var userRecord = await _context.Users.FirstOrDefaultAsync(u => u.Email == username);
            if (userRecord != null)
            {
                var userDto = new UserRecordDTO
                {
                    UserId = userRecord.UserId,
                    ProfileImagePath = userRecord.ProfileImagePath,
                    Name = userRecord.FirstName + " " + userRecord.LastName,
                    Email = userRecord.Email,
                    IsAdmin = userRecord.IsAdmin,
                    CreationDateTime = userRecord.CreationDateTime,
                    IsDeleted = userRecord.IsDeleted,
                };
                return userDto;
            }
            else
            {
                return null;
            }
        }

        public async Task<HttpStatusCode> Login(LoginRequestDTO dto)
        {
            var client = await _context.Logins.FirstOrDefaultAsync(c => c.Username == dto.Username && c.Password == dto.Password);
            if (client != null)
            {
                return HttpStatusCode.OK;
            }
            else
            {
                return HttpStatusCode.NotFound;
            }
        }

        public async Task<HttpStatusCode> Logout(int id)
        {
            var client = await _context.Users.FindAsync(id);
            var admin = await _context.Users.FindAsync(id);

            if (client != null || admin != null)
            {
                await _context.SaveChangesAsync();
                return HttpStatusCode.OK;
            }
            else
            {
                return HttpStatusCode.NotFound;
            }
        }

        public async Task<HttpStatusCode> ResetPassword(ResetPasswordDTO dto)
        {
            if (dto.NewPassword != dto.ConfirmNewPassword)
            {
                return HttpStatusCode.BadRequest; // Bad request due to password mismatch
            }

            var client = await _context.Logins.FirstOrDefaultAsync(c => c.Username == dto.Email);
            if (client != null)
            {
                client.Password = dto.NewPassword;

                await _context.SaveChangesAsync();

                return HttpStatusCode.OK;
            }
            else
            {
                return HttpStatusCode.NotFound;
            }
        }

        public async Task<HttpStatusCode> UpdateAccount(UpdateProfileDTO dto)
        {
            try
            {
                dto.Email = dto.Email.Trim().ToLower();
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == dto.Email);
                if (existingUser != null && existingUser.UserId != dto.UserId)
                {
                    return HttpStatusCode.Found; // Email is already in use
                }

                if (dto.Password.Length < 8 || dto.Password.Length > 20)
                {
                    throw new Exception("Password must be between 8 and 20 characters.");
                }

                User user = await _context.Users.FindAsync(dto.UserId);
                if (user == null)
                {
                    return HttpStatusCode.NotFound;
                }


                user.FirstName = dto.FirstName.ToLower();
                user.LastName = dto.LastName.ToLower();
                user.Email = dto.Email.ToLower();
                await _context.AddAsync(user);
                await _context.SaveChangesAsync();

                Login login = await _context.Logins.FirstOrDefaultAsync(l => l.User.UserId == dto.UserId);
                if (login != null)
                {
                    login.Username = dto.Email;
                    login.Password = dto.Password;
                    login.User = user;
                    await _context.AddAsync(login);
                    await _context.SaveChangesAsync();
                }

                else
                {
                    return HttpStatusCode.NoContent;
                }

                await _context.SaveChangesAsync();
                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while Update Account");
                return HttpStatusCode.BadRequest;
            }
        }

        public async Task<HttpStatusCode> UpdateDish(CreateOrUpdateDishDTO dto, bool isAdmin = false)
        {
            try
            {
                var dish = await _context.Dishes.FindAsync(dto.DishId);
                if (dish == null)
                {
                    return HttpStatusCode.NotFound;
                }


                dish.Name = dto.Name;
                dish.Description = dto.Description;
                dish.Steps = dto.Steps;
                dish.Ingredients = dto.Ingredients;

                _context.Update(dish);
                await _context.SaveChangesAsync();

                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while UpdateDish");
                return HttpStatusCode.BadRequest;
            }
        }


    }
}
