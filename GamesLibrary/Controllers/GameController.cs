using Microsoft.AspNetCore.Mvc;
using GamesLibrary.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using GamesLibrary.Services;
using GamesLibrary.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

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
        /// A list of all games if successful, otherwise a NotFound response.
        /// </returns>
        [HttpGet]
        [Authorize(Policy = "ManagerOnly")]
        public IActionResult GetAllGames()
        {
            try
            {
                var games = _gameService.GetAllGames();

                if (games == null)
                {
                    return NotFound();
                }

                return Ok(games);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a paginated list of games based on the provided search and pagination options.
        /// </summary>
        /// <param name="options">The pagination and search options.</param>
        /// <returns>
        /// A paginated list of games if successful, otherwise a NotFound response.
        /// </returns>
        [HttpGet("paginated")]
        public IActionResult GetAllGamesPaginated([FromQuery] PaginationAndSearchOptionsDto options)
        {
            try
            {
                var games = _gameService.GetAllGamesPaginated(options);

                if (games == null)
                {
                    return NotFound();
                }
                return Ok(games);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
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
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid ID.");
                }
                var game = _gameService.GetGameById(id);

                return Ok(game);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
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
            try
            {
                _gameService.AddGame(game);
                return CreatedAtAction(nameof(GetGameById), new { id = game.Id }, game);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Update an existing Game in the database.
        /// </summary>
        /// <param name="id">The ID of the Game to be updated.</param>
        /// <param name="game">The Game object containing the updated data.</param>
        /// <remarks>
        /// This endpoint requires the user to have the "ManagerOnly" authorization policy.
        /// </remarks>
        /// <returns>Returns a status code indicating the result of the update operation.</returns>
        /// <response code="204">The Game was successfully updated.</response>
        /// <response code="400">Bad request. The provided ID is invalid or the data received is invalid.</response>
        /// <response code="404">The Game with the specified ID was not found.</response>
        /// <response code="500">An error occurred while updating the game in the database.</response>
        [HttpPut("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public IActionResult UpdateGame(int id, [FromBody] Game game)
        {
            try
            {
                if (game == null)
                {
                return BadRequest("Invalid data received.");
                }

                if (id <= 0)
                {
                    return BadRequest("Invalid ID.");
                }

                _gameService.UpdateGame(id, game);
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
            try
            {
                if (id <= 0)
                {
                return BadRequest("Invalid ID.");
                }

                _gameService.DeleteGame(id);
                return NoContent();

            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
