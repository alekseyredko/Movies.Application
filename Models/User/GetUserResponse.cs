using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Movies.Data.Models;

namespace Movies.Application.Models.User
{
    public class GetUserResponse
    {
        public int UserId { get; set; }

        public string Login { get; set; }

        public ICollection<UserRoles> Roles { get; set; }
    }
}
