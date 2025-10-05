using JinStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JinStore.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id); 
        Task<User?> GetByUsernameAsync(string usernam); 
        Task<User?> GetByEmailAsync(string email); 
        Task<IEnumerable<User>> GetAllAsync(); 
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);
    }
}
