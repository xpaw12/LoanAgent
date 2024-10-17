using LoanAgent.Application.Common.Dtos;
using LoanAgent.Application.Common.Exceptions;
using LoanAgent.Application.Common.Interfaces.Repositories;

using MediatR;

namespace LoanAgent.Application.User.Queries.GetUserDetailsById;

public class GetUserDetailsByIdHandler : IRequestHandler<GetUserDetailsByIdQuery, UserDto>
{
    private readonly IUserRepository _userRepository;

    public GetUserDetailsByIdHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<UserDto> Handle(GetUserDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        var userEntity = await _userRepository.FindAsync(x => x.Id == request.UserId, cancellationToken);

        if (userEntity is null)
        {
            throw new NotFoundException($"User with the ID of {request.UserId} wasn't found");
        }

        return userEntity.ToUserDtoMapper();
    }
}
