using ApiBase.Constant;
using ApiBase.Models;
using ApiBase.Services.Auth.Interfaces;
using ApiBase.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NJsonSchema;
using NJsonSchema.NewtonsoftJson.Generation;

namespace ApiBase.Services.Auth
{
    public class AuthServices : IAuthServices
    {
        private IConfiguration configuration;

        public AuthServices(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<ResponseModel> AuthenticationRequestPost(string path, object body)
        {
            string? authUrl = configuration.GetSection("AuthUrl").Value;
            if (authUrl == null || authUrl == "") return new ResponseModel(StatusCodes.Status404NotFound, ReplyMessages.recordNotFound, "No se ha proporcionado la url de auth");

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("CodigoApiProject", configuration.GetSection("CodigoApiProject").Value);
            var response = await httpClient.PostAsJsonAsync(authUrl + path, body);
            string entry = response.Content.ReadAsStringAsync().Result;

            if (!JsonUtils.IsValidJson(entry)) throw new Exception("No es un JSON válido: " + entry.ToString());

            NewtonsoftJsonSchemaGeneratorSettings jsonSchemaGeneratorSettings = new NewtonsoftJsonSchemaGeneratorSettings();
            jsonSchemaGeneratorSettings.SerializerSettings = new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            JsonSchema schema = JsonSchema.FromType<ResponseModel>(jsonSchemaGeneratorSettings);

            var errors = schema.Validate(entry);
            if (errors.Count > 0) return new ResponseModel((int)response.StatusCode, ReplyMessages.genericAnswer, JsonConvert.DeserializeObject(entry));

            ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(entry)!;
            return responseModel;
        }
    }
}
