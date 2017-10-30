using Microsoft.Extensions.Logging;
using SandTigerShark.GameServer.Services.Exceptions;
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
            RegisterWarning<GameNotAvailableException>(HttpStatusCode.NotFound);
            RegisterWarning<GameTypeNotAvailableException>(HttpStatusCode.MethodNotAllowed);
            RegisterWarning<InvalidCommandException>(HttpStatusCode.MethodNotAllowed);
            RegisterWarning<EntityAlreadyExistsException>(HttpStatusCode.MethodNotAllowed);
            RegisterWarning<NotAuthorizedException>(HttpStatusCode.Unauthorized, exportMessage: false);
        }
    }
}