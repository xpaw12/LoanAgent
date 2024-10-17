using LoanAgent.Application.Common.Interfaces.Helpers;

using Microsoft.AspNetCore.Http;

using System.Security.Claims;

namespace LoanAgent.Application.Common.Helpers;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private Guid? _id;

    public CurrentUser(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid Id
    {
        get
        {
            if (_id.HasValue)
                return _id.Value;

            var user = _httpContextAccessor
                .HttpContext?
                .User;

            if (user is null)
                throw new InvalidOperationException("Unauthenticated");

            if (!Guid.TryParse(user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
                    out var userIdGuid))
                throw new InvalidOperationException("UserId is null");

            _id = userIdGuid;

            return _id.Value;
        }
    }

    public bool IsInRole(string commaSeparatedRoles)
    {
        var user = _httpContextAccessor
            .HttpContext?
            .User;

        if (user is null)
            throw new InvalidOperationException("Unauthenticated");

        ArgumentNullException
            .ThrowIfNull(commaSeparatedRoles, nameof(commaSeparatedRoles));

        return commaSeparatedRoles.Split(',').Any(r => user.IsInRole(r));
    }
}
