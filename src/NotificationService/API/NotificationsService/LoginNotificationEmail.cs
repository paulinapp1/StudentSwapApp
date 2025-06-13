﻿using System;
using System.Net.Mail;
using System.Net;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace NotificationsService;

public class LoginNotificationEmail
{
    private readonly ILogger<LoginNotificationEmail> _logger;

    public LoginNotificationEmail(ILogger<LoginNotificationEmail> logger)
    {
        _logger = logger;
    }

    [Function("KafkaTriggerFunction")]
    public async Task Run(
        [KafkaTrigger(
                "localhost:9092",
                "after-login-email-topic",
                ConsumerGroup = "function-consumer-group")]
            KafkaMessage message)
    {
        _logger.LogInformation($"Odebrano wiadomoœæ z Kafki: {message.ToString()}");
        await SendEmailAsync("W³asnie sie zalogowales", message.Value);
    }

    static async Task SendEmailAsync(string message, string toEmail)
    {
        try
        {
            string smtpHost = Environment.GetEnvironmentVariable("smtpHost");
            int smtpPort = Int32.Parse(Environment.GetEnvironmentVariable("smtpPort"));
            using (var client = new SmtpClient(smtpHost, smtpPort))
            {
                client.EnableSsl = true;
                string smtpUsername = Environment.GetEnvironmentVariable("smtpUsername");
                string smtpPassword = Environment.GetEnvironmentVariable("smtpPassword");
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("paulina@test.com"),
                    Subject = "Wiadomoœæ z Kafki",
                    Body = message,
                    IsBodyHtml = false
                };
                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);
                Console.WriteLine($"E-mail wys³any do {toEmail} z wiadomoœci¹: {message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"B³¹d podczas wysy³ania e-maila: {ex.Message}");
        }
    }
}