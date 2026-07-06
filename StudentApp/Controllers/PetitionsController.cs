using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentApp.Entities;
using StudentApp.Models;
using StudentApp.Services;

namespace StudentApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PetitionsController : ControllerBase
    {
        private readonly IPetitionService _service;

        public PetitionsController(IPetitionService service)
        { _service = service; }

        [Authorize(Roles = "Student,Reviewer")]
        [HttpPost]
        public IActionResult Create(PetitionCreateDto dto)
        { 
            return Ok(_service.Create(dto));
        }

        [HttpPost("{id}/submit")]
        public IActionResult Submit(int id)
        {
            return Ok(_service.Submit(id));
        }

        [Authorize(Roles = "Reviewer")]
        [HttpPost("{id}/review")]
        public IActionResult Review(int id, PatientReviewDto dto)
        { 
            return Ok(_service.Review(id, dto));
        }


        [Authorize(Roles = "Student,Reviewer")]
        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            var student = _service.GetById(id);
            if (student == null) return NotFound();
            if (User.IsInRole("Student"))
            {
                var claimStudentId = User.FindFirst("studentId")?.Value;
                if (claimStudentId != student.StudentId.ToString()) return Forbid();
            }

            return Ok(student);
        }
    }
}
