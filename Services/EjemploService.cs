using ApiBase.Constant;
using ApiBase.Context;
using ApiBase.Models;
using ApiBase.Models.DB;
using ApiBase.Models.DTOs.Input;
using ApiBase.Models.DTOs.Output;
using ApiBase.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace ApiBase.Services
{
    public class EjemploService : IEjemploService
    {
        private ZamoWebAppContext _db;
        private readonly IMapper _mapper;
        public EjemploService(ZamoWebAppContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ResponseModel> getEstudiantes()
        {
            List<Tbl_test_Estudiante>? dbEstudiantesList = await _db.Tbl_test_Estudiantes.Where(x => x.CodigoEstado == "A").OrderBy(x => x.CodigoEstudiante).ToListAsync();
            List<EjemploDto>? items = _mapper.Map<List<EjemploDto>>(dbEstudiantesList);
            return new ResponseModel(StatusCodes.Status200OK, ReplyMessages.successfulProcess, items);
        }

        public async Task<ResponseModel> getEstudianteById(int id)
        {
            Tbl_test_Estudiante? dbEstudiante = await _db.Tbl_test_Estudiantes.Where(x => x.CodigoEstado == "A" && x.Id == id).FirstOrDefaultAsync();
            if (dbEstudiante == null) return new ResponseModel(StatusCodes.Status404NotFound, ReplyMessages.recordNotFound, "idEstudiante NO encontrado");

            EjemploDto? item = _mapper.Map<EjemploDto>(dbEstudiante);
            return new ResponseModel(StatusCodes.Status200OK, ReplyMessages.successfulProcess, item);
        }

        public async Task<ResponseModel> createEstudiante(EjemploInputDto estudianteInputDto, string user)
        {
            Tbl_test_Estudiante estudiante = _mapper.Map<Tbl_test_Estudiante>(estudianteInputDto);
            estudiante.CodigoEstado = "A";
            estudiante.UsuarioCreador = user;
            estudiante.FechaCreador = DateTime.Now;
            await _db.Tbl_test_Estudiantes.AddAsync(estudiante);
            await _db.SaveChangesAsync();
            EjemploDto? item = _mapper.Map<EjemploDto>(estudiante);
            return new ResponseModel(StatusCodes.Status200OK, ReplyMessages.successfulProcess, item);
        }

        public async Task<ResponseModel> updateEstudiante(int id, EjemploInputDto estudianteInputDto, string user)
        {
            Tbl_test_Estudiante? dbEstudiante = await _db.Tbl_test_Estudiantes.Where(x => x.CodigoEstado == "A" && x.Id == id).FirstOrDefaultAsync();
            if (dbEstudiante == null) return new ResponseModel(StatusCodes.Status404NotFound, ReplyMessages.recordNotFound, "idEstudiante NO encontrado");

            _mapper.Map(estudianteInputDto, dbEstudiante);
            dbEstudiante.UsuarioModifica = user;
            dbEstudiante.FechaModifica = DateTime.Now;
            await _db.SaveChangesAsync();
            EjemploDto? item = _mapper.Map<EjemploDto>(dbEstudiante);
            return new ResponseModel(StatusCodes.Status200OK, ReplyMessages.successfulProcess, item);
        }

        public async Task<ResponseModel> patchEstudiante(int id, JsonPatchDocument<EjemploInputDto> patchDoc, string user)
        {
            Tbl_test_Estudiante? dbEstudiante = await _db.Tbl_test_Estudiantes.Where(x => x.CodigoEstado == "A" && x.Id == id).FirstOrDefaultAsync();
            if (dbEstudiante == null) return new ResponseModel(StatusCodes.Status404NotFound, ReplyMessages.recordNotFound, "idEstudiante NO encontrado");

            EjemploInputDto? estudianteInputDto = _mapper.Map<EjemploInputDto>(dbEstudiante);
            patchDoc.ApplyTo(estudianteInputDto);

            _mapper.Map(estudianteInputDto, dbEstudiante);

            dbEstudiante.UsuarioModifica = user;
            dbEstudiante.FechaModifica = DateTime.Now;
            await _db.SaveChangesAsync();
            EjemploDto? item = _mapper.Map<EjemploDto>(dbEstudiante);
            return new ResponseModel(StatusCodes.Status200OK, ReplyMessages.successfulProcess, item);
        }

        public async Task<ResponseModel> deleteEstudiante(int id, string user)
        {
            Tbl_test_Estudiante? dbEstudiante = await _db.Tbl_test_Estudiantes.Where(x => x.CodigoEstado == "A" && x.Id == id).FirstOrDefaultAsync();
            if (dbEstudiante == null) return new ResponseModel(StatusCodes.Status404NotFound, ReplyMessages.recordNotFound, "idEstudiante NO encontrado");

            dbEstudiante.CodigoEstado = "E";
            dbEstudiante.UsuarioModifica = user;
            dbEstudiante.FechaModifica = DateTime.Now;
            await _db.SaveChangesAsync();

            return new ResponseModel(StatusCodes.Status200OK, ReplyMessages.successfulProcess);
        }
    }
}
