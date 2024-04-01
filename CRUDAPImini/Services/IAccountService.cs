using CRUDAPImini.Models.DTOs;

namespace CRUDAPImini.Services
{
    public interface IAccountService
    {
        Task<Response> Register(RegisterDTO registerDTO);
        Task<LoginResponse> Login(LoginDTO loginDTO);
    }
}
