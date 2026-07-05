using StudentApp.Entities;

namespace StudentApp.Models
{
    public class PetitionCreateDto
    {
        public int StudentId { get; set; }
        public PetitionType PetitionType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

}
