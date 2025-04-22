using ApiBase.Models;
using ApiBase.Models.DTOs.Input;
using Microsoft.AspNetCore.JsonPatch;

namespace ApiBase.Services.Interfaces
{
    public interface IEjemploService
    {
        Task<ResponseModel> getEstudiantes();
        Task<ResponseModel> getEstudianteById(int id);
        Task<ResponseModel> createEstudiante(EjemploInputDto unidadInputDto, string user);
        Task<ResponseModel> updateEstudiante(int id, EjemploInputDto UnidadInputDto, string user);
        Task<ResponseModel> patchEstudiante(int id, JsonPatchDocument<EjemploInputDto> patchDoc, string user);
        Task<ResponseModel> deleteEstudiante(int id, string user);
    }
}
