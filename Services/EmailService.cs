using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using rmsbe.Configs;
using rmsbe.Contracts.Email.Request;
using rmsbe.Contracts.Email.Response;
using rmsbe.Services.Interfaces;

namespace rmsbe.Services;

public class EmailService : IEmailService
{
    public EmailServiceResponse Send(EmailRequestBody emailRequestBody)
    {
        var recipients = new InternetAddressList();
        foreach (var rc in emailRequestBody.To)
        {
            recipients.Add(MailboxAddress.Parse(rc));
        }
        
        // create message
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(SMTPConfigs.SenderEmail));
        email.To.AddRange(recipients);
        email.Subject = !string.IsNullOrWhiteSpace(emailRequestBody.Subject) ? emailRequestBody.Subject : SMTPConfigs.SubjectText;
        email.Body = new TextPart(TextFormat.Plain) { Text = emailRequestBody.Text };

        // send email
        using var smtp = new SmtpClient();
        smtp.Connect(SMTPConfigs.SmtpHost, SMTPConfigs.SmtpPort, SecureSocketOptions.StartTls);
        smtp.Authenticate(SMTPConfigs.SmtpUsername, SMTPConfigs.SmtpPassword);
        smtp.Send(email);
        smtp.Disconnect(true);

        return new EmailServiceResponse()
        {
            Status = "Success"
        };
    }
}