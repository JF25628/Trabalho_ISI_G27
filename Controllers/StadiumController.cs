using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PremierLeagueAPI.Models;
using PremierLeagueAPI.Interfaces;
using PremierLeagueAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PremierLeagueAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StadiumController : ControllerBase
    {
        private readonly IStadiumService _stadiumService;

        public StadiumController(IStadiumService stadiumService)
        {
            _stadiumService = stadiumService;
        }

        // GET: api/stadium
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Stadium>>> GetStadiums()
        {
            var stadiums = await _stadiumService.GetAllStadiumsAsync();
            return Ok(stadiums);
        }

        // GET: api/stadium/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Stadium>> GetStadium(int id)
        {
            var stadium = await _stadiumService.GetStadiumByIdAsync(id);
            if (stadium == null)
            {
                return NotFound();
            }
            return Ok(stadium);
        }

        // POST: api/stadium
        [HttpPost]
        public async Task<ActionResult> CreateStadium([FromBody] Stadium stadium)
        {
            if (stadium == null)
            {
                return BadRequest("Stadium is null.");
            }

            await _stadiumService.CreateStadiumAsync(stadium);
            return CreatedAtAction(nameof(GetStadium), new { id = stadium.StadiumId }, stadium);
        }

        // PUT: api/stadium/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateStadium(int id, [FromBody] Stadium stadium)
        {
            if (id != stadium.StadiumId)
            {
                return BadRequest("Stadium ID mismatch.");
            }

            var existingStadium = await _stadiumService.GetStadiumByIdAsync(id);
            if (existingStadium == null)
            {
                return NotFound();
            }

            await _stadiumService.UpdateStadiumAsync(stadium);
            return NoContent();
        }

        // DELETE: api/stadium/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStadium(int id)
        {
            var stadium = await _stadiumService.GetStadiumByIdAsync(id);
            if (stadium == null)
            {
                return NotFound();
            }

            await _stadiumService.DeleteStadiumAsync(id);
            return NoContent();
        }
    }
}
