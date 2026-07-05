namespace StudentApp.Models
{
    public class StudentResponseDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string StudentNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
