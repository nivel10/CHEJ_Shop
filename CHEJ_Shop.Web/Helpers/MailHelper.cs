namespace CHEJ_Shop.Web.Helpers
{
    using Common.Models;
    using MailKit.Net.Smtp;
    using Microsoft.Extensions.Configuration;
    using MimeKit;
    using System;
    using System.Threading.Tasks;

    public class MailHelper : IMailHelper
    {
        private readonly IConfiguration iConfiguration;

        public MailHelper(IConfiguration _iConfiguration)
        {
            this.iConfiguration = _iConfiguration;
        }

        public async Task<Response> SendMail(
            string _to,
            string _nameTo,
            string _subject,
            string _body)
        {
            try
            {
                var from = this.iConfiguration["Mail:From"];
                var nameFrom = this.iConfiguration["Mail:NameFrom"];
                var smtp = this.iConfiguration["Mail:Smtp"];
                var port = this.iConfiguration["Mail:Port"];
                var password = this.iConfiguration["Mail:Password"];

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(nameFrom, from));
                message.To.Add(new MailboxAddress(_nameTo, _to));
                message.Subject = _subject;
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = _body
                };
                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    #region Old Code

                    //client.Connect(smtp, int.Parse(port), false);
                    //client.Authenticate(from, password);
                    //client.Send(message);
                    //client.Disconnect(true); 

                    #endregion Old Code

                    await client.ConnectAsync(smtp, int.Parse(port), false);
                    await client.AuthenticateAsync(from, password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = "This method is ok...!!!",
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
    }
}
