using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

namespace MediatRTest.CommandMsg
{
    public class UserAddSuccessMsgSMSHandler : INotificationHandler<UserAddSuccessMsg>
    {
        private readonly ILogger<UserAddSuccessMsgSMSHandler> _logger;

        public UserAddSuccessMsgSMSHandler(ILogger<UserAddSuccessMsgSMSHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(UserAddSuccessMsg notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("UserAddSuccessMsgSMSHandler->{0}", notification.Message);

            return Task.CompletedTask;
        }
    }
}
