using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using SandTigerShark.GameServer.Services.Exceptions;
using SandTigerShark.GameServer.Services.Users;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SandTigerShark.GameServer.Utils
{
    public static class HttpExtensions
    {
        private const string userTokenKey = "user-token";

        public static bool TryGetUserToken(
            this HttpContext httpContext,
            out Guid userToken)
        {
            StringValues authorizationValues;
            userToken = Guid.Empty;

            if (httpContext.Request.Headers.TryGetValue(userTokenKey, out authorizationValues)
                && authorizationValues.Count == 1
                && !string.IsNullOrEmpty(authorizationValues.Single()))
            {
                var userTokenString = authorizationValues.Single();

                if (Guid.TryParse(userTokenString, out userToken))
                {
                    return true;
                }
            }
            return false;
        }

        public static async Task<IActionResult> Invoke(
            this HttpContext httpContext,
            IUserRepository userRepository,
            Func<Guid, Task<IActionResult>> onUserAutenticated)
        {
            Guid userToken = Guid.Empty;

            if (httpContext.TryGetUserToken(out userToken))
            {
                var user = await userRepository.GetByToken(userToken);

                if (user != null)
                {
                    return await onUserAutenticated(userToken);
                }
            }
            throw new NotAuthorizedException();
        }
    }
}