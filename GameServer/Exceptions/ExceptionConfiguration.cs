using System;
using System.Net;

namespace SandTigerShark.GameServer.Exceptions
{
    public class ExceptionConfiguration
    {
        public HttpStatusCode StatusCode { get; set; }
        public Action<Exception> Log { get; set; }
        public bool ExportMessage { get; set; }

        public ExceptionConfiguration()
        {
            ExportMessage = true;
        }
    }
}