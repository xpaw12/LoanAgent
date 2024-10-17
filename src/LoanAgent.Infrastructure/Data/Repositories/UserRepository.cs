using LoanAgent.Application.Common.Interfaces.Repositories;
using LoanAgent.Domain.Entities;
using LoanAgent.Infrastructure.Data.Repositories.Base;

using Microsoft.EntityFrameworkCore;

namespace LoanAgent.Infrastructure.Data.Repositories;

public class UserRepository
: Repository<UserEntity>, IUserRepository
{
    public UserRepository(
        AppDbContext context)
        : base(context)
    {
    }

    public Task<bool> DoesUserExist(string userName, string email)
    {
        return DbSet.AnyAsync(u => u.Username == userName || u.Email == email);
    }
}
