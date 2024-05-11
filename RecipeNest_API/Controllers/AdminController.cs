using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RecipeNest_Core.Dtos.FoodSection;
using RecipeNest_Core.IServices;
using RecipeNest_Core.Models.Context;
using Serilog;
using static RecipeNest_Core.Helper.Enums.RecipeNestLookups;

namespace RecipeNest_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        private readonly IAdminServiceInterface _adminService;
        private readonly RecipeNestDbContext _context;
        public AdminController(IAdminServiceInterface adminService, RecipeNestDbContext context)
        {
            _adminService = adminService;
            _context = context;
        }


        /// <summary>
        /// Create a new food section.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/CreateNewFoodSection
        ///     {
        ///        "ApiKey": "your_api_key",
        ///        "Name": "Section Name",
        ///        "Description": "Description of the new section"
        ///     }
        /// </remarks>
        /// <param name="ApiKey">The API key for authentication.</param>
        /// <param name="dto">The DTO containing the new food section's name and description.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateNewFoodSection([FromHeader] string ApiKey, [FromBody] CreateOrUpdateFoodSectionDTO dto)
        {
            try
            {
                var userKey = await _context.Logins.FirstOrDefaultAsync(l => l.ApiKey == ApiKey);

                if (userKey == null)
                {
                    return Unauthorized("Invalid API Key");
                }

                var createAccount = await _adminService.CreateNewFoodSection(dto);
                return Ok(createAccount);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while creating a new food section");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieve all members.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/GetAllMember
        ///     {
        ///        "ApiKey": "your_api_key"
        ///     }
        /// </remarks>
        /// <param name="ApiKey">The API key for authentication.</param>
        /// <returns>An IActionResult containing a list of all members.</returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllMember([FromHeader] string ApiKey)
        {
            try
            {
                var userKey = await _context.Logins.FirstOrDefaultAsync(l => l.ApiKey == ApiKey);

                if (userKey == null)
                {
                    return Unauthorized("Invalid API Key");
                }
                var result = await _adminService.GetAllMember();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieve all reviews.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/GetAllReview
        ///     {
        ///        "ApiKey": "your_api_key"
        ///     }
        /// </remarks>
        /// <param name="ApiKey">The API key for authentication.</param>
        /// <returns>An IActionResult containing a list of all reviews.</returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllReview([FromHeader] string ApiKey)
        {
            try
            {
                var userKey = await _context.Logins.FirstOrDefaultAsync(l => l.ApiKey == ApiKey);

                if (userKey == null)
                {
                    return Unauthorized("Invalid API Key");
                }
                var result = await _adminService.GetAllReviewRecord();
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get a donation by its ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/GetDonationById/{donationId}
        ///     {
        ///        "ApiKey": "your_api_key",
        ///        "donationId" : "donation_id_here" 
        ///     }
        /// </remarks>
        /// <param name="ApiKey">The API key for authentication.</param>
        /// <param name="donationId">The ID of the donation to retrieve.</param>
        /// <returns>An IActionResult containing the donation details.</returns>
        [HttpGet]
        [Route("[action]/{donationId}")]
        public async Task<IActionResult> GetDonationById([FromHeader] string ApiKey, [FromRoute] int donationId)
        {
            try
            {
                var userKey = await _context.Logins.FirstOrDefaultAsync(l => l.ApiKey == ApiKey);
                if (userKey == null)
                {
                    return Unauthorized("Invalid API Key");
                }
                else
                {
                    var donationRecord = await _adminService.GetDonationRecordById(donationId);
                    if (donationRecord != null)
                    {
                        return Ok(donationRecord);
                    }
                    else
                    {
                        return StatusCode(404, donationRecord);

                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while get donation by ID");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete a food section by its ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE api/DeleteFoodSection/{foodSectionId}
        ///     {
        ///        "ApiKey": "your_api_key",
        ///        "foodSectionId" : "id of the food section to delete"
        ///     }
        /// </remarks>
        /// <param name="ApiKey">The API key for authentication.</param>
        /// <param name="foodSectionId">The ID of the food section to delete.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpDelete]
        [Route("[action]/{foodSectionId}")]
        public async Task<IActionResult> DeleteFoodSection([FromHeader] string ApiKey, [FromRoute] int foodSectionId)
        {
            try
            {
                var userKey = await _context.Logins.FirstOrDefaultAsync(l => l.ApiKey == ApiKey);
                if (userKey == null)
                {
                    return Unauthorized("Invalid API Key");
                }
                else
                {
                    var foodSection = await _adminService.DeleteFoodSection(foodSectionId);

                    if (foodSection.Contains("not found"))
                    {
                        return NotFound(foodSection);
                    }
                    else if (foodSection.Contains("successfully"))
                    {
                        return Ok(foodSection);
                    }
                    else
                    {
                        return BadRequest(foodSection);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while deleted section");
                return BadRequest(ex.Message);
            }
        }



        /// <summary>
        /// Get donations by card type.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/GetDonationByType/{cardType}
        ///     {
        ///        "ApiKey": "your_api_key",
        ///        "cardType" : "Card you want to retrieve"  
        ///        
        ///     }
        /// </remarks>
        /// <param name="ApiKey">The API key for authentication.</param>
        /// <param name="cardType">The type of card for donations: {"10": "Gold", "11": "Silver", "12": "Bronze", "13": "Platinum", "14": "Diamond"}</param>
        /// <returns>An IActionResult containing the donations.</returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetDonationByType([FromHeader] string ApiKey, [FromRoute] CardType cardType)
        {
            try
            {
                var userKey = await _context.Logins.FirstOrDefaultAsync(l => l.ApiKey == ApiKey);
                if (userKey == null)
                {
                    return Unauthorized("Invalid API Key");
                }
                else
                {
                    var donationHistory = await _adminService.GetDonationRecordByType(cardType);
                    if (donationHistory?.Count > 0)
                    {
                        return Ok(donationHistory);
                    }
                    else
                    {
                        return StatusCode(404, donationHistory);
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while get donation by type");
                return BadRequest(ex.Message);
            }


        }


        /// <summary>
        /// Get a review record by its ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/GetReviewRecordById/{reviewId}
        ///     {
        ///        "ApiKey": "your_api_key",
        ///        "reviewId" : "Review ID you want to retrieve"  
        ///     }
        /// </remarks>
        /// <param name="ApiKey">The API key for authentication.</param>
        /// <param name="reviewId">The ID of the review record to retrieve.</param>
        /// <returns>An IActionResult containing the review record.</returns>
        [HttpGet]
        [Route("[action]/{reviewId}")]
        public async Task<IActionResult> GetReviewRecordById([FromHeader] string ApiKey, [FromRoute] int reviewId)
        {
            try
            {
                var userKey = await _context.Logins.FirstOrDefaultAsync(l => l.ApiKey == ApiKey);
                if (userKey == null)
                {
                    return Unauthorized("Invalid API Key");
                }
                var review = await _adminService.GetReviewRecordById(reviewId);
                if (review != null )
                {
                    return Ok(review);
                }
                else
                {
                    return StatusCode(404, "No Review were found. Please try again later.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while get Review Record");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update a food section by its ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/UpdateFoodSection
        ///     {
        ///        "ApiKey": "your_api_key",
        ///        "foodSectionId" : "ID of the food section to update",
        ///        "Name": "New name for the food section",
        ///        "Description": "New description for the food section"
        ///     }
        /// </remarks>
        /// <param name="ApiKey">The API key for authentication.</param>
        /// <param name="dto">The DTO containing the updated information for the food section.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> UpdateFoodSection([FromHeader] string ApiKey, [FromBody] CreateOrUpdateFoodSectionDTO dto)
        {
            try
            {
                var userKey = await _context.Logins.FirstOrDefaultAsync(l => l.ApiKey == ApiKey);
                if (userKey == null)
                {
                    return Unauthorized("Invalid API Key");
                }
                else
                {
                    var foodSection = await _adminService.UpdateFoodSection(dto);
                    if (foodSection.Contains("not found"))
                    {
                        return NotFound(foodSection);
                    }
                    else if (foodSection.Contains("successfully"))
                    {
                        return Ok(foodSection);
                    }
                    else if (foodSection.Contains("already"))
                    {
                        return BadRequest(foodSection);
                    }
                    else
                    {
                        return BadRequest($"Failed to update FoodSection updated: {foodSection}");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while get update section");
                return BadRequest(ex.Message);
            }


        }
    }
}
