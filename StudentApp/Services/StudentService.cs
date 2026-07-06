using StudentApp.Entities;
using StudentApp.Models;
using StudentApp.Repositories;
namespace StudentApp.Services
{
    public interface IStudentService
    {
        StudentResponseDto Create(StudentCreateDto dto);
        StudentResponseDto GetById(int id);
        IEnumerable<StudentResponseDto> GetAll(int page, int pageSize);
    }
    public class StudentService : IStudentService
    {

        private readonly IStudentDbRepo _studentDbRepo;

        public StudentService(IStudentDbRepo studentDbRepo)
        {
            _studentDbRepo = studentDbRepo;
        }

        public StudentResponseDto Create(StudentCreateDto dto)
        {
            if(_studentDbRepo.StudentExistsByEmail(dto.Email))
                throw new Exception("Student with the same email already exists");
            if (_studentDbRepo.StudentExistsByStudentNumber(dto.StudentNumber))
                throw new Exception("Student with the same student number already exists");
            var student = new Student
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                StudentNumber = dto.StudentNumber
            };

            _studentDbRepo.AddStudent(student);
            _studentDbRepo.SaveChanges();

            return new StudentResponseDto
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                StudentNumber = student.StudentNumber
            };
        }

        public IEnumerable<StudentResponseDto> GetAll(int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100; 


            List<StudentResponseDto> studentsAllList = _studentDbRepo.GetStudentsPage(page,pageSize)
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
            var student = _studentDbRepo.GetStudentById(id);
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
