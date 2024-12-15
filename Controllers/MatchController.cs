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
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        /// <summary>
        /// retorna todas as partidas
        /// </summary>
        /// <returns></returns>
        // GET: api/match
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Match>>> GetMatches()
        {
            var matches = await _matchService.GetAllMatchesAsync();
            return Ok(matches);
        }

        // GET: api/match/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Match>> GetMatch(int id)
        {
            var match = await _matchService.GetMatchByIdAsync(id);
            if (match == null)
            {
                return NotFound();
            }
            return Ok(match);
        }

        // POST: api/match
        [HttpPost]
        public async Task<ActionResult> CreateMatch([FromBody] Match match)
        {
            if (match == null)
            {
                return BadRequest("Match is null.");
            }

            await _matchService.CreateMatchAsync(match);
            return CreatedAtAction(nameof(GetMatch), new { id = match.MatchId }, match);
        }

        // PUT: api/match/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMatch(int id, [FromBody] Match match)
        {
            if (id != match.MatchId)
            {
                return BadRequest("Match ID mismatch.");
            }

            var existingMatch = await _matchService.GetMatchByIdAsync(id);
            if (existingMatch == null)
            {
                return NotFound();
            }

            await _matchService.UpdateMatchAsync(match);
            return NoContent();
        }

        // DELETE: api/match/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMatch(int id)
        {
            var match = await _matchService.GetMatchByIdAsync(id);
            if (match == null)
            {
                return NotFound();
            }

            await _matchService.DeleteMatchAsync(id);
            return NoContent();
        }
    }
}
