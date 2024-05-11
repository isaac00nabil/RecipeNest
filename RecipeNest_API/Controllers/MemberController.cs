using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeNest_Core.Dtos.Donation;
using RecipeNest_Core.Dtos.Review;
using RecipeNest_Core.IServices;
using RecipeNest_Core.Models.Context;
using RecipeNest_Core.Models.Entites;
using Serilog;

namespace RecipeNest_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberServiceInterface _memberService;
        private readonly RecipeNestDbContext _context;
        public MemberController(IMemberServiceInterface memberService, RecipeNestDbContext context)
        {
            _memberService = memberService;
            _context = context;
        }

        /// <summary>
        /// Make a donation from the member side.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/MakeDonation
        ///     {
        ///        "ApiKey": "your_api_key",
        ///        "UserId": 123,
        ///        "Type": "100", {"10": "Gold", "11": "Silver, "12": "Bronze, "13": "Platinum", "14": "Diamond"}
        ///        "PaymentMethod": "PaymentMethod",  {"100": "Visa", "101": "PayPal, "102": "Bitcoin"}
        ///        "Price": 10, // Price Range (10, 15, 20, 30, 40)
        ///        "Point": 5 // Point Range (5, 10, 15, 25, 35)
        ///     }
        /// 
        /// </remarks>
        /// <param name="ApiKey">The API key for authentication.</param>
        /// <param name="dto">The DTO containing the user's ID and donation information.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateDonation([FromHeader] string ApiKey, [FromBody] CreateDonationDTO dto)
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
                    var createDonation = await _memberService.CreateDonationDTO(dto);
                    return Ok(createDonation);
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while creating a new Donation");
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Create a new review from a member.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/CreateReview
        ///     {
        ///        "ApiKey": "your_api_key",
        ///        "Rating": 7.5,
        ///        "Comment": "A great experience!"
        ///     }
        /// </remarks>
        /// <param name="ApiKey">The API key for authentication.</param>
        /// <param name="dto">The DTO containing the review's rating and comment.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateReview([FromHeader] string ApiKey, [FromBody] CreateOrUpdateReviewDTO dto)
        {
            try
            {
                var userKey = await _context.Logins.FirstOrDefaultAsync(l => l.ApiKey == ApiKey);

                if (userKey == null)
                {
                    return Unauthorized("Invalid API Key");
                }

                var createReview = await _memberService.CreateReview(dto);
                return Ok(createReview);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while creating a new Review");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete a user account.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE api/DeleteAccount
        ///     {
        ///        "ApiKey": "your_api_key",
        ///        "userId": "Id of the User to delete"
        ///     }
        /// </remarks>
        /// <param name="ApiKey">The API key for authentication.</param>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpDelete]
        [Route("[action]/{userId}")]
        public async Task<IActionResult> DeleteAccount([FromHeader] string ApiKey, [FromRoute] int userId)
        {
            try
            {
                var userKey = await _context.Logins.FirstOrDefaultAsync(l => l.ApiKey == ApiKey);

                if (userKey == null)
                {
                    return Unauthorized("Invalid API Key");
                }

                var deleteAccount = await _memberService.DeleteAccount(userId);
                return deleteAccount.Contains("successfully") ? Ok(deleteAccount) : NotFound(deleteAccount);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while deleted account");
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Update a review.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/UpdateReview
        ///     {
        ///        "ApiKey": "your_api_key",
        ///        "ReviewId": 1,
        ///        "Rating": 9.5,
        ///        "Comment": "Updated comment"
        ///     }
        /// </remarks>
        /// <param name="ApiKey">The API key for authentication.</param>
        /// <param name="dto">The DTO containing the ID of the review to update and the new rating and comment.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> UpdateReview([FromHeader] string ApiKey, [FromBody] CreateOrUpdateReviewDTO dto)
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
                    var review = await _memberService.UpdateReview(dto);
                    if (review.Contains("find"))
                    {
                        return NotFound(review);
                    }
                    else if (review.Contains("successfully"))
                    {
                        return Ok(review);
                    }
                    else
                    {
                        return BadRequest(review);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while deleted account");
                return BadRequest(ex.Message);
            }
        }
    }
}
