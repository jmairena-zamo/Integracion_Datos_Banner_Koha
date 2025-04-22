using ApiBase.Models.DB;
using ApiBase.Models.DTOs.Input;
using ApiBase.Models.DTOs.Output;
using AutoMapper;

namespace ApiBase.Profiles
{
    public class EjemploProfile: Profile
    {
        public EjemploProfile()
        {
            CreateMap<EjemploInputDto, Tbl_test_Estudiante>().ReverseMap();
            CreateMap<Tbl_test_Estudiante, EjemploDto>();
        }
    }
}
