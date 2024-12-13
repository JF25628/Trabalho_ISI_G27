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
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        // GET: api/team
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeams()
        {
            var teams = await _teamService.GetAllTeamsAsync();
            return Ok(teams);
        }

        // GET: api/team/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetTeam(int id)
        {
            var team = await _teamService.GetTeamByIdAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            return Ok(team);
        }

        // POST: api/team
        [HttpPost]
        public async Task<ActionResult> CreateTeam([FromBody] Team team)
        {
            if (team == null)
            {
                return BadRequest("Team is null.");
            }

            await _teamService.CreateTeamAsync(team);
            return CreatedAtAction(nameof(GetTeam), new { id = team.TeamId }, team);
        }

        // PUT: api/team/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTeam(int id, [FromBody] Team team)
        {
            if (id != team.TeamId)
            {
                return BadRequest("Team ID mismatch.");
            }

            var existingTeam = await _teamService.GetTeamByIdAsync(id);
            if (existingTeam == null)
            {
                return NotFound();
            }

            await _teamService.UpdateTeamAsync(team);
            return NoContent();
        }

        // DELETE: api/team/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTeam(int id)
        {
            var team = await _teamService.GetTeamByIdAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            await _teamService.DeleteTeamAsync(id);
            return NoContent();
        }
    }
}
