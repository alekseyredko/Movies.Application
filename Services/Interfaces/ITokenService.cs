using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Movies.Application.Authentication;
using Movies.Data.Models;

namespace Movies.Application.Services
{
    interface ITokenService
    {
        string GenerateJWTAsync(UserResponse user, AuthConfiguration authConfiguration);
        int GetIdFromToken(HttpContext context);
    }
}
