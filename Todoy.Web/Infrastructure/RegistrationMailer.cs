using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Todoy.Features.Users.Models;

namespace Todoy.Web.Infrastructure
{
    public class RegistrationMailer
    {
        public async Task SendVerificationMailAsync(string siteUrl, User user)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.sendgrid.net";
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("azure_8cb7f8da82048f59446492d9bf71ad95@azure.com", "98964WdGPz6wcue");

            MailMessage mail = new MailMessage();
            mail.To.Add(new MailAddress(user.EmailAddress));
            mail.From = new MailAddress("registrations@todoy.azurewebsites.net");
            mail.Subject = "Verify Todoy Registration.";
            mail.IsBodyHtml = true;

            string plainToken = string.Format("{0};{1}", user.EmailAddress, user.VerificationToken);

            string encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(plainToken));

            string link = string.Format("{0}/verify/{1}", siteUrl, encoded);

            mail.Body =
                string.Format(
                    @"<p>To start using your Todoy account click the following link, <a href=""{0}"">{1}</a></p>",
                    link, 
                    link);

            await client.SendMailAsync(mail);
        }
    }
}