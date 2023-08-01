using Microsoft.AspNetCore.Mvc;
using GamesLibrary.DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using GamesLibrary.Services;
using GamesLibrary.DataAccessLayer.Interfaces;

namespace GamesLibrary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;

        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }

        /// <summary>
        /// Retrieves all games.
        /// </summary>
        /// <remarks>
        /// This endpoint requires the user to have the "ManagerOnly" authorization policy.
        /// </remarks>
        /// <returns>
        /// A list of all games if successful, otherwise an error response.
        /// </returns>
        [HttpGet]
        [Authorize(Policy = "ManagerOnly")]
        public IActionResult GetAllGames()
        {
            var games = _gameService.GetAllGames();
            return Ok(games);
        }

        /// <summary>
        /// Retrieves a paginated list of games based on the provided search and pagination options.
        /// </summary>
        /// <param name="options">The pagination and search options.</param>
        /// <returns>
        /// A paginated list of games if successful, otherwise an error response.
        /// </returns>
        [HttpGet("paginated")]
        public IActionResult GetAllGamesPaginated([FromQuery] PaginationAndSearchOptions options)
        {
            var games = _gameService.GetAllGamesPaginated(options);
            return Ok(games);
        }

        /// <summary>
        /// Retrieves a game by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the game.</param>
        /// <returns>
        /// The game's information if found, otherwise a NotFound response.
        /// </returns>
        [HttpGet("{id}")]
        public IActionResult GetGameById(int id)
        {
            var game = _gameService.GetGameById(id);
            if (game == null)
            {
                return NotFound();
            }
            return Ok(game);
        }

        /// <summary>
        /// Adds a new game.
        /// </summary>
        /// <param name="game">The game information to be added.</param>
        /// <remarks>
        /// This endpoint requires the user to have the "ManagerOnly" authorization policy.
        /// </remarks>
        /// <returns>
        /// If successful, returns a CreatedAtAction response with the URL of the newly created game.
        /// </returns>
        [HttpPost]
        [Authorize(Policy = "ManagerOnly")]
        public IActionResult AddGame([FromBody] Game game)
        {
            _gameService.AddGame(game);
            return CreatedAtAction(nameof(GetGameById), new { id = game.Id }, game);
        }

        /// <summary>
        /// Updates an existing game.
        /// </summary>
        /// <param name="id">The unique identifier of the game to update.</param>
        /// <param name="game">The updated game information.</param>
        /// <remarks>
        /// This endpoint requires the user to have the "ManagerOnly" authorization policy.
        /// </remarks>
        /// <returns>
        /// If successful, returns a NoContent response.
        /// If the provided id invalid returns a BadRequest response.
        /// </returns>
        [HttpPut("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public IActionResult UpdateGame(int id, [FromBody] Game game)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            game.Id = id;

            _gameService.UpdateGame(game);
            return NoContent();
        }

        /// <summary>
        /// Deletes a game by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the game to delete.</param>
        /// <remarks>
        /// This endpoint requires the user to have the "ManagerOnly" authorization policy.
        /// </remarks>
        /// <returns>
        /// If successful, returns a NoContent response.
        /// If the game to delete is not found, returns a NotFound response.
        /// </returns>
        [HttpDelete("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public IActionResult DeleteGame(int id)
        {
            var game = _gameService.GetGameById(id);
            if (game == null)
            {
                return NotFound();
            }

            _gameService.DeleteGame(id);
            return NoContent();
        }
    }
}
