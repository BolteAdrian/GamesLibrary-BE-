using GamesLibrary.DataAccessLayer.Data;
using GamesLibrary.DataAccessLayer.Interfaces;
using GamesLibrary.DataAccessLayer.Models;
using GamesLibrary.Utils.Constants;
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
        /// Get all purchases.
        /// </summary>
        /// <returns>A list of all purchases.</returns>
        /// <exception cref="Exception">Thrown when there is an error retrieving the purchases.</exception>
        public List<Purchase> GetAllPurchases()
        {
            try
            {
                return _dbContext.Purchases.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ResponseConstants.UNKNOWN, ex);
            }
        }

        /// <summary>
        /// Get a purchase by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the purchase.</param>
        /// <returns>The Purchase object if found, otherwise throws an exception.</returns>
        /// <exception cref="Exception">Thrown when the purchase is not found or there is an error retrieving it.</exception>
        public Purchase GetPurchaseById(int id)
        {
            try
            {
                var purchase = _dbContext.Purchases.FirstOrDefault(p => p.Id == id);
                if (purchase == null)
                {
                    throw new Exception(ResponseConstants.PURCHASE.NOT_FOUND);
                }
                return purchase;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(ResponseConstants.UNKNOWN, id), ex);
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
                throw new Exception(ResponseConstants.PURCHASE.NOT_FOUND, ex);
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
            if (purchase == null)
            {
                throw new ArgumentNullException(nameof(purchase), ResponseConstants.UNKNOWN);
            }

            try
            {
                _dbContext.Purchases.Add(purchase);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ResponseConstants.PURCHASE.NOT_SAVED, ex);
            }
        }

        /// <summary>
        /// Update an existing purchase in the database.
        /// </summary>
        /// <param name="purchase">The Purchase object to be updated.</param>
        /// <exception cref="ArgumentNullException">Thrown when the purchase object is null.</exception>
        /// <exception cref="Exception">Thrown when there is an error updating the purchase in the database.</exception>
        public void UpdatePurchase(Purchase purchase)
        {
            if (purchase == null)
            {
                throw new ArgumentNullException(nameof(purchase), ResponseConstants.UNKNOWN);
            }

            try
            {
                _dbContext.Purchases.Update(purchase);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ResponseConstants.PURCHASE.ERROR_UPDATING, ex);
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
                    throw new Exception(string.Format(ResponseConstants.PURCHASE.NOT_FOUND, id));
                }

                _dbContext.Purchases.Remove(purchase);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(ResponseConstants.PURCHASE.ERROR_DELETING, id), ex);
            }
        }
    }
}
