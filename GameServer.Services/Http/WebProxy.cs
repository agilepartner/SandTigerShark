using System;
using System.Net;

namespace SandTigerShark.GameServer.Services.Http
{
    public class CustomWebProxy : IWebProxy
    {
        public CustomWebProxy(string proxyUri)
            : this(new Uri(proxyUri))
        {
        }

        public CustomWebProxy(Uri proxyUri)
        {
            this.ProxyUri = proxyUri;
        }

        public Uri ProxyUri { get; set; }

        public ICredentials Credentials { get; set; }

        public Uri GetProxy(Uri destination)
        {
            return this.ProxyUri;
        }

        public bool IsBypassed(Uri host)
        {
            return false; /* Proxy all requests */
        }
    }
}
