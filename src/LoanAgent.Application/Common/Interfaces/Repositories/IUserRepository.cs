using LoanAgent.Application.Common.Interfaces.Repositories.Base;
using LoanAgent.Domain.Entities;

namespace LoanAgent.Application.Common.Interfaces.Repositories;

public interface IUserRepository : IRepository<UserEntity>
{
    public Task<bool> DoesUserExist(string userName, string email);
}
