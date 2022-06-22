using System.Security.Claims;

namespace PerfumeShop.Helpers;

public class CurrentUser
{
    private readonly IHttpContextAccessor _http;

    public CurrentUser(IHttpContextAccessor http)
    {
        _http = http;
    }

    public string? Id => _http.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}