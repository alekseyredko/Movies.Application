using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Movies.Application.Filters;

namespace Movies.Application.Extensions
{
    public static class FilterExtensions
    {
        public static void AddFilters(this IServiceCollection collection)
        {
            collection.AddScoped<ReviewerFilterAttribute>();
        }
    }
}
