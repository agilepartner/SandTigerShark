using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using SandTigerShark.GameServer.Exceptions;
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
            out string userToken)
        {
            StringValues authorizationValues;
            userToken = null;

            if (httpContext.Request.Headers.TryGetValue(userTokenKey, out authorizationValues)
                && authorizationValues.Count == 1
                && !string.IsNullOrEmpty(authorizationValues.Single()))
            {
                userToken = authorizationValues.Single();
            }
            return !string.IsNullOrEmpty(userToken);
        }

        public static Task<IActionResult> Call(
            this HttpContext httpContext,
            Func<string, Task<IActionResult>> onTokenSuccessfullyGot)
        {
            string userToken = null;

            if (httpContext.TryGetUserToken(out userToken))
            {
                return onTokenSuccessfullyGot(userToken);
            }
            throw new NotAuthorizedException();
        }
    }
}