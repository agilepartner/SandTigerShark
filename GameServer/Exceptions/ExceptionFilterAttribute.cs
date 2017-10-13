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
        private readonly ExceptionConfiguration _defaultExceptionConfiguration;
        private readonly IDictionary<Type, ExceptionConfiguration> _exceptionConfigurations;

        public ExceptionFilterAttribute()
        {
            _defaultExceptionConfiguration = new ExceptionConfiguration { StatusCode = HttpStatusCode.InternalServerError, Log = LogCritical, ExportMessage = false };
            _exceptionConfigurations = new Dictionary<Type, ExceptionConfiguration>();

            RegisterExceptionConfigurations();
        }

        protected abstract void RegisterExceptionConfigurations();

        private void RegisterException<TException>(ExceptionConfiguration configuration)
        {
            _exceptionConfigurations.Add(typeof(TException), configuration);
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
            var exceptionConfiguration = _exceptionConfigurations.ContainsKey(exceptionType) ? _exceptionConfigurations[exceptionType] : _defaultExceptionConfiguration;

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
            //_logger.LogWarning(new EventId(), exception, exception.Message);
        }

        private void LogCritical(Exception exception)
        {
            //_logger.LogCritical(new EventId(), exception, $"An unhandled exception occured () : {exception.Message}");
        }
    }
}