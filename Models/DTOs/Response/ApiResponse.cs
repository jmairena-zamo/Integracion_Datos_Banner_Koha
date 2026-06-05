//Título del Programa: BannerKoha
//Autor: José Anibal Mairena Diaz
//Fecha de Creación:  19 / 05 / 2026
//Lenguaje: C#
//Versión: v1.0
/*Propósito: En este apartado se guarda el estatus del envío de correos
  con los datos de los perfiles creados en Koha.*/
//Responsable de Mantenimiento: José Anibal Mairena Diaz

namespace Integracion_Datos_Banner_Koha.Models.DTOs.Response
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; }
        public bool IsSuccess { get; }
        public T? Data { get; }
        public object? Error { get; }

        private ApiResponse(int statusCode, bool isSuccess, T? data, object? error)
        {
            StatusCode = statusCode;
            IsSuccess = isSuccess;
            Data = data;
            Error = error;
        }

        public static ApiResponse<T> Success(int statusCode, T data) =>
            new(statusCode, true, data, null);

        public static ApiResponse<T> Failure(int statusCode, object? error) =>
            new(statusCode, false, default, error);
    }
}

