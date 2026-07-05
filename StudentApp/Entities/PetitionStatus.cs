namespace StudentApp.Entities
{
    public enum PetitionStatus
    {
        Draft,   // we will have action  -> Submit
        Submitted,  // automatic  not sure ->   UnderReview  
        UnderReview,   //  comment  mandatory -> true / false ..
        Approved,
        Rejected
    }
}
