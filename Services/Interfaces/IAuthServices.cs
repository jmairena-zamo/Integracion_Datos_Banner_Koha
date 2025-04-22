using ApiBase.Models;

namespace ApiBase.Services.Interfaces
{
    public interface IAuthServices
    {
        Task<ResponseModel> AuthenticationRequestPost(string path, object body);
    }
}
