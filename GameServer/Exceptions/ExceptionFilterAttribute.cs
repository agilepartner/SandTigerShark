using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;

namespace SandTigerShark.GameServer.Exceptions
{
    public abstract class ExceptionFilterAttribute : Microsoft.AspNetCore.Mvc.Filters.ExceptionFilterAttribute
    {
        private readonly ILogger logger;
        private readonly ExceptionConfiguration defaultExceptionConfiguration;
        private readonly IDictionary<Type, ExceptionConfiguration> exceptionConfigurations;

        public ExceptionFilterAttribute(ILogger logger)
        {
            this.logger = logger;
            defaultExceptionConfiguration = new ExceptionConfiguration { StatusCode = HttpStatusCode.InternalServerError, Log = LogCritical, ExportMessage = false };
            exceptionConfigurations = new Dictionary<Type, ExceptionConfiguration>();

            RegisterExceptionConfigurations();
        }

        protected abstract void RegisterExceptionConfigurations();

        private void RegisterException<TException>(ExceptionConfiguration configuration)
        {
            exceptionConfigurations.Add(typeof(TException), configuration);
        }

        protected void RegisterWarning<TException>(HttpStatusCode statusCode, bool exportMessage = true)
        {
            RegisterException<TException>(new ExceptionConfiguration
            {
                Log = LogWarning,
                StatusCode = statusCode,
                ExportMessage = exportMessage
            });
        }

        protected void RegisterCritical<TException>(HttpStatusCode statusCode, bool exportMessage = true)
        {
            RegisterException<TException>(new ExceptionConfiguration
            {
                Log = LogCritical,
                StatusCode = statusCode,
                ExportMessage = exportMessage
            });
        }

        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var exceptionType = exception.GetType();
            var exceptionConfiguration = exceptionConfigurations.ContainsKey(exceptionType) ? exceptionConfigurations[exceptionType] : defaultExceptionConfiguration;

            exceptionConfiguration.Log(exception);

            context.ExceptionHandled = true;
            context.HttpContext.Response.StatusCode = (int)exceptionConfiguration.StatusCode;

            if (exceptionConfiguration.ExportMessage)
            {
                context.HttpContext.Response.WriteAsync(exception.Message);
            }
        }

        private void LogWarning(Exception exception)
        {
            logger.LogWarning(new EventId(), exception, exception.Message);
        }

        private void LogCritical(Exception exception)
        {
            logger.LogCritical(new EventId(), exception, $"An unhandled exception occured () : {exception.Message}");
        }
    }
}