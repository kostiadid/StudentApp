using Microsoft.EntityFrameworkCore;
using StudentApp.Database;
using StudentApp.Entities;
using StudentApp.Models;

namespace StudentApp.Services
{
    public interface IStudentService
    {
        StudentResponseDto Create(StudentCreateDto dto);
        StudentResponseDto GetById(int id);
        IEnumerable<StudentResponseDto> GetAll();
    }
    public class StudentService : IStudentService
    {
        private readonly StudentDbContext _context;
        public StudentService(StudentDbContext context)
        {
            _context = context;
        }
        public StudentResponseDto Create(StudentCreateDto dto)
        {
            if(_context.Students.Any(n=>n.Email == dto.Email))
                throw new Exception("Student with the same email already exists");
            if (_context.Students.Any(n => n.StudentNumber == dto.StudentNumber))
                throw new Exception("Student with the same student number already exists");
            var student = new Student
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                StudentNumber = dto.StudentNumber
            };

            _context.Students.Add(student);
            _context.SaveChanges();

            return new StudentResponseDto
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                StudentNumber = student.StudentNumber
            };
        }

        public IEnumerable<StudentResponseDto> GetAll()
        {
            List<StudentResponseDto> studentsAllList = _context.Students
                .AsNoTracking()
                .Select(student => new StudentResponseDto
                {
                    Id = student.Id,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Email = student.Email,
                    StudentNumber = student.StudentNumber
                })
                .ToList();
            return studentsAllList;
        }

        public StudentResponseDto GetById(int id)
        {
            var student = _context.Students.Find(id);
            if (student == null) throw new Exception("Student not found");

            return new StudentResponseDto
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                StudentNumber = student.StudentNumber
            };
        }
    }
}
