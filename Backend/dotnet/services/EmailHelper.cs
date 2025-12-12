using System.Net;
using System.Net.Mail;
using dotnet.models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;

public class EmailHelper
{
    private readonly EmailSettings _emailSettings;
     public EmailHelper(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendOtpEmailAsync(string toEmail, string otp)
    {
        using var smtp = new System.Net.Mail.SmtpClient
        {
            Host = _emailSettings.Host,
            Port = _emailSettings.Port,
            EnableSsl = _emailSettings.EnableSSL,
            Credentials = new NetworkCredential(_emailSettings.FromEmail, _emailSettings.Password)
        };
        var mail = new MailMessage
        {
            From = new MailAddress(_emailSettings.FromEmail, _emailSettings.DisplayName),
            Subject = "Your OTP Code",
            Body = $"Your OTP is: {otp}",
            IsBodyHtml = false
        };

        mail.To.Add(toEmail);

        await smtp.SendMailAsync(mail);
    }
}
