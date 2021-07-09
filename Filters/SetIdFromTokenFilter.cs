using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Movies.Application.Models.User;
using Movies.Application.Services;
using Movies.Data.Models;

namespace Movies.Application.Filters
{
    public class SetIdFromTokenFilter: IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue("userRequest", out object val))
            {
                switch (val)
                {
                    case UpdateUserRequest updateUser:
                        updateUser.UserId = TokenHelper.GetIdFromToken(context.HttpContext);
                        break;

                    case Reviewer reviewer:
                        reviewer.ReviewerId = TokenHelper.GetIdFromToken(context.HttpContext);
                        break;
                }
            }

            await next();
        }
    }
}
