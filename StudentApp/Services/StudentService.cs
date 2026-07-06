using AutoMapper;
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
        private readonly IMapper _mapper;

        public StudentService(IStudentDbRepo studentDbRepo, IMapper mapper)
        {
            _studentDbRepo = studentDbRepo;
            _mapper = mapper;
        }

        public StudentResponseDto Create(StudentCreateDto dto)
        {
            if(_studentDbRepo.StudentExistsByEmail(dto.Email))
                throw new Exception("Student with the same email already exists");
            if (_studentDbRepo.StudentExistsByStudentNumber(dto.StudentNumber))
                throw new Exception("Student with the same student number already exists");

            var student = _mapper.Map<Student>(dto);

            _studentDbRepo.AddStudent(student);
            _studentDbRepo.SaveChanges();

            return _mapper.Map<StudentResponseDto>(student);
        }

        public IEnumerable<StudentResponseDto> GetAll(int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var students = _studentDbRepo.GetStudentsPage(page, pageSize);
            return _mapper.Map<List<StudentResponseDto>>(students);
        }

        public StudentResponseDto GetById(int id)
        {
            var student = _studentDbRepo.GetStudentById(id);
            if (student == null) throw new Exception("Student not found");

            return _mapper.Map<StudentResponseDto>(student);
        }
    }
}
