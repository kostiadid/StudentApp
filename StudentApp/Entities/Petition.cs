namespace StudentApp.Entities
{
    public class Petition
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public PetitionType PetitionType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public PetitionStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? ReviewedBy { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public string? ReviewComment { get; set; }
    }
} 
