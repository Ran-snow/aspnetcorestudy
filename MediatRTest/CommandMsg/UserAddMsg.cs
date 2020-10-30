using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MediatR;

namespace MediatRTest.CommandMsg
{
    public class UserAddMsg : IRequest<int>
    {
        public string Name { get; set; }
    }
}
