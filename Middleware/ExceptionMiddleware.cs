using Integracion_Datos_Banner_Koha.Constant;
using Integracion_Datos_Banner_Koha.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog.Context;
using System.Net;
using System.Net.Mime;

namespace Integracion_Datos_Banner_Koha.Middleware
{
    public class ExceptionMiddleware
    {
        public RequestDelegate requestDelegate;
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly List<string> whiteRoutes = new() { "/" };
        private readonly List<string> AllowjsonMediaTypes = new() { "application/x-www-form-urlencoded; charset=UTF-8", "application/json", "application/json; charset=utf-8", "multipart/form-data" };

        public ExceptionMiddleware
        (RequestDelegate requestDelegate, ILogger<ExceptionMiddleware> logger)
        {
            this.requestDelegate = requestDelegate;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                //VALIDAR MEDIA TYPE
                if (context.Request.Method == "PUT" || context.Request.Method == "POST" || context.Request.Method == "PATCH")
                {
                    string mediaTypeRequest = context.Request.ContentType ?? "No especificada";
                    string mediaType = GetMediaType(mediaTypeRequest);
                    bool isValidJsonMediaType = AllowjsonMediaTypes.Any(x => string.Equals(mediaTypeRequest, x, StringComparison.OrdinalIgnoreCase));
                    if (!isValidJsonMediaType)
                    {
                        context.Response.Headers.Append("Content-Type", "application/json");
                        context.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(
                            new ResponseModel(StatusCodes.Status415UnsupportedMediaType, ReplyMessages.unsupportedMediaType, $"Media Type no soportada: {mediaTypeRequest}")
                            ));
                        return;
                    }
                }

                await requestDelegate(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private string GetMediaType(string mediaType)
        {
            if (mediaType.Contains("multipart/form-data")) return mediaType.Split(';').First().Trim();
            return mediaType;
        }

        private Task HandleException(HttpContext context, Exception ex)
        {
            LogContext.PushProperty("IpUser", context.Connection.RemoteIpAddress);
            logger.LogError(ex.ToString());
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            ResponseModel response = new ResponseModel(context.Response.StatusCode, ReplyMessages.errorProcess, context.TraceIdentifier);
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var errorMessageObject = JsonConvert.SerializeObject(response, serializerSettings);
            return context.Response.WriteAsync(errorMessageObject);
        }
    }
}
