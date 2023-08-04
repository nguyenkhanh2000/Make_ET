using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace Make_ET.Mail
{
    public class Send_Mail
    {
        public void Send_Message(string success_message)
        {
            try
            {
                //set up the email messagee
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Khanh Nguyen", "khanhnv2000@gmail.com"));
                message.To.Add(new MailboxAddress("Trendy HP", "hptrendy@gmail.com"));
                message.Subject = "Notification";

                //Create the message body(text/Html)
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = "Redis_S5G_ET_QUOTE";
                bodyBuilder.HtmlBody = success_message;  /*"<p>S5G_ET_QUOTE saved successfully:</p>"*/

                // Attachments (if any)
                //bodyBuilder.Attachments.Add("path_to_attachment_file");
                message.Body = bodyBuilder.ToMessageBody();
                //Configure the SMTP client
                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 465, useSsl: true);
                    client.Authenticate("khanhnv2000@gmail.com", "sflgsnijtgcuivqr");
                    // Send the email
                    client.Send(message);
                    // Disconnect from the SMTP server
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }
    }
}
