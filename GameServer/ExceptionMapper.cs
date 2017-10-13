using GameServer.Repositories;
using Microsoft.Extensions.Logging;
using SandTigerShark.GameServer.Exceptions;
using System.Net;

namespace SandTigerShark.GameServer
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
        }
    }
}