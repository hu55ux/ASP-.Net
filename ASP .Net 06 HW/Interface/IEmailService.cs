namespace ASP_.Net_06_HW.Interface;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string body, string fromEmail = null);
}
