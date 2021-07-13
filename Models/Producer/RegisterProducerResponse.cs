using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Application.Models.Producer
{
    public class RegisterProducerResponse: ProducerResponse
    {
        public string Token { get; set; }
    }
}
