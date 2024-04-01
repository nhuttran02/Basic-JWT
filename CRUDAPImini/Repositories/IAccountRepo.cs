using CRUDAPImini.Models.DTOs;

namespace CRUDAPImini.Repositories
{
    public interface IAccountRepo
    {
        Task<Response> Register(RegisterDTO registerDTO);
        Task<LoginResponse> Login(LoginDTO loginDTO);
    }
}
