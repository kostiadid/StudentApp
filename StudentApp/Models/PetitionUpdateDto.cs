using StudentApp.Entities;

namespace StudentApp.Models
{
    public class PetitionUpdateDto
    {
        public PetitionType PetitionType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
