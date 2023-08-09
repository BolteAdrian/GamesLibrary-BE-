using Microsoft.AspNetCore.Mvc;
using GamesLibrary.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using GamesLibrary.Services;
using GamesLibrary.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace GamesLibrary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewService _reviewService;

        public ReviewController(ReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        /// <summary>
        /// Retrieves all reviews.
        /// </summary>
        /// <remarks>
        /// This endpoint requires the user to have the "ManagerOnly" authorization policy.
        /// </remarks>
        /// <returns>
        /// A list of all reviews if successful, otherwise a NotFound response.
        /// </returns>
        [HttpGet]
        [Authorize(Policy = "ManagerOnly")]
        public IActionResult GetAllReviews()
        {
            try
            {
                var reviews = _reviewService.GetAllReviews();

                if (reviews == null)
                {
                    return NotFound();
                }

                return Ok(reviews);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a paginated list of reviews based on the provided search and pagination options.
        /// </summary>
        /// <param name="gameId">The ID of the Game that have the reviews.</param>
        /// <param name="options">The pagination and search options.</param>
        /// <returns>
        /// A paginated list of reviews if successful, otherwise a NotFound response.
        /// </returns>
        [HttpGet("paginated/{gameId}")]
        public IActionResult GetAllReviewsByGamePaginated(int gameId, [FromQuery] PaginationAndSearchOptionsDto options)
        {
            try
            {
                if (gameId >= 0)
                {
                    return BadRequest("Invalid ID.");
                }

                var reviews = _reviewService.GetAllReviewsByGamePaginated(gameId, options);

                if (reviews == null)
                {
                    return NotFound();
                }
                return Ok(reviews);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a reviews by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the reviews.</param>
        /// <returns>
        /// The reviews's information if found, otherwise a NotFound response.
        /// </returns>
        [HttpGet("{id}")]
        public IActionResult GetReviewById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid ID.");
                }
                var reviews = _reviewService.GetReviewById(id);

                return Ok(reviews);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Adds a new review.
        /// </summary>
        /// <param name="review">The review information to be added.</param>
        /// <remarks>
        /// This endpoint requires the user to be authorized.
        /// </remarks> 
        /// <returns>
        /// If successful, returns a CreatedAtAction response with the URL of the newly created review.
        /// </returns>
        [HttpPost]
        [Authorize]
        public IActionResult AddReview([FromBody] Review review)
        {
            try
            {
                _reviewService.AddReview(review);
                return CreatedAtAction(nameof(GetReviewById), new { id = review.Id }, review);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Update an existing Review in the database.
        /// </summary>
        /// <param name="id">The ID of the Review to be updated.</param>
        /// <param name="review">The Review object containing the updated data.</param>
        /// <remarks>
        /// This endpoint requires the user to be authorized.
        /// </remarks> 
        /// <returns>Returns a status code indicating the result of the update operation.</returns>
        /// <response code="204">The Review was successfully updated.</response>
        /// <response code="400">Bad request. The provided ID is invalid or the data received is invalid.</response>
        /// <response code="404">The Review with the specified ID was not found.</response>
        /// <response code="500">An error occurred while updating the review in the database.</response>
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateReview(int id, [FromBody] Review review)
        {
            try
            {
                if (review == null)
                {
                return BadRequest("Invalid data received.");
                }

                if (id <= 0)
                {
                    return BadRequest("Invalid ID.");
                }

                _reviewService.UpdateReview(id, review);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the game.");
            }
        }


        /// <summary>
        /// Deletes a review by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the review to delete.</param>
        /// <remarks>
        /// This endpoint requires the user to be authorized.
        /// </remarks>
        /// <returns>
        /// If successful, returns a NoContent response.
        /// If the review to delete is not found, returns a NotFound response.
        /// </returns>
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteReview(int id)
        {
            try
            {
                if (id <= 0)
                {
                return BadRequest("Invalid ID.");
                }

                _reviewService.DeleteReview(id);
                return NoContent();

            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
