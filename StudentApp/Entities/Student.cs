namespace StudentApp.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName{ get; set; }
        public string LastName{ get; set; } 
        public string Email { get; set; } 
        public string StudentNumber { get; set; } 
        public DateTime CreatedDate { get; set; }
        public ICollection<Petition> Petitions { get; set; } 

    }
}
