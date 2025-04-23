using ApiBase.Models;

namespace ApiBase.Services.Auth.Interfaces
{
    public interface IAuthServices
    {
        Task<ResponseModel> AuthenticationRequestPost(string path, object body);
    }
}
