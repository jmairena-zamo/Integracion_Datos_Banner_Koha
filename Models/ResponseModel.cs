namespace Integracion_Datos_Banner_Koha.Models
{
    public class ResponseModel
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public object? Response { get; set; }

        public ResponseModel(int status, string message, object? response = null)
        {
            Status = status;
            Message = message;
            Response = response;

        }
    }
}
