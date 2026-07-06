using AutoMapper;
using StudentApp.Entities;
using StudentApp.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StudentApp.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<StudentCreateDto, Student>();

            CreateMap<Student, StudentResponseDto>()
                .ForMember(d => d.CreatedAt, o => o.MapFrom(s => s.CreatedDate));
        }
    }
}