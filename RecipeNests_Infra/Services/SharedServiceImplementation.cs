using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using RecipeNest_Core.Dtos.Account;
using RecipeNest_Core.Dtos.Dish;
using RecipeNest_Core.Dtos.Donation;
using RecipeNest_Core.Dtos.Login;
using RecipeNest_Core.Dtos.User;
using RecipeNest_Core.IRepositories;
using RecipeNest_Core.IServices;
using RecipeNest_Core.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RecipeNests_Infra.Services
{
    public class SharedServiceImplementation : ISharedServiceInterface
    {

        private readonly ISharedRepositoryInterface _sharedRepositoryInterface;
        public SharedServiceImplementation(ISharedRepositoryInterface sharedRepositoryInterface)
        {
            _sharedRepositoryInterface = sharedRepositoryInterface;
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }

        public async Task<string> CreateAccount(RegistrationDTO dto)
        {

            string cleanedFirstName = dto.FirstName.Trim();
            string cleanedLastName = dto.LastName.Trim();

            var createAccount = await _sharedRepositoryInterface.CreateAccount(dto);

            if (createAccount == HttpStatusCode.OK)
            {
                throw new Exception("Created account successful.");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(dto.FirstName) || string.IsNullOrWhiteSpace(dto.LastName))
                {
                    throw new ArgumentException("First name and last name are required.");
                }

                if (!Regex.IsMatch(cleanedFirstName, @"^\p{L}{3,255}$"))
                {
                    throw new ArgumentException("First name must be between 3 and 255 characters and contain only letters.");
                }

                if (!Regex.IsMatch(cleanedLastName, @"^\p{L}{3,255}$"))
                {
                    throw new ArgumentException("Last name must be between 3 and 255 characters and contain only letters.");
                }
                if (!IsValidEmail(dto.Email))
                {
                    throw new Exception("Invalid email format");
                }

                if (createAccount == HttpStatusCode.Found)
                {
                    throw new Exception("Email is already exist.");
                }

                if (dto.Password.Length < 8 || dto.Password.Length > 20)
                {
                    throw new Exception("Password must be between 8 and 20 characters.");
                }

                else
                {
                    throw new Exception("Unexpected error.");
                }
            }

        }

        public async Task<string> CreateDish(CreateOrUpdateDishDTO dto)
        {
            string cleanedName = dto.Name.Trim();
            string cleanedDescription = dto.Description.Trim();

            var createDish = await _sharedRepositoryInterface.CreateDish(dto);

            if (createDish == HttpStatusCode.OK)
            {
                throw new Exception("Created dish successfully");
            }
            else
            {
                if (createDish == HttpStatusCode.NotFound)
                {
                    throw new Exception("Food section not found");
                }

                if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Description))
                {
                    throw new ArgumentException("Dish Name and Description are required.");
                }

                if (!Regex.IsMatch(cleanedName, @"^\p{L}{3,255}$"))
                {
                    throw new ArgumentException("Name must be between 3 and 255 characters and contain only letters.");
                }

                if (!Regex.IsMatch(cleanedDescription, @"^[\p{L}\s]{3,1000}$"))
                {
                    throw new ArgumentException("Description must be between 3 and 1000 characters and contain only letters, numbers, and spaces.");
                }

                if (dto.Steps.Count < 3)
                {
                    throw new ArgumentException("Dish must have at least 3 steps.");
                }

                if (dto.Ingredients.Count < 3)
                {
                    throw new ArgumentException("Dish must have at least 3 ingredients.");
                }

                else
                {
                    throw new Exception("Unexpected error.");
                }
            }
        }

        public async Task<string> DeleteDish(int dishId, bool isAdmin = false)
        {
            var deleteDish = await _sharedRepositoryInterface.DeleteDish(dishId);
            if (deleteDish == HttpStatusCode.OK)
            {
                return "Dish deleted successfully";
            }
            else if (deleteDish == HttpStatusCode.NotFound)
            {
                throw new Exception("Dish not found");
            }
            else
            {
                throw new Exception("An error occurred while deleting the dish");
            }
        }


        public async Task<string> DeleteReview(int reviewId, bool isAdmin = false)
        {
            var deleteReview = await _sharedRepositoryInterface.DeleteReview(reviewId);
            if (deleteReview == HttpStatusCode.OK)
            {
                throw new Exception("Review deleted successfully");
            }
            else
            {
                throw new Exception("Review not found");
            }
        }

        public async Task<List<DishRecordDTO>> GetAllDishRecord()
        {
            var getAllDish = await _sharedRepositoryInterface.GetAllDishRecord();
            if (getAllDish?.Count > 0)
            {
                return getAllDish;
            }
            else
            {
                throw new Exception ("No dish records found");
            }
        }

        public async Task<List<DonationRecordDTO>> GetAllDonationRecord()
        {
            var getAllDonation = await _sharedRepositoryInterface.GetAllDonationRecord();
            if (getAllDonation?.Count > 0)
            {
                return getAllDonation;
            }
            else
            {
                return null;
            }
        }

        public async Task<DishRecordDTO> GetDishRecordById(int dishId)
        {
            var getDishById = await _sharedRepositoryInterface.GetDishRecordById(dishId);
            if (getDishById != null)
            {
                return getDishById;
            }
            else
            {
                return new DishRecordDTO();
            }
        }

        public async Task<DishRecordDTO> GetDishRecordByName(string dishName)
        {
            var getDishByName = await _sharedRepositoryInterface.GetDishRecordByName(dishName);
            if (getDishByName != null)
            {
                return getDishByName;
            }
            else
            {
                return null;
            }
        }

        public async Task<UserRecordDTO> GetMemberById(int memberId)
        {
            var getMember = await _sharedRepositoryInterface.GetMemberById(memberId);
            if (getMember != null)
            {
                return getMember;
            }
            else
            {
                return null;

            }
        }

        public async Task<UserRecordDTO> GetMemberByUsername(string username)
        {
            var getMember = await _sharedRepositoryInterface.GetMemberByUsername(username);
            if (getMember != null)
            {
                return getMember;
            }
            else
            {
                return null;

            };
        }

        public async Task<string> Login(LoginRequestDTO dto)
        {
            var login = await _sharedRepositoryInterface.Login(dto);
            if (login == HttpStatusCode.OK)
            {
                return "Login successful";
            }
            else
            {
                return "Login failed";
            }
        }

        public async Task<string> Logout(int id)
        {
            var logout = await _sharedRepositoryInterface.Logout(id);
            if (logout == HttpStatusCode.OK)
            {
                return "Logout successful";

            }
            else
            {
                return "Logout failed";
            }
        }

        public async Task<string> ResetPassword(ResetPasswordDTO dto)
        {
            var resetPassword = await _sharedRepositoryInterface.ResetPassword(dto);
            if (resetPassword == HttpStatusCode.OK)
            {
                return "Reset password successful";

            }
            else
            {
                return "Reset password failed";
            }
        }

        public async Task<string> UpdateAccount(UpdateProfileDTO dto)
        {
            var updateAccount = await _sharedRepositoryInterface.UpdateAccount(dto);
            if (updateAccount == HttpStatusCode.OK)
            {
                return "Update account successful";

            }
            else
            {
                return "Update account failed";
            }
        }

        public async Task<string> UpdateDish(CreateOrUpdateDishDTO dto, bool isAdmin = false)
        {
            var updateDish = await _sharedRepositoryInterface.UpdateDish(dto);
            if (updateDish == HttpStatusCode.OK)
            {
                return "Update dish successful";

            }
            else
            {
                return "Update dish failed";
            }
        }
    }
}
