using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Net;

namespace SandTigerShark.GameServer
{
    public class AddTokenHeaderOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<IParameter>();
            }

            if (operation.Responses.ContainsKey(((int)HttpStatusCode.Unauthorized).ToString()))
            {
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "user-token",
                    In = "header",
                    Type = "string",
                    Required = true
                });
            }
        }
    }
}