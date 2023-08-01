using Microsoft.AspNetCore.Mvc;
using GamesLibrary.DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using GamesLibrary.Services;
using Microsoft.EntityFrameworkCore;

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
        /// Retrieves all purchases.
        /// </summary>
        /// <remarks>
        /// This endpoint requires the user to have the "ManagerOnly" authorization policy.
        /// </remarks>
        /// <returns>
        /// A list of all purchases if successful, otherwise an error response.
        /// </returns>
        [HttpGet]
        [Authorize(Policy = "ManagerOnly")]
        public IActionResult GetAllPurchases()
        {
            try
            {
                var purchases = _purchaseService.GetAllPurchases();
                return Ok(purchases);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a purchase by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the purchase.</param>
        /// <remarks>
        /// This endpoint requires the user to be authorized.
        /// </remarks>
        /// <returns>
        /// The purchase's information if found, otherwise a NotFound response.
        /// </returns>
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetPurchaseById(int id)
        {
            try
            {
                if (id <= 0)
                {
                return BadRequest("Invalid ID.");
                }

            var purchase = _purchaseService.GetPurchaseById(id);

            return Ok(purchase);

            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all purchases made by a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose purchases are being retrieved.</param>
        /// <remarks>
        /// This endpoint requires the user to be authorized.
        /// </remarks>
        /// <returns>
        /// A list of purchases made by the user if successful, otherwise a NotFound response.
        /// </returns>
        [HttpGet("user/{userId}")]
        [Authorize]
        public IActionResult GetPurchasesByUserId(string userId)
        {
            try
            {
                if (userId == null)
                {
                return BadRequest("Invalid ID.");
                }
                var purchases = _purchaseService.GetPurchasesByUserId(userId);
                if (purchases.Count == 0)
                {
                    return NotFound();
                }
                return Ok(purchases);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Adds a new purchase.
        /// </summary>
        /// <param name="purchase">The purchase information to be added.</param>
        /// <remarks>
        /// This endpoint requires the user to be authorized.
        /// </remarks>
        /// <returns>
        /// If successful, returns a CreatedAtAction response with the URL of the newly created purchase.
        /// </returns>
        [HttpPost]
        [Authorize]
        public IActionResult AddPurchase([FromBody] Purchase purchase)
        {
            try
            {
                _purchaseService.AddPurchase(purchase);
                return CreatedAtAction(nameof(GetPurchaseById), new { id = purchase.Id }, purchase);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing purchase.
        /// </summary>
        /// <param name="id">The unique identifier of the purchase to update.</param>
        /// <param name="purchase">The updated purchase information.</param>
        /// <remarks>
        /// This endpoint requires the user to have the "ManagerOnly" authorization policy.
        /// </remarks>
        /// <returns>
        /// If successful, returns a NoContent response.
        /// If the provided id is invalid, returns a BadRequest response.
        /// </returns>
        [HttpPut("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public IActionResult UpdatePurchase(int id, [FromBody] Purchase purchase)
        {
            try
            {
                if (purchase == null)
                {
                return BadRequest("Invalid data received.");
                }

                if (id <= 0)
                {
                    return BadRequest("Invalid ID.");
                }

                _purchaseService.UpdatePurchase(id,purchase);
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
        /// Deletes a purchase by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the purchase to delete.</param>
        /// <remarks>
        /// This endpoint requires the user to have the "ManagerOnly" authorization policy.
        /// </remarks>
        /// <returns>
        /// If successful, returns a NoContent response.
        /// If the purchase to delete is not found, returns a NotFound response.
        /// </returns>
        [HttpDelete("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public IActionResult DeletePurchase(int id)
        {
            try
            {
                if (id <= 0)
                {
                return BadRequest("Invalid ID.");
                }

                _purchaseService.DeletePurchase(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

