using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

namespace MediatRTest.CommandMsg
{
    public class UserAddMsgHandler : IRequestHandler<UserAddMsg, int>
    {
        private readonly ILogger<UserAddMsgHandler> _logger;

        public UserAddMsgHandler(ILogger<UserAddMsgHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<int> Handle(UserAddMsg request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("UserAddMsgHandler->{0}", request.Name);

            return Task.FromResult(666);
        }
    }
}
