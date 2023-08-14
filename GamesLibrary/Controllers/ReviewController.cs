using Microsoft.AspNetCore.Mvc;
using GamesLibrary.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using GamesLibrary.Services;
using GamesLibrary.Repository.Interfaces;
using static GamesLibrary.Utils.Constants.ResponseConstants;

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
        /// Retrieves all reviews from the database.
        /// </summary>
        /// <remarks>
        /// This endpoint requires the user to have the "ManagerOnly" authorization policy.
        /// </remarks>
        /// <returns>
        /// Returns a list of all reviews if successful.
        /// If no reviews are found, returns a NotFound response with an appropriate message.
        /// If an error occurs during processing, returns a StatusCode 500 response with an error message.
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
                    return NotFound(REVIEW.NOT_FOUND);
                }

                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = REVIEW.NOT_FOUND, error = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a paginated list of reviews of a game based on the provided search and pagination options.
        /// </summary>
        /// <param name="options">The pagination and search options.</param>
        /// <returns>
        /// Returns a paginated list of reviews if successful.
        /// If no reviews are found, returns a NotFound response with an appropriate message.
        /// If an error occurs during processing, returns a StatusCode 500 response with an error message.
        /// </returns>
        [HttpGet("paginated/{gameId}")]
        public IActionResult GetAllReviewsByGamePaginated(int gameId, [FromQuery] PaginationAndSearchOptionsDto options)
        {
            try
            {
                if (gameId <= 0)
                {
                    return BadRequest(INVALID_ID);
                }

                var reviews = _reviewService.GetAllReviewsByGamePaginated(gameId, options);

                if (reviews == null)
                {
                    return NotFound(GAME.NOT_FOUND);
                }

                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = REVIEW.NOT_FOUND, error = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a review by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the review.</param>
        /// <returns>
        /// Returns the review's information if found.
        /// If the provided ID is invalid, returns a BadRequest response with an appropriate message.
        /// If no review is found with the specified ID, returns a NotFound response with an appropriate message.
        /// If an error occurs during processing, returns a StatusCode 500 response with an error message.
        /// </returns>information if found, otherwise a NotFound response.
        /// </returns>
        [HttpGet("{id}")]
        public IActionResult GetReviewById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(INVALID_ID);
                }
                var reviews = _reviewService.GetReviewById(id);

                if (reviews == null)
                {
                    return NotFound(GAME.NOT_FOUND);
                }

                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = REVIEW.NOT_FOUND, error = ex.Message });
            }
        }

        /// <summary>
        /// Adds a new review to the database.
        /// </summary>
        /// <param name="review">The review information to be added.</param>
        /// <remarks>
        /// This endpoint requires the user to have the "ManagerOnly" authorization policy.
        /// </remarks>
        /// <returns>
        /// Returns a CreatedAtAction response with the URL of the newly created review if successful.
        /// If the provided review data is invalid, returns a BadRequest response with an appropriate message.
        /// If an error occurs during processing, returns a StatusCode 500 response with an error message.
        /// </returns>
        [HttpPost]
        [Authorize]
        public IActionResult AddReview([FromBody] Review review)
        {
            try
            {
                if (review == null)
                {
                    return BadRequest(INVALID_DATA);
                }
                _reviewService.AddReview(review);

                return CreatedAtAction(nameof(GetReviewById), new { id = review.Id }, review);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = REVIEW.NOT_SAVED, error = ex.Message });
            }
        }

        /// <summary>
        /// Updates an existing review's information in the database.
        /// </summary>
        /// <param name="id">The ID of the review to be updated.</param>
        /// <param name="review">The updated review information.</param>
        /// <remarks>
        /// This endpoint requires the user to have the "ManagerOnly" authorization policy.
        /// </remarks>
        /// <returns>
        /// Returns a status code indicating the result of the update operation.
        /// If successful, returns a success message.
        /// If the provided ID is invalid, returns a BadRequest response.
        /// If the provided review data is invalid, returns a BadRequest response.
        /// If the review with the specified ID is not found, returns a NotFound response.
        /// If an error occurs during the update operation, returns a 500 Internal Server Error response with an error message.
        /// </returns>
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateReview(int id, [FromBody] Review review)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(INVALID_ID);
                }

                if (review == null)
                {
                    return BadRequest(INVALID_DATA);
                }

                _reviewService.UpdateReview(id, review);

                return Ok(new { message = REVIEW.SUCCES_UPDATING });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = REVIEW.ERROR_UPDATING, error = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a review from the database based on its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the review to be deleted.</param>
        /// <remarks>
        /// This endpoint requires the user to have the "ManagerOnly" authorization policy.
        /// </remarks>
        /// <returns>
        /// Returns a status code indicating the result of the update operation.
        /// If the provided ID is invalid, returns a BadRequest response with an appropriate message.
        /// If the review with the specified ID is not found, returns a NotFound response with an appropriate message.
        /// If an error occurs during processing, returns a StatusCode 500 response with an error message.
        /// </returns>
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteReview(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(INVALID_ID);
                }

                _reviewService.DeleteReview(id);
                return Ok(new { message = REVIEW.SUCCES_DELETING });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = REVIEW.ERROR_DELETING, error = ex.Message });
            }
        }
    }
}
