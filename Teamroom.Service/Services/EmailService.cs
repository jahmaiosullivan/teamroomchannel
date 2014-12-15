using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Internal;
using System.Threading.Tasks;
using Mandrill;

namespace HobbyClue.Business.Services
{
    public interface IEmailService
    {
        List<EmailResult> Send(IList<EmailAddress> recipients, string fromEmail, string fromName, string body, string subject);
    }

    public class EmailService : IEmailService
    {
        

        public List<EmailResult> Send(IList<EmailAddress> recipients, string fromEmail, string fromName, string body, string subject)
        {
            var apiKey = ConfigurationManager.AppSettings["MandrillApiKey"];
            var api = new MandrillApi(apiKey);
            var sendTask = api.SendMessageAsync(new EmailMessage
            {
                to = recipients,
                from_email = fromEmail,
                from_name = fromName,
                html = body,
                subject = subject,
                track_opens = true
            });

            var results = sendTask.ContinueWith(data => data.Result);

            return results.Result;
        }
    }
}
