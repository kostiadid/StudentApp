using Microsoft.AspNetCore.Mvc;
using StudentApp.Models;
using StudentApp.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace StudentApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        { _studentService = studentService;}

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateStudent(StudentCreateDto student)
        {
            return Ok(_studentService.Create(student));
        }


        [Authorize(Roles = "Student,Reviewer")]
        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            if (User.IsInRole("Student"))
            {
                var claimStudentId = User.FindFirst("studentId")?.Value;
                if (claimStudentId != id.ToString())
                    return Forbid();
            }
            var student = _studentService.GetById(id);
            if (student == null) return NotFound();
            return Ok(student);
        }

        [Authorize(Roles = "Reviewer")]
        [HttpGet]
        public IActionResult GetAllStudents([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var students = _studentService.GetAll(page, pageSize);
            return Ok(students);
        }

    }
}

