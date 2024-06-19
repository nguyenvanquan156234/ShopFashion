using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using BanHangOnline.Models;

namespace BanHangOnline.Models.Common
{
    public class SendMail
    {
        private static string password = ConfigurationManager.AppSettings["PasswordEmail"];
        private static string Email = ConfigurationManager.AppSettings["Email"];
        public static bool SendMailer(string name, string subject,string content,string toMail)
        {
            bool rs = false;
            try
            {
                MailMessage mailMessage = new MailMessage();
                var smtp = new System.Net.Mail.SmtpClient();
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.Credentials = new NetworkCredential(Email, password);
                    smtp.Timeout = 20000;

                }
                MailAddress mailAddress = new MailAddress(Email,name);
                mailMessage.From = mailAddress;
                mailMessage.To.Add(toMail);
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = content;
                smtp.Send(mailMessage);
                rs = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                rs = false;
            }
            return rs;
        }
    }
}