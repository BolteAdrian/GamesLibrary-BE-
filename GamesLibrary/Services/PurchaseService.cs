using GamesLibrary.DataAccessLayer.Data;
using GamesLibrary.DataAccessLayer.Models;
using GamesLibrary.Utils.Constants;

namespace GamesLibrary.Services
{
    public class PurchaseService
    {
        private readonly GamesLibraryDbContext _dbContext;

        public PurchaseService(GamesLibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

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
