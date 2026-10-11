using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Net;
using System.Net.Mail;
namespace HomeApp.Services
{
    public class MailService
    {
        private IConfiguration _configuration;
        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async void Login()
        {
            var password = _configuration.GetValue<string>("GmailCreds:Password");
            using var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential("vannieuland.geordi@gmail.com", "Mamamama12357951")
            };

            using var client = new ImapClient();


            await client.ConnectAsync("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);

            await client.AuthenticateAsync("vannieuland.geordi@gmail.com", password);

            var inbox = client.Inbox;

            await inbox.OpenAsync(FolderAccess.ReadWrite);
            var unreadUids = await inbox.SearchAsync(SearchQuery.DeliveredBefore(DateTime.Now.AddYears(-3)));

            Console.WriteLine($"Totale mails: {inbox.Count}");
            Console.WriteLine($"Totale mails sinds 5 jaar geleded: {unreadUids.Count}");
            await inbox.AddFlagsAsync(
    unreadUids,
    MessageFlags.Deleted,
    true);
            /*
            foreach (var uid in unreadUids)
            {
               // var message = await inbox.GetMessageAsync(uid);
              //  Console.WriteLine(message.From);
                //Console.WriteLine(message.Subject);
                Console.WriteLine($"Marking uuid {uid} as read :P");
                await inbox.AddFlagsAsync(uid, MessageFlags.Seen, true);
                Console.WriteLine("Next :)");

            }*/

            //All Mails
            /*
            for (int i = 0; i < inbox.Count; i++)
            {
                var message = await inbox.GetMessageAsync(i);
                if(message.Headers)
                Console.WriteLine($"Van: {message.From}");
                Console.WriteLine($"Onderwerp: {message.Subject}");
                Console.WriteLine($"Datum: {message.Date}");
                Console.WriteLine();
            }
            */
            await client.DisconnectAsync(true);
        }
    }
}
