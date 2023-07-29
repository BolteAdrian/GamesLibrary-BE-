using Microsoft.AspNetCore.Mvc;
using GamesLibrary.DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using GamesLibrary.Services;

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

        [HttpGet]
        public IActionResult GetAllGames()
        {
            var games = _gameService.GetAllGames();
            return Ok(games);
        }

        [HttpGet("search/{searchTerm}")]
        public IActionResult SearchGames(string searchTerm)
        {
            var games = _gameService.SearchGames(searchTerm);
            if (games.Count == 0)
            {
                return NotFound();
            }
            return Ok(games);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public IActionResult GetGameById(int id)
        {
            var game = _gameService.GetGameById(id);
            if (game == null)
            {
                return NotFound();
            }
            return Ok(game);
        }

        [HttpPost]
        [Authorize(Policy = "ManagerOnly")]
        public IActionResult AddGame([FromBody] Game game)
        {
            _gameService.AddGame(game);
            return CreatedAtAction(nameof(GetGameById), new { id = game.Id }, game);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public IActionResult UpdateGame(int id, [FromBody] Game game)
        {
            if (id != game.Id)
            {
                return BadRequest();
            }

            var existingGame = _gameService.GetGameById(id);
            if (existingGame == null)
            {
                return NotFound();
            }

            _gameService.UpdateGame(game);
            return NoContent();
        }

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
