using Microsoft.AspNetCore.Mvc;
using GamesLibrary.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using GamesLibrary.Services;
using static GamesLibrary.Utils.Constants.ResponseConstants;

namespace GamesLibrary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : ControllerBase
    {
        private readonly PurchaseService _purchaseService;

        public PurchaseController(PurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        /// <summary>
        /// Retrieves all purchases from the database.
        /// </summary>
        /// <remarks>
        /// This endpoint requires the user to have the "ManagerOnly" authorization policy.
        /// </remarks>
        /// <returns>
        /// Returns a list of all purchases if successful.
        /// If no purchases are found, returns a NotFound response with an appropriate message.
        /// If an error occurs during processing, returns a StatusCode 500 response with an error message.
        /// </returns>
        [HttpGet]
        [Authorize(Policy = "ManagerOnly")]
        public IActionResult GetAllPurchases()
        {
            try
            {
                var purchases = _purchaseService.GetAllPurchases();

                if (purchases == null)
                {
                    return NotFound(PURCHASE.NOT_FOUND);
                }

                return Ok(purchases);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = PURCHASE.NOT_FOUND, error = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a purchase by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the purchase.</param>
        /// <returns>
        /// Returns the purchase's information if found.
        /// If the provided ID is invalid, returns a BadRequest response with an appropriate message.
        /// If no purchase is found with the specified ID, returns a NotFound response with an appropriate message.
        /// If an error occurs during processing, returns a StatusCode 500 response with an error message.
        /// </returns>
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetPurchaseById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(INVALID_ID);
                }
                var purchase = _purchaseService.GetPurchaseById(id);

                if (purchase == null)
                {
                    return NotFound(PURCHASE.NOT_FOUND);
                }

                return Ok(purchase);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = PURCHASE.NOT_FOUND, error = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves all purchases by its user identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>
        /// Returns a list of all purchases of a user if successful.
        /// If the provided ID is invalid, returns a BadRequest response with an appropriate message.
        /// If no purchase is found with the specified ID, returns a NotFound response with an appropriate message.
        /// If an error occurs during processing, returns a StatusCode 500 response with an error message.
        /// </returns>
        [HttpGet("user/{userId}")]
        [Authorize]
        public IActionResult GetPurchasesByUserId(string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return BadRequest(INVALID_DATA);
                }
                var purchases = _purchaseService.GetPurchasesByUserId(userId);

                if (purchases == null)
                {
                    return NotFound(PURCHASE.NOT_FOUND);
                }
                return Ok(purchases);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = PURCHASE.NOT_FOUND, error = ex.Message });
            }
        }

        /// <summary>
        /// Adds a new purchase to the database.
        /// </summary>
        /// <param name="purchase">The purchase information to be added.</param>
        /// <remarks>
        /// This endpoint requires the user to have the "ManagerOnly" authorization policy.
        /// </remarks>
        /// <returns>
        /// Returns a CreatedAtAction response with the URL of the newly created purchase if successful.
        /// If the provided purchase data is invalid, returns a BadRequest response with an appropriate message.
        /// If an error occurs during processing, returns a StatusCode 500 response with an error message.
        /// </returns>
        [HttpPost]
        [Authorize]
        public IActionResult AddPurchase([FromBody] Purchase purchase)
        {
            try
            {
                if (purchase == null)
                {
                    return BadRequest(INVALID_DATA);
                }

                _purchaseService.AddPurchase(purchase);

                return CreatedAtAction(nameof(GetPurchaseById), new { id = purchase.Id }, purchase);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = PURCHASE.NOT_SAVED, error = ex.Message });
            }
        }

        /// <summary>
        /// Updates an existing purchase's information in the database.
        /// </summary>
        /// <param name="id">The ID of the purchase to be updated.</param>
        /// <param name="purchase">The updated purchase information.</param>
        /// <remarks>
        /// This endpoint requires the user to have the "ManagerOnly" authorization policy.
        /// </remarks>
        /// <returns>
        /// Returns a status code indicating the result of the update operation.
        /// If successful, returns a success message.
        /// If the provided ID is invalid, returns a BadRequest response.
        /// If the provided purchase data is invalid, returns a BadRequest response.
        /// If the purchase with the specified ID is not found, returns a NotFound response.
        /// If an error occurs during the update operation, returns a 500 Internal Server Error response with an error message.
        /// </returns>
        [HttpPut("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public IActionResult UpdatePurchase(int id, [FromBody] Purchase purchase)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(INVALID_ID);
                }

                if (purchase == null)
                {
                    return BadRequest(INVALID_DATA);
                }

                _purchaseService.UpdatePurchase(id, purchase);

                return Ok(new { message = PURCHASE.SUCCES_UPDATING });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = PURCHASE.ERROR_UPDATING, error = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a purchase from the database based on its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the purchase to be deleted.</param>
        /// <remarks>
        /// This endpoint requires the user to have the "ManagerOnly" authorization policy.
        /// </remarks>
        /// <returns>
        /// Returns a status code indicating the result of the update operation.
        /// If the provided ID is invalid, returns a BadRequest response with an appropriate message.
        /// If the purchase with the specified ID is not found, returns a NotFound response with an appropriate message.
        /// If an error occurs during processing, returns a StatusCode 500 response with an error message.
        /// </returns>
        [HttpDelete("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public IActionResult DeletePurchase(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(INVALID_ID);
                }

                _purchaseService.DeletePurchase(id);
                return Ok(new { message = PURCHASE.SUCCES_DELETING });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = PURCHASE.ERROR_DELETING, error = ex.Message });
            }
        }
    }
}

