using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Movies.Application.Middleware
{
    public class AddNewClaimsToTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public AddNewClaimsToTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);
        }
    }
}
