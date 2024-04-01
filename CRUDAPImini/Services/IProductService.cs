using CRUDAPImini.Models.DTOs;

namespace CRUDAPImini.Services
{
    public interface IProductService
    {
        Task<Response> Add(AddRequestDTO request);
        Task<Response> Update(UpdateRequestDTO request);

        Task<List<ResponseDTO>> GetAll();
        Task<ResponseDTO> GetById(int id);
        Task<Response> Delete(int id);
    }
}
