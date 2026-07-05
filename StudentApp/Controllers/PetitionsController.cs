using Microsoft.AspNetCore.Mvc;
using StudentApp.Database;
using StudentApp.Entities;
using StudentApp.Models;
using StudentApp.Services;

namespace StudentApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetitionsController : ControllerBase
    {
        private readonly IPetitionService _service;

        public PetitionsController(IPetitionService service)
        { _service = service;}

        [HttpPost]
        public IActionResult Create(PetitionCreateDto dto)
             => Ok(_service.Create(dto));

        [HttpPost("{id}/submit")]
        public IActionResult Submit(int id)
            => Ok(_service.Submit(id));

        [HttpPost("{id}/review")]
        public IActionResult Review(int id, PatientReviewDto dto)
            => Ok(_service.Review(id, dto));
    }
}
