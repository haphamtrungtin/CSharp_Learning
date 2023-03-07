using EF.Data;
using EF.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EF.Repository
{
    public class UserRepository : IUserRepository
    {
        public void CreateUser(UserInfo user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(UserInfo user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserInfo>> GetAllUserAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserInfo> GetUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserInfo> GetUserWithDetailsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(UserInfo user)
        {
            throw new NotImplementedException();
        }
    }
}
