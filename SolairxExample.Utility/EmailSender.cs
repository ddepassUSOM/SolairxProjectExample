using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

namespace SolairxExample.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(ILogger<EmailSender> logger)
        {
            _logger = logger;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            string fromMail = "solairxWeb@solairx.com";
            string fromPassword = "DylanadCarrolemd2174!";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = subject;
            message.To.Add(new MailAddress(email));
            message.Body = "<html><body>" + htmlMessage + "</body></html>";
            message.IsBodyHtml = true;
            using (var smtpClient = new SmtpClient("mail.solairx.com"))
            {
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential(fromMail, fromPassword);
                smtpClient.EnableSsl = false; // Enable SSL for secure communication
                smtpClient.Timeout = 10000; // Set timeout to 10 seconds

                try
                {
                    await smtpClient.SendMailAsync(message);
                }
                catch (SmtpException ex)
                {
                    // Log the exception (you can use your logging framework here)  
                    _logger.LogError($"Failed to send email: {ex.Message}");
                    // Log or handle the exception as needed
                    throw new InvalidOperationException("Failed to send email. See inner exception for details.", ex);
                }
            }
            //var smtpClient = new SmtpClient("mail.solairx.com")
            //{
            //    Port = 587,
            //    Credentials = new NetworkCredential(fromMail, fromPassword),
            //    EnableSsl = false,
            //};
            //smtpClient.Send(message);
        }

        //public Task SendEmailAsync(string email, string subject, string htmlMessage)
        //{
        //    string fromMail = "solairxWeb@solairx.com";
        //    string fromPassword = "Web0123!";

        //    var loginInfo = new NetworkCredential(fromMail, fromPassword);
        //    var mail = new MailMessage();

        //    mail.IsBodyHtml = true;
        //    mail.From = new MailAddress(fromMail, "Solairx Support");
        //    mail.To.Add(email);
        //    mail.Subject = subject;
        //    mail.Body = "<html><body>" + htmlMessage + "</body></html>";
        //    try
        //    {
        //        using (var smtpClient = new SmtpClient("mail.solairx.com", 993))//993
        //        {
        //            smtpClient.EnableSsl = false;
        //            smtpClient.UseDefaultCredentials = false;
        //            smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
        //            smtpClient.Credentials = loginInfo;
        //            smtpClient.Timeout = 50000;//50000
        //            smtpClient.Send(mail);
        //            var t = Task.Run(() => smtpClient.SendAsync(mail, null));
        //            return t;
        //        }
        //    }
        //    catch (SmtpException ex)
        //    {
        //        // Log the exception (you can use your logging framework here)  
        //        _logger.LogError($"Failed to send email: {ex.Message}");
        //        // Log or handle the exception as needed
        //        throw new InvalidOperationException("Failed to send email. See inner exception for details.", ex);
        //    }
        //    finally
        //    {
        //        //dispose the client
        //        mail.Dispose();

        //    }
        //}
    }
}
