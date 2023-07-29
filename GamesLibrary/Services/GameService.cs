using GamesLibrary.DataAccessLayer.Data;
using GamesLibrary.DataAccessLayer.Models;
using GamesLibrary.Utils.Constants;

namespace GamesLibrary.Services
{
    public class GameService
    {
        private readonly GamesLibraryDbContext _dbContext;

        public GameService(GamesLibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

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

        public List<Game> SearchGames(string searchTerm)
        {
            try
            {
                return _dbContext.Games.Where(g =>
                    g.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    g.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    g.Genre.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    g.Developer.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    g.Platform.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    g.Price.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ResponseConstants.GAME.NOT_FOUND, ex);
            }
        }

        public void AddGame(Game game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game), ResponseConstants.GAME.NOT_FOUND);
            }

            try
            {
                _dbContext.Games.Add(game);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ResponseConstants.GAME.NOT_SAVED, ex);
            }
        }

        public void UpdateGame(Game game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game), ResponseConstants.GAME.NOT_FOUND);
            }

            try
            {
                _dbContext.Games.Update(game);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ResponseConstants.GAME.ERROR_UPDATING, ex);
            }
        }

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
