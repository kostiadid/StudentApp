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
        public StudentResponseDto Create(StudentCreateDto dto)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StudentResponseDto> GetAll()
        {
            throw new NotImplementedException();
        }

        public StudentResponseDto GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
