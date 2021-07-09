using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Movies.Application.Services;

namespace Movies.Application.Middleware
{
    public class AddIdFromTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public AddIdFromTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Form.ContainsKey("Authentication"))
            {
                var token = TokenHelper.GetIdFromToken(context);

                context.GetRouteData();
            }

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }
}

