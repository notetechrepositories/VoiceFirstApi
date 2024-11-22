using System.Net;
using System.Text;

namespace VoiceFirstApi.Service
{
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private const string Username = "Admin";
        private const string Password = "Admin1234";

        public BasicAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                string authHeader = context.Request.Headers["Authorization"];
                if (authHeader == null || !IsAuthorized(authHeader))
                {
                    context.Response.Headers["WWW-Authenticate"] = "Basic";
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return;
                }
            }

            await _next(context);
        }

        private bool IsAuthorized(string authHeader)
        {
            var encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
            var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
            var parts = decodedCredentials.Split(':');
            return parts[0] == Username && parts[1] == Password;
        }
    }
}
