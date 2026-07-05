using Microsoft.AspNetCore.Mvc;
using StudentApp.Database;
using StudentApp.Entities;

namespace StudentApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetitionsController : ControllerBase
    {
        private readonly StudentDbContext _context;

        public PetitionsController(StudentDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CreatePetition(Petition petition)
        {
            petition.Status = PetitionStatus.Draft;
            petition.CreatedAt = DateTime.UtcNow;
            _context.Petitions.Add(petition);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetPetitionById), new { id = petition.Id }, petition);
        }

        [HttpGet("{id}")]
        public IActionResult GetPetitionById(int id)
        {
            var petition = _context.Petitions.Find(id);
            if (petition == null) return NotFound();
            return Ok(petition);
        }

        [HttpGet]
        public IActionResult GetAllPetitions()
        {
            var petitions = _context.Petitions.ToList();
            return Ok(petitions);
        }
    }

}
