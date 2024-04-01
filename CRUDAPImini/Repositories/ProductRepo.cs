using AutoMapper;
using CRUDAPImini.Data;
using CRUDAPImini.Models.DTOs;
using CRUDAPImini.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRUDAPImini.Repositories
{
    public class ProductRepo(IMapper mapper, AppDBContext appDBContext) : IProductRepo
    {
        public async Task<Response> Add(AddRequestDTO request)
        {
            appDBContext.Products.Add(mapper.Map<Product>(request));
            await appDBContext.SaveChangesAsync();
            return new Response(true, "Saved");
        }

        //public async Task<List<ResponseDTO>> GetAll() =>
        //    mapper.Map<List<ResponseDTO>>(await appDBContext.Products.ToListAsync());
        public async Task<List<ResponseDTO>> GetAll() =>
            mapper.Map<List<ResponseDTO>>(await appDBContext.Products.ToListAsync());


        public async Task<ResponseDTO> GetById(int id) =>
            mapper.Map<ResponseDTO>(await appDBContext.Products.FindAsync(id));

        public async Task<Response> Update(UpdateRequestDTO request)
        {
            appDBContext.Products.Update(mapper.Map<Product>(request));
            await appDBContext.SaveChangesAsync();
            return new Response(true, "Updated");
        }

        public async Task<Response> Delete(int id)
        {
            appDBContext.Products.Remove(await appDBContext.Products.FindAsync(id));
            await appDBContext.SaveChangesAsync();
            return new Response(true, "Deleted");
        }
    }
}
