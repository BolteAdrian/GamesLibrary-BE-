using GamesLibrary.Repository.Data;
using GamesLibrary.Repository.Interfaces;
using GamesLibrary.Repository.Models;
using System.Data.Entity.Infrastructure;
using static GamesLibrary.Utils.Constants.ResponseConstants;

namespace GamesLibrary.Services
{
    public class ReviewService
    {
        private readonly GamesLibraryDbContext _dbContext;

        public ReviewService(GamesLibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get all reviews.
        /// </summary>
        /// <returns>A list of all reviews.</returns>
        /// <exception cref="Exception">Thrown when there is an error retrieving the reviews.</exception>
        public List<Review> GetAllReviews()
        {
            try
            {
                return _dbContext.Reviews.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(REVIEW.NOT_FOUND, ex);
            }
        }

        /// <summary>
        /// Get all reviews with pagination and search options.
        /// </summary>
        /// <param name="options">The pagination and search options.</param>
        /// <returns>A list of reviews based on the provided pagination and search criteria.</returns>
        /// <exception cref="Exception">Thrown when there is an error retrieving the reviews.</exception>
        public List<Review> GetAllReviewsByGamePaginated(int gameId, PaginationAndSearchOptionsDto options)
        {
            try
            {
                IQueryable<Review> query = _dbContext.Reviews.Where(r => r.GameId == gameId).AsQueryable();

                // Sorting
                if (!string.IsNullOrEmpty(options.SortField))
                {
                    // In this example, we'll use the SortOrder enum to decide whether sorting is done in ascending or descending order
                    bool isAscending = options.SortOrder == SortOrder.Ascending;
                    query = SortQuery(query, options.SortField, isAscending);
                }

                // Calculate the total number of records
                int totalItems = query.Count();

                // Pagination - calculate the start index and retrieve the desired number of elements
                int startIndex = (options.PageNumber - 1) * options.PageSize;
                query = query.Skip(startIndex).Take(options.PageSize);

                // Return the results and pagination information
                return query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(REVIEW.NOT_FOUND, ex);
            }
        }

        /// <summary>
        /// Sort the query based on the provided sort field and sort order.
        /// </summary>
        /// <param name="query">The query to be sorted.</param>
        /// <param name="sortField">The field to be used for sorting.</param>
        /// <param name="isAscending">True for ascending sorting, false for descending sorting.</param>
        /// <returns>The sorted query.</returns>
        private IQueryable<Review> SortQuery(IQueryable<Review> query, string sortField, bool isAscending)
        {
            switch (sortField.ToLower())
            {
                case "Rating":
                    return isAscending ? query.OrderBy(g => g.Rating) : query.OrderByDescending(g => g.Rating);
                case "Comment":
                    return isAscending ? query.OrderBy(g => g.Comment) : query.OrderByDescending(g => g.Comment);
                default:
                    return query; // If the sorting field does not exist or is not specified, return the unchanged query
            }
        }

        /// <summary>
        /// Get a Review by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the review.</param>
        /// <returns>The Review object if found, otherwise throws an exception.</returns>
        /// <exception cref="Exception">Thrown when the review with the specified ID is not found or there is an error retrieving the review.</exception>
        public Review GetReviewById(int id)
        {
            try
            {
                var review = _dbContext.Reviews.FirstOrDefault(g => g.Id == id);
                if (review == null)
                {
                    throw new Exception(string.Format(REVIEW.NOT_FOUND, id));
                }
                return review;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(REVIEW.NOT_FOUND, id), ex);
            }
        }

        /// <summary>
        /// Add a new Review to the database.
        /// </summary>
        /// <param name="review">The Review object to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown when the review object is null.</exception>
        /// <exception cref="Exception">Thrown when there is an error saving the review to the database.</exception>
        public void AddReview(Review review)
        {
            try
            {
                _dbContext.Reviews.Add(review);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(REVIEW.NOT_SAVED, ex);
            }
        }

        /// <summary>
        /// Update an existing Review in the database.
        /// </summary>
        /// <param name="id">The ID of the Review to be updated.</param>
        /// <param name="review">The Review object containing the updated data.</param>
        /// <exception cref="ArgumentNullException">Thrown when the review object is null.</exception>
        /// <exception cref="Exception">Thrown when the review with the specified ID is not found or there is an error updating the review in the database.</exception>
        public void UpdateReview(int id, Review review)
        {
            try
            {
                var existingReview = _dbContext.Reviews.Find(id);

                if (existingReview == null)
                {
                    throw new Exception(string.Format(REVIEW.NOT_FOUND, id));
                }

                existingReview.UserId = review.UserId;
                existingReview.GameId = review.GameId;
                existingReview.Rating = review.Rating;
                existingReview.Comment = review.Comment;

                _dbContext.SaveChanges();

            }
            catch (DbUpdateException ex)
            {
                throw new Exception(string.Format(REVIEW.ERROR_UPDATING, id), ex);
            }
        }


        /// <summary>
        /// Delete a Review from the database based on its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the review to be deleted.</param>
        /// <exception cref="Exception">Thrown when the review is not found or there is an error deleting it from the database.</exception>
        public void DeleteReview(int id)
        {
            try
            {
                var review = _dbContext.Reviews.Find(id);

                if (review == null)
                {
                    throw new Exception(string.Format(REVIEW.NOT_FOUND, id));
                }

                _dbContext.Reviews.Remove(review);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(REVIEW.ERROR_DELETING, id), ex);
            }
        }
    }
}
