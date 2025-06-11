using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Notification.Service;

public class LoginNotificationService
{
    private readonly ILogger<LoginNotificationService> _logger;

    public LoginNotificationService(ILogger<LoginNotificationService> logger)
    {
        _logger = logger;
    }

    [Function(nameof(LoginNotificationService))]
    public void Run([QueueTrigger("myqueue-items", Connection = "")] QueueMessage message)
    {
        _logger.LogInformation("C# Queue trigger function processed: {messageText}", message.MessageText);
    }
}