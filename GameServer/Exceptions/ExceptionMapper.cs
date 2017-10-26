using GameServer.Services.Repositories;
using Microsoft.Extensions.Logging;
using System.Net;

namespace SandTigerShark.GameServer.Exceptions
{
    public class ExceptionMapper : ExceptionFilterAttribute
    {
        public ExceptionMapper(ILogger logger)
            : base(logger)
        {

        }

        protected override void RegisterExceptionConfigurations()
        {
            RegisterWarning<NotFoundException>(HttpStatusCode.NotFound);
            RegisterWarning<NotAuthorizedException>(HttpStatusCode.Unauthorized);
        }
    }
}