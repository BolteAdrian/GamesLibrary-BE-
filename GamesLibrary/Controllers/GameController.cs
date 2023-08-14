using Microsoft.AspNetCore.Mvc;
using GamesLibrary.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using GamesLibrary.Services;
using GamesLibrary.Repository.Interfaces;
using static GamesLibrary.Utils.Constants.ResponseConstants;

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
        /// Retrieves all games from the database.
        /// </summary>
        /// <remarks>
        /// This endpoint requires the user to have the "ManagerOnly" authorization policy.
        /// </remarks>
        /// <returns>
        /// Returns a list of all games if successful.
        /// If no games are found, returns a NotFound response with an appropriate message.
        /// If an error occurs during processing, returns a StatusCode 500 response with an error message.
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
                    return NotFound(GAME.NOT_FOUND);
                }

                return Ok(games);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = GAME.NOT_FOUND, error = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a paginated list of games based on the provided search and pagination options.
        /// </summary>
        /// <param name="options">The pagination and search options.</param>
        /// <returns>
        /// Returns a paginated list of games if successful.
        /// If no games are found, returns a NotFound response with an appropriate message.
        /// If an error occurs during processing, returns a StatusCode 500 response with an error message.
        /// </returns>
        [HttpGet("paginated")]
        public IActionResult GetAllGamesPaginated([FromQuery] PaginationAndSearchOptionsDto options)
        {
            try
            {
                var games = _gameService.GetAllGamesPaginated(options);

                if (games == null)
                {
                    return NotFound(GAME.NOT_FOUND);
                }
                return Ok(games);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = GAME.NOT_FOUND, error = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a game by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the game.</param>
        /// <returns>
        /// Returns the game's information if found.
        /// If the provided ID is invalid, returns a BadRequest response with an appropriate message.
        /// If no game is found with the specified ID, returns a NotFound response with an appropriate message.
        /// If an error occurs during processing, returns a StatusCode 500 response with an error message.
        /// </returns>
        [HttpGet("{id}")]
        public IActionResult GetGameById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(INVALID_ID);
                }
                var game = _gameService.GetGameById(id);

                if (game == null)
                {
                    return NotFound(GAME.NOT_FOUND);
                }

                return Ok(game);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = GAME.NOT_FOUND, error = ex.Message });
            }
        }

        /// <summary>
        /// Adds a new game to the database.
        /// </summary>
        /// <param name="game">The game information to be added.</param>
        /// <remarks>
        /// This endpoint requires the user to have the "ManagerOnly" authorization policy.
        /// </remarks>
        /// <returns>
        /// Returns a CreatedAtAction response with the URL of the newly created game if successful.
        /// If the provided game data is invalid, returns a BadRequest response with an appropriate message.
        /// If an error occurs during processing, returns a StatusCode 500 response with an error message.
        /// </returns>
        [HttpPost]
        [Authorize(Policy = "ManagerOnly")]
        public IActionResult AddGame([FromBody] Game game)
        {
            try
            {
                if (game == null)
                {
                    return BadRequest( INVALID_DATA);
                }

                _gameService.AddGame(game);

                return CreatedAtAction(nameof(GetGameById), new { id = game.Id }, game);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = GAME.NOT_SAVED, error = ex.Message });
            }
        }

        /// <summary>
        /// Updates an existing game's information in the database.
        /// </summary>
        /// <param name="id">The ID of the game to be updated.</param>
        /// <param name="game">The updated game information.</param>
        /// <remarks>
        /// This endpoint requires the user to have the "ManagerOnly" authorization policy.
        /// </remarks>
        /// <returns>
        /// Returns a status code indicating the result of the update operation.
        /// If successful, returns a success message.
        /// If the provided ID is invalid, returns a BadRequest response.
        /// If the provided game data is invalid, returns a BadRequest response.
        /// If the game with the specified ID is not found, returns a NotFound response.
        /// If an error occurs during the update operation, returns a 500 Internal Server Error response with an error message.
        /// </returns>
        [HttpPut("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public IActionResult UpdateGame(int id, [FromBody] Game game)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest( INVALID_ID );
                }

                if (game == null)
                {
                    return BadRequest( INVALID_DATA );
                }

                _gameService.UpdateGame(id, game);

                return Ok(new { message = GAME.SUCCES_UPDATING });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = GAME.ERROR_UPDATING, error = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a game from the database based on its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the game to be deleted.</param>
        /// <remarks>
        /// This endpoint requires the user to have the "ManagerOnly" authorization policy.
        /// </remarks>
        /// <returns>
        /// Returns a status code indicating the result of the update operation.
        /// If the provided ID is invalid, returns a BadRequest response with an appropriate message.
        /// If the game with the specified ID is not found, returns a NotFound response with an appropriate message.
        /// If an error occurs during processing, returns a StatusCode 500 response with an error message.
        /// </returns>
        [HttpDelete("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public IActionResult DeleteGame(int id)
        {
            try
            {
                if (id <= 0)
                {
                  return BadRequest(INVALID_ID);
                }

                _gameService.DeleteGame(id);
                return Ok(new { message = GAME.SUCCES_DELETING });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = GAME.ERROR_DELETING, error = ex.Message });
            }
        }
    }
}
