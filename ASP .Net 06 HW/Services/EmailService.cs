using System.Net;
using System.Net.Mail;
using ASP_.Net_06_HW.Interface;
using ASP_.Net_06_HW.Models;
namespace ASP_.Net_06_HW.Services;

public class EmailService : IEmailService
{
    private readonly string _appPassword = "buaxdpcbyuoaoznh";
    public async Task SendEmailAsync(string toEmail, string subject, string body, string fromEmail = null)
    {
        var myProfile = User.Instance;

        var client = new SmtpClient("smtp.gmail.com", 587)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(myProfile.Email, _appPassword)
        };
        string senderName = fromEmail == null ? $"{myProfile.FirstName} {myProfile.LastName}" : "Visitor";
        var mailMessage = new MailMessage()
        {
            From = new MailAddress(myProfile.Email, senderName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(toEmail);

        if (fromEmail != null)
        {
            mailMessage.ReplyToList.Add(new MailAddress(fromEmail));
        }
        await client.SendMailAsync(mailMessage);

    }
}
