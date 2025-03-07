using System;

namespace BE.Interface;

public interface IEmailSender
{
        Task SendEmailAsync(string to, string subject, string body);

}
