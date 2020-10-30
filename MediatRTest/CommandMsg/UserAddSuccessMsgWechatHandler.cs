using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

namespace MediatRTest.CommandMsg
{
    public class UserAddSuccessMsgWechatHandler : INotificationHandler<UserAddSuccessMsg>
    {
        private readonly ILogger<UserAddSuccessMsgWechatHandler> _logger;

        public UserAddSuccessMsgWechatHandler(ILogger<UserAddSuccessMsgWechatHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(UserAddSuccessMsg notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("UserAddSuccessMsgWechatHandler->{0}", notification.Message);

            return Task.CompletedTask;
        }
    }
}
