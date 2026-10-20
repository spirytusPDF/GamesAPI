using GameShopApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        public static List<Game> games = new List<Game>() {
            new Game() {Id= 1, Name="Tekken6", Description="aa", Priority=3, IsCompleted=true },
            new Game() {Id= 2, Name="SilentHill", Description="aa", Priority=5, IsCompleted=false }
        };

        [HttpGet]
        public IActionResult Get()
        {
            if (games.Any())
                return Ok(games);
            else return NotFound();
        }

        [HttpGet("{id:int:min(1)}")]
        public IActionResult GetOne(int id)
        {
            var game = games.FirstOrDefault(g => g.Id == id);
            if (game != null)
                return Ok(game);
            else
                return NotFound();
        }

        [HttpGet("/count")]
        public ActionResult<int> GetCount()
        {
            if (games.Count <= 0) { return NotFound(); }
            return StatusCode(200, games.Count);
        }

        //homework
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("{id:int:min(1)}")]
        public ActionResult NewGamePut([FromBody] Game game, [FromRoute] int id)
        {
            if (games.Contains(games.FirstOrDefault(g => g.Name == game.Name)))
            {
                return StatusCode(409, "There's a game with the same name");
            }
            var gameToChange = games.FirstOrDefault(g => g.Id == id);
            if (gameToChange != null)
            {
                int index = games.IndexOf(gameToChange);
                games[index] = (Game)game.Clone();
                return Ok(games);
            }
            else
            {
                games.Add(game);
                return Ok(games);
            }

        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPost("{id:int:min(1)}/complete")]
        public ActionResult GamePassed([FromRoute] int id)
        {
            var gamePass = games.FirstOrDefault(g => g.Id == id);
            int index = games.IndexOf(gamePass);
            if (gamePass != null)
            {
                if (gamePass.IsCompleted)
                    return StatusCode(409, "The game is already passed");
                else games[index].IsCompleted = true;
                return StatusCode(204);
            }
            else return NotFound();
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpDelete("{id:int:min(1)}")]
        public ActionResult GameDelete([FromRoute] int id)
        {
            var gamePass = games.FirstOrDefault(g => g.Id == id);
            if (gamePass == null) return NotFound();
            if (gamePass.Priority == 5 && !gamePass.IsCompleted) 
                return StatusCode(409, "The priority is too high and the game is still not passed");

            games.Remove(gamePass);
            return Ok(gamePass);


        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("completed")]
        public ActionResult GameDeleteCompleted()
        {
            int before = games.Count;
            games.RemoveAll(g => g.IsCompleted);
            int current = before-games.Count;
            return Ok(current);
        }
    }
}
