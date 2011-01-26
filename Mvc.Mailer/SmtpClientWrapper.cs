﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.ComponentModel;

namespace Mvc.Mailer
{
    public class SmtpClientWrapper : SmtpClientBase
    {
        public SmtpClientWrapper()
        {
            InnerSmtpClient = new SmtpClient();
        }
        
        public SmtpClientWrapper(SmtpClient smtpClient)
        {
            InnerSmtpClient = smtpClient;
        }

        public SmtpClient InnerSmtpClient
        {
            get;
            set;
        }

        public override void Send(MailMessage mailMessage)
        {
            using (InnerSmtpClient)
            {
                InnerSmtpClient.Send(mailMessage);
            }
        }

        public override void SendAsync(MailMessage mailMessage, object userState)
        {
            using (InnerSmtpClient)
            {
                InnerSmtpClient.SendCompleted += new SendCompletedEventHandler(InnerSmtpClient_SendCompleted);
                InnerSmtpClient.SendAsync(mailMessage, userState);
            }
        }

        void InnerSmtpClient_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            OnSendCompleted(sender, e);
        }

        public override void Dispose()
        {
            InnerSmtpClient.Dispose();
        }

        public static implicit operator SmtpClientWrapper(SmtpClient innerSmtpClient)
        {
            return new SmtpClientWrapper(innerSmtpClient);
        }
    }
}
