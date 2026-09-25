using Hangfire.Dashboard;

namespace JobApplicationManagement.Api.Infrastructure;

public sealed class LocalDashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var connection = context.GetHttpContext().Connection;
        return connection.RemoteIpAddress is not null &&
               (connection.RemoteIpAddress.Equals(System.Net.IPAddress.Loopback) ||
                connection.RemoteIpAddress.Equals(System.Net.IPAddress.IPv6Loopback));
    }
}
