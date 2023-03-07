using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EF.Data;

namespace EF.Interfaces
{
    internal interface IUserRepository : IDisposable
    {
        Task<IEnumerable<UserInfo>> GetAllUserAsync();
        Task<UserInfo> GetUserByIdAsync(int id);
        Task<UserInfo> GetUserWithDetailsAsync(int id);
        void CreateUser(UserInfo user);
        void UpdateUser(UserInfo user);
        void DeleteUser(UserInfo user);
    }
}
