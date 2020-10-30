using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MediatR;

namespace MediatRTest.CommandMsg
{
    public class UserAddSuccessMsg : INotification
    {
        public string Message { get; set; }
    }
}
