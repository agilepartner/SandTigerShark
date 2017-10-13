using GameServer.Repositories;
using SandTigerShark.GameServer.Exceptions;
using System.Net;

namespace SandTigerShark.GameServer
{
    public class ExceptionMapper : ExceptionFilterAttribute
    {
        protected override void RegisterExceptionConfigurations()
        {
            RegisterWarning<NotFoundException>(HttpStatusCode.NotFound);
        }
    }
}