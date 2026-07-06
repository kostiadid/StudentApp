using Microsoft.EntityFrameworkCore;
using StudentApp.Database;
using StudentApp.Entities;

namespace StudentApp.Repositories
{
    public interface IStudentDbRepo {

        // Students
            bool StudentExistsByEmail(string email);
            bool StudentExistsByStudentNumber(string studentNumber);
            void AddStudent(Student student);
            Student? GetStudentById(int id);
            IReadOnlyList<Student> GetStudentsPage(int page, int pageSize);

        // Petitions
            void AddPetition(Petition petition);
            Petition? GetPetitionById(int id);
            IReadOnlyList<Petition> GetPetitions(
                string? status,
                PetitionType? type,
                int? studentId,
                DateTime? dateFrom,
                DateTime? dateTo);

        void SaveChanges();
    }
    public class StudentDbRepo : IStudentDbRepo
    {
        private readonly StudentDbContext _context;

        public StudentDbRepo(StudentDbContext context)
        {
            _context = context;
        }
        public bool StudentExistsByEmail(string email)
            => _context.Students.Any(s => s.Email == email);

        public bool StudentExistsByStudentNumber(string studentNumber)
            => _context.Students.Any(s => s.StudentNumber == studentNumber);

        public void AddStudent(Student student)
            => _context.Students.Add(student);

        public Student? GetStudentById(int id)
            => _context.Students.Find(id);

        public IReadOnlyList<Student> GetStudentsPage(int page, int pageSize)
            => _context.Students
                .AsNoTracking()
                .OrderBy(s => s.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

        public void AddPetition(Petition petition)
            => _context.Petitions.Add(petition);

        public Petition? GetPetitionById(int id)
            => _context.Petitions.Find(id);

        public IReadOnlyList<Petition> GetPetitions(
            string? status,
            PetitionType? type,
            int? studentId,
            DateTime? dateFrom,
            DateTime? dateTo)
        {
            var query = _context.Petitions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(p => p.Status.ToString() == status);

            if (type.HasValue)
                query = query.Where(p => p.PetitionType == type.Value);

            if (studentId.HasValue)
                query = query.Where(p => p.StudentId == studentId.Value);

            if (dateFrom.HasValue)
                query = query.Where(p => p.CreatedAt >= dateFrom.Value);

            if (dateTo.HasValue)
                query = query.Where(p => p.CreatedAt <= dateTo.Value);

            return query.ToList();
        }
        public void SaveChanges()
        { 
            _context.SaveChanges();
        }
    }
}
