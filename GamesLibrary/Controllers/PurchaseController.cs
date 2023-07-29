using Microsoft.AspNetCore.Mvc;
using GamesLibrary.DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using GamesLibrary.Services;

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

        [HttpGet]
        [Authorize(Policy = "ManagerOnly")]
        public IActionResult GetAllPurchases()
        {
            var purchases = _purchaseService.GetAllPurchases();
            return Ok(purchases);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public IActionResult GetPurchaseById(int id)
        {
            var purchase = _purchaseService.GetPurchaseById(id);
            if (purchase == null)
            {
                return NotFound();
            }
            return Ok(purchase);
        }

        [HttpGet("user/{userId}")]
        [Authorize]
        public IActionResult GetPurchasesByUserId(string userId)
        {
            var purchases = _purchaseService.GetPurchasesByUserId(userId);
            if (purchases.Count == 0)
            {
                return NotFound();
            }
            return Ok(purchases);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddPurchase([FromBody] Purchase purchase)
        {
            _purchaseService.AddPurchase(purchase);
            return CreatedAtAction(nameof(GetPurchaseById), new { id = purchase.Id }, purchase);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public IActionResult UpdatePurchase(int id, [FromBody] Purchase purchase)
        {
            if (id != purchase.Id)
            {
                return BadRequest();
            }

            var existingPurchase = _purchaseService.GetPurchaseById(id);
            if (existingPurchase == null)
            {
                return NotFound();
            }

            _purchaseService.UpdatePurchase(purchase);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeletePurchase(int id)
        {
            var purchase = _purchaseService.GetPurchaseById(id);
            if (purchase == null)
            {
                return NotFound();
            }

            _purchaseService.DeletePurchase(id);
            return NoContent();
        }
    }
}
