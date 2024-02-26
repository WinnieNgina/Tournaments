using API.DTO;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoachController : ControllerBase
    {
        private readonly ICoachService _coachService;
        public CoachController(ICoachService coachService)
        {
             _coachService = coachService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CoachDTO>>> GetAllCoaches()
        {
            var coaches = await _coachService.GetAllCoachesAsync();
            return Ok(coaches);
        }
    }
}
