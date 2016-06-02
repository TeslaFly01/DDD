using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace DDD.Utility
{
    public class MailHelper
    {
        public static void SendMail(string To, string Subject, string Body, string userName, string Password, string Server,string NickName)
        {
            MailAddress from = new MailAddress(userName,NickName);
            MailAddress to = new MailAddress(To);
            MailMessage message = new MailMessage(from, to)
            {
                Subject = Subject,
                Body = Body,
                IsBodyHtml = true,
                SubjectEncoding = Encoding.Default,
                BodyEncoding = Encoding.Default
            };
            
            SmtpClient client = new SmtpClient(Server, 587);
            client.EnableSsl = true;
            
            NetworkCredential credential = new NetworkCredential
            {
                UserName = userName,
                Password = Password
            };
            client.Credentials = credential;
            client.Send(message);
        }


    }
}
