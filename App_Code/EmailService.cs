using System;
using System.Collections.Generic;
using System.Web;
using System.Net.Mail;
using System.Configuration;
using System.Diagnostics;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;

public class EmailService
{
    public static void SendEmail(String name, String comments, List<Attachment> attachments)
    {
        using (Stream stream = File.OpenRead(HttpContext.Current.Server.MapPath(@"~\App_Data\Emails.xml")))
        {
            XmlDocument document = new XmlDocument();
            document.Load(stream);

            XmlNode email = document.SelectSingleNode("/emails/email[@name = '" + name + "']");
            
            XmlNode fromNode = email.SelectSingleNode("from");
            XmlNode toNode = email.SelectSingleNode("to");
            XmlNode ccNode = email.SelectSingleNode("cc");
            XmlNode subjectNode = email.SelectSingleNode("subject");
            XmlNode bodyNode = email.SelectSingleNode("body");

            MailMessage mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(fromNode.InnerText);

            String[] toAddresses = Regex.Split(toNode.InnerText, @"\s*,\s*");

            foreach (String toAddress in toAddresses)
                mailMessage.To.Add(toAddress);

            if (!String.IsNullOrEmpty(ccNode.InnerText))
            {
                String[] ccAddresses = Regex.Split(ccNode.InnerText, @"\s*,\s*");

                foreach (String ccAddress in ccAddresses)
                    mailMessage.CC.Add(ccAddress);
            }

            mailMessage.Subject = subjectNode.InnerText;
            mailMessage.Body = bodyNode.InnerText.Trim();

            if (!String.IsNullOrEmpty(comments))
                mailMessage.Body += Environment.NewLine + Environment.NewLine + comments;

            foreach (Attachment attachment in attachments)
                mailMessage.Attachments.Add(attachment);

            SmtpClient smtp = new SmtpClient();

            smtp.Send(mailMessage);
        }
    }
}