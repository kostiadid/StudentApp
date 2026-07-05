using StudentApp.Entities;
using Microsoft.AspNetCore.Mvc;
using StudentApp.Database;
using StudentApp.Models;
namespace StudentApp.Services
{
    public interface IPetitionService
    {
        Petition Create(PetitionCreateDto dto);
        Petition GetById(int id);
        IEnumerable<Petition> GetAll(string? status, PetitionType? type, int? studentId, DateTime? dateFrom, DateTime? dateTo);
        Petition Submit(int id);
        Petition Review(int id, PatientReviewDto dto);
        Petition Update(int id, Petition updated);
    }

    public class PetitionService : IPetitionService
    {
        private readonly StudentDbContext _context;
        public PetitionService(StudentDbContext context)
        {
            _context = context;
        }
        public Petition Create(PetitionCreateDto dto)
        {
            var petition = new Petition
            {
                StudentId = dto.StudentId,
                PetitionType = dto.PetitionType,
                Title = dto.Title,
                Description = dto.Description,
                Status = PetitionStatus.Draft,
                CreatedAt = DateTime.UtcNow
            };
            _context.Petitions.Add(petition);
            _context.SaveChanges();
            return petition;
        }
        public Petition GetById(int id)
        {
            return _context.Petitions.Find(id);
        }
        public IEnumerable<Petition> GetAll(string? status, PetitionType? type, int? studentId, DateTime? dateFrom, DateTime? dateTo)
        {
            var query = _context.Petitions.AsQueryable();
            if (!string.IsNullOrEmpty(status))
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
        public Petition Submit(int id)
        {
            var petition = _context.Petitions.Find(id);
            if (petition == null)
                throw new Exception("Petition not found");

            if (petition.Status != PetitionStatus.Draft)
                throw new Exception("Submit allowed only for Draft");

            petition.Status = PetitionStatus.Submitted;
            petition.UpdatedAt = DateTime.UtcNow;

            petition.Status = PetitionStatus.UnderReview;

            _context.SaveChanges();
            return petition;
        }
        public Petition Review(int id, PatientReviewDto dto)
        {
            var petition = _context.Petitions.Find(id);
            if (petition == null)
                throw new Exception("Petition not found");

            if (petition.Status != PetitionStatus.UnderReview)
                throw new Exception("Review allowed only for UnderReview");

            if (string.IsNullOrWhiteSpace(dto.Comment))
                throw new Exception("Review comment is required");

            petition.Status = dto.Approved ? PetitionStatus.Approved : PetitionStatus.Rejected;
            petition.ReviewComment = dto.Comment;
            petition.ReviewedAt = DateTime.UtcNow;

            _context.SaveChanges();
            return petition;
        } 

        public Petition Update(int id, Petition updated)
        {
            var petition = _context.Petitions.Find(id);
            if (petition == null) 
                throw new Exception("Petition not found");
            if (petition.Status != PetitionStatus.Draft)
                throw new Exception("Update allowed only for Draft");

            var hasChanges =
                petition.Title != updated.Title ||
                petition.Description != updated.Description ||
                petition.PetitionType != updated.PetitionType;
            if (!hasChanges)
                return petition;

            petition.UpdatedAt = DateTime.UtcNow;
            petition.Title = updated.Title;
            petition.Description = updated.Description;
            petition.PetitionType = updated.PetitionType;
            _context.SaveChanges();
            return petition;
        }   
    }
}
