using GamesLibrary.Repository.Data;
using GamesLibrary.Repository.Models;
using static GamesLibrary.Utils.Constants.ResponseConstants;

namespace GamesLibrary.Services
{
    public class PurchaseService
    {
        private readonly GamesLibraryDbContext _dbContext;

        public PurchaseService(GamesLibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves all purchases from the database.
        /// </summary>
        /// <returns>
        /// Returns a list of all purchases if successful.
        /// If an error occurs during processing, throws an exception with an error message.
        /// </returns>
        public List<Purchase> GetAllPurchases()
        {
            try
            {
                var purchases =  _dbContext.Purchases.ToList();

                return purchases;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a purchase by its unique identifier from the database.
        /// </summary>
        /// <param name="id">The unique identifier of the purchase.</param>
        /// <returns>
        /// Returns the purchase object if found.
        /// If the purchase with the specified ID is not found, throws an exception with an appropriate error message.
        /// If an error occurs during processing, throws an exception with an error message.
        /// </returns>on cref="Exception">Thrown when the purchase is not found or there is an error retrieving it.</exception>
        public Purchase GetPurchaseById(int id)
        {
            try
            {
                var purchase = _dbContext.Purchases.FirstOrDefault(p => p.Id == id);
                if (purchase == null)
                {
                    throw new Exception(PURCHASE.NOT_FOUND);
                }
                return purchase;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(PURCHASE.NOT_FOUND, id), ex);
            }
        }

        /// <summary>
        /// Get all purchases made by a user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A list of purchases made by the user.</returns>
        /// <exception cref="Exception">Thrown when there is an error retrieving the purchases.</exception>
        public List<Purchase> GetPurchasesByUserId(string userId)
        {
            try
            {
                return _dbContext.Purchases.Where(p => p.UserId == userId).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(PURCHASE.NOT_FOUND, ex);
            }
        }

        /// <summary>
        /// Add a new purchase to the database.
        /// </summary>
        /// <param name="purchase">The Purchase object to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown when the purchase object is null.</exception>
        /// <exception cref="Exception">Thrown when there is an error saving the purchase to the database.</exception>
        public void AddPurchase(Purchase purchase)
        {
            try
            {
                _dbContext.Purchases.Add(purchase);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(PURCHASE.NOT_SAVED, ex);
            }
        }

        /// <summary>
        /// Update an existing Purchase in the database.
        /// </summary>
        /// <param name="id">The ID of the Purchase to be updated.</param>
        /// <param name="purchase">The Purchase object containing the updated data.</param>
        /// <exception cref="ArgumentNullException">Thrown when the purchase object is null.</exception>
        /// <exception cref="Exception">Thrown when the purchase with the specified ID is not found or there is an error updating the purchase in the database.</exception>
        public void UpdatePurchase(int id,Purchase purchase)
        {
            try
            {
                var existingPurchase = _dbContext.Purchases.Find(id);

                if (existingPurchase == null)
                {
                    throw new Exception(string.Format(PURCHASE.NOT_FOUND, id));
                }

                existingPurchase.UserId = purchase.UserId;
                existingPurchase.PurchaseDate = purchase.PurchaseDate;
                existingPurchase.GameId = purchase.GameId;

                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(PURCHASE.ERROR_UPDATING, id), ex);
            }
        }

        /// <summary>
        /// Delete a purchase from the database based on its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the purchase to be deleted.</param>
        /// <exception cref="Exception">Thrown when the purchase is not found or there is an error deleting it from the database.</exception>
        public void DeletePurchase(int id)
        {
            try
            {
                var purchase = _dbContext.Purchases.Find(id);

                if (purchase == null)
                {
                    throw new Exception(string.Format(PURCHASE.NOT_FOUND, id));
                }

                _dbContext.Purchases.Remove(purchase);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(PURCHASE.ERROR_DELETING, id), ex);
            }
        }
    }
}
