using Microsoft.AspNetCore.Mvc;
using StudentApp.Database;
using StudentApp.Entities;
using StudentApp.Models;
using StudentApp.Services;

namespace StudentApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        { _studentService = studentService;}

        [HttpPost]
        public IActionResult CreateStudent(StudentCreateDto student)
        {
            //_studentService.Create(student);
            return Ok(_studentService.Create(student));
        }

        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            var student = _studentService.GetById(id);
            if (student == null) return NotFound();
            return Ok(student);
        }

        [HttpGet]
        public IActionResult GetAllStudents()
        {
            var students = _studentService.GetAll();
            return Ok(students);
        }

    }
}

