using API.DTO;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ModelsLibrary.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        private readonly ITournamentService _tournamentService;
        public TournamentController(ITournamentService tournamentService)
        {
            _tournamentService = tournamentService;

        }
        [HttpPost("CreateTournament")]
        // [Authorize(Roles = "Organizer")]
        public async Task<IActionResult> CreateTournament([FromBody] CreateTournamentDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var tournamentId = await _tournamentService.CreateTournamentAsync(model);
            if (tournamentId != null)
            {
                return Ok(new { Message = "Tournament created successfully", TournamentId = tournamentId });
            }
            else
            {
                return BadRequest(new { Message = "Failed to create tournament" });
            }
        }
        [HttpPut("UpdateTournamentStatusByName/{name}")]
        public async Task<IActionResult> UpdateTournamentStatusByName(string name, [FromBody] TournamentStatus newStatus)
        {
            var result = await _tournamentService.UpdateTournamentStatusByNameAsync(name, newStatus);

            if (!result)
            {
                return NotFound("Tournament not found.");
            }

            return Ok("Tournament status updated successfully.");
        }
        [HttpDelete("Delete Tournament/{name}")]
        public async Task<IActionResult> DeleteTournament(string name)
        {
            var result = await _tournamentService.DeleteTournamentByNameAsync(name);
            if (!result)
            {
                return NotFound("Tournament not found.");
            }

            return Ok("Tournament status deleted successfully.");
        }
        [HttpGet("GetTournamentByName/{name}")]
        public async Task<IActionResult> GetTournamentByName(string name)
        {
            var tournamentDto = await _tournamentService.GetTournamentByNameAsync(name);

            if (tournamentDto == null)
            {
                return NotFound("Tournament not found.");
            }

            return Ok(tournamentDto);
        }

    }
}
