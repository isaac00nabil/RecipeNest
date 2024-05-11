using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeNest_Core.Dtos.Account;
using RecipeNest_Core.Dtos.Dish;
using RecipeNest_Core.Dtos.Donation;
using RecipeNest_Core.Dtos.Login;
using RecipeNest_Core.IRepositories;
using RecipeNest_Core.IServices;
using RecipeNest_Core.Models.Context;
using Serilog;
using System.Net;

namespace RecipeNest_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SharedController : ControllerBase
    {
        private readonly ISharedServiceInterface _sharedService;
        private readonly RecipeNestDbContext _context;
        public SharedController(ISharedServiceInterface sharedService, RecipeNestDbContext context)
        {
            _sharedService = sharedService;
            _context = context;
        }

        /// <summary>
        /// Creates a new Account for Member
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/CreateAccount
        ///     {        
        ///       "firstName": "Ahmad",
        ///       "lastName": "Mohammad",
        ///       "Email": "Ahmad.Mohammad@gmail.com",
        ///       "password": "@Pass123456"
        ///     }
        /// </remarks>
        /// <param name="dto">The DTO containing information about the account to create.</param>
        /// <returns>An IActionResult indicating the result of the Create Account.</returns>

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateAccount([FromBody] RegistrationDTO dto)
        {
            try
            {
                var createAccount = await _sharedService.CreateAccount(dto);
                return Ok(createAccount);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while creating a account");
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Creates an new dish By Admin or any Member .
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/CreateDish
        ///     {       
        ///       "ApiKey": "your_api_key"
        ///       
        ///       "dishImagePath": "/images/dish1.jpg",
        ///       "foodSectionId": 2,
        ///       "name": "Chicken Curry",
        ///       "description": "A delicious and spicy chicken curry.",
        ///       "steps": ["Heat oil in a pan.", "Add chopped onions and cook until golden brown.","Add chicken pieces and cook until they turn white."]    
        ///       "ingredients": ["500g chicken pieces", "2 onions, chopped", "3 tbsp curry paste", "1 cup water", "Salt and pepper to taste"]
        ///     }
        /// </remarks>
        /// <param name="ApiKey">The API key for authentication.</param> 
        /// <param name="dto">The DTO containing information about the dish to create.</param>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateDish([FromHeader] string ApiKey, [FromBody] CreateOrUpdateDishDTO dto)
        {
            try
            {
                var userKey = await _context.Logins.FirstOrDefaultAsync(l => l.ApiKey == ApiKey);
                if (userKey == null)
                {
                    return Unauthorized("Invalid API Key");
                }
                var dish = await _sharedService.CreateDish(dto);
                if (dish == "successfully")
                {
                    return Ok("Dish created successfully");
                }
                else
                {
                    return StatusCode(500, dish);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while creating a dish");
                return BadRequest(ex.Message);
            }

        }


        /// <summary>
        /// Deletes a dish by ID
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE api/DeleteDish
        ///     {
        ///        "ApiKey": "your_api_key"
        ///        "dishId": "Dish ID you want to delete"
        ///     }
        /// </remarks>
        /// <param name="ApiKey">The API key for authentication.</param>
        /// <param name="dishId">The ID of the dish to delete.</param>
        /// <returns>An IActionResult indicating the result of the deletion.</returns>
        [HttpDelete]
        [Route("[action]/{dishId}")]
        public async Task<IActionResult> DeleteDish([FromHeader] string ApiKey, [FromRoute] int dishId)
        {
            try
            {
                var userKey = await _context.Logins.FirstOrDefaultAsync(l => l.ApiKey == ApiKey);
                if (userKey == null)
                {
                    return Unauthorized("Invalid API Key");
                }
                var result = await _sharedService.DeleteDish(dishId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while deleting the dish");
                return StatusCode(500, "An error occurred while deleting the dish.");
            }
        }



        /// <summary>
        /// Deletes a Review by ID
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE api/DeleteReview
        ///     {
        ///        "ApiKey": "your_api_key"
        ///        "reviewId": "Review ID you want to delete"
        ///     }
        /// </remarks>
        /// <param name="ApiKey">The API key for authentication.</param>
        /// <param name="reviewId">The ID of the review to delete.</param>
        /// <returns>An IActionResult indicating the result of the deletion.</returns>
        [HttpDelete]
        [Route("[action]/{reviewId}")]
        public async Task<IActionResult> DeleteReview([FromHeader] string ApiKey, [FromRoute] int reviewId)
        {
            try
            {
                var userKey = await _context.Logins.FirstOrDefaultAsync(l => l.ApiKey == ApiKey);

                if (userKey == null)
                {
                    return Unauthorized("Invalid API Key");
                }

                var deleteReview = await _sharedService.DeleteReview(reviewId);
                if (deleteReview.Contains("successfully"))
                {
                    return Ok("Review deleted successfully");
                }
                else
                {
                    return NotFound("Review not found");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while delete review");
                return StatusCode(500, "An unexpected error occurred");
            }
        }


        /// <summary>
        /// Return List of all Dish Records
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/GetAllDishRecord
        ///     {
        ///        "ApiKey": "your_api_key"
        ///     }
        /// </remarks>
        /// <param name="ApiKey">The API key for authentication.</param>
        /// <returns>An IActionResult indicating the retrieve of all Dish Records.</returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllDishRecord([FromHeader] string ApiKey)
        {
            try
            {
                var userKey = await _context.Logins.FirstOrDefaultAsync(l => l.ApiKey == ApiKey);

                if (userKey == null)
                {
                    return Unauthorized("Invalid API Key");
                }

                var getAllDish = await _sharedService.GetAllDishRecord();
                    return Ok(getAllDish);

            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while Get All Dish Record");
                return StatusCode(500, "An error occurred while retrieving dish records.");
            }
        }

        /// <summary>
        /// Retrieve a list of all dish records.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/GetAllDonationRecord
        ///     {
        ///        "ApiKey": "your_api_key"
        ///     }
        /// </remarks>
        /// <param name="ApiKey">The API key for authentication.</param>
        /// <returns>An IActionResult indicating the retrieval of all Dish Records.</returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllDonationRecord([FromHeader] string ApiKey)
        {
            try
            {
                var userKey = await _context.Logins.FirstOrDefaultAsync(l => l.ApiKey == ApiKey);

                if (userKey == null)
                {
                    return Unauthorized("Invalid API Key");
                }

                var getAllDonation = await _sharedService.GetAllDonationRecord();
                return Ok(getAllDonation ?? new List<DonationRecordDTO>());
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while Get All Donation Record");
                return StatusCode(500, "An error occurred while retrieving donation records.");
            }
        }

        /// <summary>
        /// Retrieve a dish record by ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/GetDishRecordById
        ///     
        ///     {
        ///        "ApiKey": "your_api_key"
        ///        "dishId": "Dish ID you want to retrieve"
        ///     }
        /// </remarks>
        /// <param name="ApiKey">The API key for authentication.</param>
        /// <param name="dishId">The ID of the dish to retrieve.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpGet]
        [Route("[action]/{dishId}")]
        public async Task<IActionResult> GetDishRecordById([FromHeader] string ApiKey, [FromRoute] int dishId)
        {
            try
            {
                var userKey = await _context.Logins.FirstOrDefaultAsync(l => l.ApiKey == ApiKey);

                if (userKey == null)
                {
                    return Unauthorized("Invalid API Key");
                }

                var getDishById = await _sharedService.GetDishRecordById(dishId);
                if (getDishById != null)
                {
                    return Ok(getDishById);
                }
                else
                {
                    return NotFound($"Dish with ID {dishId} not found");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while Get Dish Record By Id");
                return StatusCode(500, "An error occurred while retrieving dish record.");
            }
        }


        /// <summary>
        /// Retrieve a dish record by name.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/GetDishRecordByName
        ///     
        ///     {
        ///        "ApiKey": "your_api_key"
        ///        "dishName": "Name of the dish you want to retrieve"
        ///     }
        /// </remarks>
        /// <param name="ApiKey">The API key for authentication.</param>
        /// <param name="dishName">The name of the dish to retrieve.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpGet]
        [Route("[action]/{dishName}")]
        public async Task<IActionResult> GetDishRecordByName([FromHeader] string ApiKey, [FromRoute] string dishName)
        {
            try
            {
                var userKey = await _context.Logins.FirstOrDefaultAsync(l => l.ApiKey == ApiKey);

                if (userKey == null)
                {
                    return Unauthorized("Invalid API Key");
                }

                var getDishByName = await _sharedService.GetDishRecordByName(dishName);
                if (getDishByName != null)
                {
                    return Ok(getDishByName);
                }
                else
                {
                    return NotFound($"Dish with name '{dishName}' not found");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while Get Dish Record By Name");
                return StatusCode(500, "An error occurred while retrieving dish record.");
            }
        }


        /// <summary>
        /// Retrieve a member record by ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/GetMemberById
        ///     
        ///     {
        ///        "ApiKey": "your_api_key"
        ///        "memberId": "ID of the member you want to retrieve"
        ///     }
        /// </remarks>
        /// <param name="ApiKey">The API key for authentication.</param>
        /// <param name="memberId">The ID of the member to retrieve.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpGet]
        [Route("[action]/{memberId}")]
        public async Task<IActionResult> GetMemberById([FromHeader] string ApiKey, [FromRoute] int memberId)
        {
            try
            {
                var userKey = await _context.Logins.FirstOrDefaultAsync(l => l.ApiKey == ApiKey);

                if (userKey == null)
                {
                    return Unauthorized("Invalid API Key");
                }

                var getMember = await _sharedService.GetMemberById(memberId);
                if (getMember != null)
                {
                    return Ok(getMember);
                }
                else
                {
                    return NotFound($"User with ID {memberId} not found");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while Get Member By Id");
                return StatusCode(500, "An error occurred while retrieving user record.");
            }
        }

        /// <summary>
        /// Retrieve a member record by username.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/GetMemberByUsername
        ///     
        ///     {
        ///        "ApiKey": "your_api_key"
        ///        "username": "Username of the member you want to retrieve"
        ///     }
        /// </remarks>
        /// <param name="ApiKey">The API key for authentication.</param>
        /// <param name="username">The username of the member to retrieve.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpGet]
        [Route("[action]/{username}")]
        public async Task<IActionResult> GetMemberByUsername([FromHeader] string ApiKey, [FromRoute] string username)
        {
            try
            {
                var userKey = await _context.Logins.FirstOrDefaultAsync(l => l.ApiKey == ApiKey);

                if (userKey == null)
                {
                    return Unauthorized("Invalid API Key");
                }

                var getMember = await _sharedService.GetMemberByUsername(username);
                if (getMember != null)
                {
                    return Ok(getMember);
                }
                else
                {
                    return NotFound($"User with username {username} not found");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while Get Member By Username");
                return StatusCode(500, "An error occurred while retrieving user record.");
            }
        }


        /// <summary>
        /// Authenticate a User by username and password.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/Login
        ///     {
        ///        "ApiKey": "your_api_key",
        ///        "username": "Username of the User",
        ///        "password": "Password of the User"
        ///     }
        /// </remarks>
        /// <param name="ApiKey">The API key for authentication.</param>
        /// <param name="dto">The DTO containing the username and password of the User to retrieve.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Login([FromHeader] string ApiKey, [FromBody] LoginRequestDTO dto)
        {
            try
            {
                var userKey = await _context.Logins.FirstOrDefaultAsync(l => l.ApiKey == ApiKey);

                if (userKey == null)
                {
                    return Unauthorized("Invalid API Key");
                }

                var login = await _sharedService.Login(dto);
                if (login.Contains("successful"))
                {
                    return Ok("Login successful");
                }
                else
                {
                    return BadRequest("Login failed");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while Login");
                return StatusCode(500, "An error occurred while processing the login request.");
            }
        }

        /// <summary>
        /// Logout a User by ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/Logout
        ///     {
        ///        "ApiKey": "your_api_key",
        ///        "id": "ID of the User to logout"
        ///     }
        /// </remarks>
        /// <param name="ApiKey">The API key for authentication.</param>
        /// <param name="id">The ID of the User to logout.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost]
        [Route("[action]/{id}")]
        public async Task<IActionResult> Logout([FromHeader] string ApiKey, [FromRoute] int id)
        {
            try
            {
                var userKey = await _context.Logins.FirstOrDefaultAsync(l => l.ApiKey == ApiKey);

                if (userKey == null)
                {
                    return Unauthorized("Invalid API Key");
                }

                var logout = await _sharedService.Logout(id);
                if (logout.Contains("successful"))
                {
                    return Ok("Logout successful");
                }
                else
                {
                    return BadRequest("Logout failed");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while Logout");
                return StatusCode(500, "An error occurred while processing the logout request.");
            }
        }

        /// <summary>
        /// Reset a user's password.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/ResetPassword
        ///     {
        ///        "ApiKey": "your_api_key",
        ///        "Email": "user@example.com",
        ///        "NewPassword": "new_password",
        ///        "ConfirmNewPassword": "repeat_the_new_password"
        ///     }
        /// </remarks>
        /// <param name="ApiKey">The API key for authentication.</param>
        /// <param name="dto">The DTO containing the user's email and new password.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> ResetPassword([FromHeader] string ApiKey, [FromBody] ResetPasswordDTO dto)
        {
            try
            {
                var userKey = await _context.Logins.FirstOrDefaultAsync(l => l.ApiKey == ApiKey);

                if (userKey == null)
                {
                    return Unauthorized("Invalid API Key");
                }

                var resetPassword = await _sharedService.ResetPassword(dto);
                if (resetPassword.Contains("successful"))
                {
                    return Ok("Reset password successful");
                }
                else
                {
                    return BadRequest("Reset password failed");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while Reset Password");
                return StatusCode(500, "An error occurred while resetting the password.");
            }
        }

        /// <summary>
        /// Update a user's profile.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/UpdateProfile
        ///     {
        ///        "ApiKey": "your_api_key",
        ///        "UserId": UserID to be updated,
        ///        "FirstName": "NewFirstName",
        ///        "LastName": "NewLastName",
        ///        "Email": "new_email@example.com",
        ///        "ProfileImagePath": "new_profile_image_path"
        ///     }
        /// </remarks>
        /// <param name="ApiKey">The API key for authentication.</param>
        /// <param name="dto">The DTO containing the user's Information to update.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> UpdateAccount([FromHeader] string ApiKey, [FromBody] UpdateProfileDTO dto)
        {
            try
            {
                var userKey = await _context.Logins.FirstOrDefaultAsync(l => l.ApiKey == ApiKey);

                if (userKey == null)
                {
                    return Unauthorized("Invalid API Key");
                }

                var updateAccount = await _sharedService.UpdateAccount(dto);
                if (updateAccount.Contains("successful"))
                {
                    return Ok("Update account successful");
                }
                else
                {
                    return BadRequest("Update account failed");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while Update Account");
                return StatusCode(500, "An error occurred while updating the account.");
            }
        }

        //[HttpPost]
        //[Route("action")]
        //public async Task<IActionResult> UpdateDish( [FromHeader] string ApiKey,[FromBody] CreateOrUpdateDishDTO dto)
        //{
        //    try
        //    {

        //    var userKey = await _context.Logins.FirstOrDefaultAsync(l => l.ApiKey == ApiKey);

        //            if (userKey == null)
        //            {
        //                return Unauthorized("Invalid API Key");
        //            }
        //        var result = await _sharedService.UpdateDish(dto);
        //        if (result.Contains("successful"))
        //        {
        //            return Ok(result);
        //        }
        //        else
        //        {
        //            return BadRequest(result);  
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, "An error occurred while Update Dish");
        //        return StatusCode(500, "An error occurred while updating the dish.");
        //    }
        //}
    }
}
