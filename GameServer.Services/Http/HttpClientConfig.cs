﻿namespace SandTigerShark.GameServer.Services.Http
{
    public class HttpClientConfig
    {
        public string ProxyUri { get; set; }
        public string DefaultContentType { get; set; }
        public double Timeout { get; set; }
    }
}