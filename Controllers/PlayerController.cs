using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PremierLeagueAPI.Models;
using PremierLeagueAPI.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PremierLeagueAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        // GET: api/player
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            var players = await _playerService.GetAllPlayersAsync();
            return Ok(players);
        }

        // GET: api/player/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Player>> GetPlayer(int id)
        {
            var player = await _playerService.GetPlayerByIdAsync(id);
            if (player == null)
            {
                return NotFound();
            }
            return Ok(player);
        }

        // POST: api/player
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreatePlayer([FromBody] Player player)
        {
            if (player == null)
            {
                return BadRequest("Player is null.");
            }

            await _playerService.CreatePlayerAsync(player);
            return CreatedAtAction(nameof(GetPlayer), new { id = player.PlayerId }, player);
        }

        // PUT: api/player/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> UpdatePlayer(int id, [FromBody] Player player)
        {
            if (id != player.PlayerId)
            {
                return BadRequest("Player ID mismatch.");
            }

            var existingPlayer = await _playerService.GetPlayerByIdAsync(id);
            if (existingPlayer == null)
            {
                return NotFound();
            }

            await _playerService.UpdatePlayerAsync(player);
            return NoContent();
        }

        // DELETE: api/player/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeletePlayer(int id)
        {
            var player = await _playerService.GetPlayerByIdAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            await _playerService.DeletePlayerAsync(id);
            return NoContent();
        }
    }
}
