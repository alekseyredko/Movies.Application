﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Movies.Application.Extensions
{
    public static class AutomapperExtensions
    {
        public static void AddAutomapperAndProfile(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddAutoMapper(typeof(ApplicationMappingProfile));
        }
    }
}