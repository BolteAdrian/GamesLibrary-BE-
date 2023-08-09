using GamesLibrary.Repository.Data;
using GamesLibrary.Repository.Interfaces;
using GamesLibrary.Repository.Models;
using GamesLibrary.Utils.Constants;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;

namespace GamesLibrary.Services
{
    public class GameService
    {
        private readonly GamesLibraryDbContext _dbContext;

        public GameService(GamesLibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get all games.
        /// </summary>
        /// <returns>A list of all games.</returns>
        /// <exception cref="Exception">Thrown when there is an error retrieving the games.</exception>
        public List<Game> GetAllGames()
        {
            try
            {
                return _dbContext.Games.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ResponseConstants.GAME.NOT_FOUND, ex);
            }
        }

        /// <summary>
        /// Get all games with pagination and search options.
        /// </summary>
        /// <param name="options">The pagination and search options.</param>
        /// <returns>A list of games based on the provided pagination and search criteria.</returns>
        /// <exception cref="Exception">Thrown when there is an error retrieving the games.</exception>
        public List<Game> GetAllGamesPaginated(PaginationAndSearchOptionsDto options)
        {
            try
            {
                IQueryable<Game> query = _dbContext.Games.AsQueryable();

                // Apply search filters
                if (!string.IsNullOrEmpty(options.SearchTerm))
                {
                    string searchTermLower = options.SearchTerm.ToLower();
                    query = query.Where(g =>
                        options.SearchFields.Any(f => g.Title.ToLower().Contains(searchTermLower)) ||
                        options.SearchFields.Any(f => g.Description.ToLower().Contains(searchTermLower)) ||
                        options.SearchFields.Any(f => g.Genre.ToLower().Contains(searchTermLower)) ||
                        options.SearchFields.Any(f => g.Developer.ToLower().Contains(searchTermLower)) ||
                        options.SearchFields.Any(f => g.Platform.ToLower().Contains(searchTermLower)) ||
                        options.SearchFields.Any(f => g.Price.ToString().Contains(searchTermLower))
                    );
                }

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
                throw new Exception(ResponseConstants.GAME.NOT_FOUND, ex);
            }
        }

        /// <summary>
        /// Sort the query based on the provided sort field and sort order.
        /// </summary>
        /// <param name="query">The query to be sorted.</param>
        /// <param name="sortField">The field to be used for sorting.</param>
        /// <param name="isAscending">True for ascending sorting, false for descending sorting.</param>
        /// <returns>The sorted query.</returns>
        private IQueryable<Game> SortQuery(IQueryable<Game> query, string sortField, bool isAscending)
        {
            switch (sortField.ToLower())
            {
                case "title":
                    return isAscending ? query.OrderBy(g => g.Title) : query.OrderByDescending(g => g.Title);
                case "description":
                    return isAscending ? query.OrderBy(g => g.Description) : query.OrderByDescending(g => g.Description);
                case "genre":
                    return isAscending ? query.OrderBy(g => g.Genre) : query.OrderByDescending(g => g.Genre);
                case "developer":
                    return isAscending ? query.OrderBy(g => g.Developer) : query.OrderByDescending(g => g.Developer);
                case "platform":
                    return isAscending ? query.OrderBy(g => g.Platform) : query.OrderByDescending(g => g.Platform);
                case "price":
                    return isAscending ? query.OrderBy(g => g.Price) : query.OrderByDescending(g => g.Price);
                default:
                    return query; // If the sorting field does not exist or is not specified, return the unchanged query
            }
        }

        /// <summary>
        /// Get a Game by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the game.</param>
        /// <returns>The Game object if found, otherwise throws an exception.</returns>
        /// <exception cref="Exception">Thrown when the game with the specified ID is not found or there is an error retrieving the game.</exception>
        public Game GetGameById(int id)
        {
            try
            {
                var game = _dbContext.Games.FirstOrDefault(g => g.Id == id);
                if (game == null)
                {
                    throw new Exception(string.Format(ResponseConstants.GAME.NOT_FOUND, id));
                }
                return game;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(ResponseConstants.GAME.NOT_FOUND, id), ex);
            }
        }

        /// <summary>
        /// Add a new Game to the database.
        /// </summary>
        /// <param name="game">The Game object to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown when the game object is null.</exception>
        /// <exception cref="Exception">Thrown when there is an error saving the game to the database.</exception>
        public void AddGame(Game game)
        {
            try
            {
                if (game == null)
                {
                throw new ArgumentNullException(nameof(game), ResponseConstants.GAME.NOT_FOUND);
                }

                _dbContext.Games.Add(game);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ResponseConstants.GAME.NOT_SAVED, ex);
            }
        }

        /// <summary>
        /// Update an existing Game in the database.
        /// </summary>
        /// <param name="id">The ID of the Game to be updated.</param>
        /// <param name="game">The Game object containing the updated data.</param>
        /// <exception cref="ArgumentNullException">Thrown when the game object is null.</exception>
        /// <exception cref="Exception">Thrown when the game with the specified ID is not found or there is an error updating the game in the database.</exception>
        public void UpdateGame(int id, Game game)
        {
            try
            {
                var existingGame = _dbContext.Games.Find(id);

                if (existingGame == null)
                {
                    throw new Exception(string.Format(ResponseConstants.GAME.NOT_FOUND, id));
                }

                existingGame.Title = game.Title;
                existingGame.Description = game.Description;
                existingGame.ReleaseDate = game.ReleaseDate;
                existingGame.Genre = game.Genre;
                existingGame.Developer = game.Developer;
                existingGame.Platform = game.Platform;
                existingGame.Price = game.Price;

                _dbContext.SaveChanges();

            }
            catch (DbUpdateException ex)
            {
                throw new Exception(string.Format(ResponseConstants.GAME.ERROR_UPDATING, id), ex);
            }
        }


        /// <summary>
        /// Delete a Game from the database based on its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the game to be deleted.</param>
        /// <exception cref="Exception">Thrown when the game is not found or there is an error deleting it from the database.</exception>
        public void DeleteGame(int id)
        {
            try
            {
                var game = _dbContext.Games.Find(id);

                if (game == null)
                {
                    throw new Exception(string.Format(ResponseConstants.GAME.NOT_FOUND, id));
                }

                _dbContext.Games.Remove(game);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(ResponseConstants.GAME.ERROR_DELETING, id), ex);
            }
        }
    }
}
