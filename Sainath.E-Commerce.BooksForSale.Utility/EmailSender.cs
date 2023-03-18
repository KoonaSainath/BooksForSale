using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string emailTo, string subject, string htmlMessage)
        {
            //enable 2 factor authentication and set it up for the gmail you are trying as from address
            //after 2 factor auth is enabled, you can see "Apps password" option in security tab of manage account (2-factor auth also can be set up here only)
            //give it a name and choose custom domain in dropdown. Now copy the displayed password and use it here as a password to authenticate
            //this would work until you delete the apps password from your gmail account's security tab
            string appsPassword = "klklursawjahxlwl";
            string emailFrom = "sainath.koona@gmail.com";
            try
            {
                MimeMessage mimeMessage = new MimeMessage();

                mimeMessage.To.Add(MailboxAddress.Parse(emailTo));
                mimeMessage.From.Add(MailboxAddress.Parse(emailFrom));

                mimeMessage.Subject = subject;

                TextPart textPart = new TextPart(MimeKit.Text.TextFormat.Html);
                textPart.Text = htmlMessage;

                mimeMessage.Body = textPart;

                using (SmtpClient smtpClient = new SmtpClient())
                {
                    smtpClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    smtpClient.Authenticate(emailFrom, appsPassword);
                    smtpClient.Send(mimeMessage);
                    smtpClient.Disconnect(true);
                }
            }
            catch(Exception e)
            {

            }
            
            return Task.CompletedTask;
        }
    }
}
