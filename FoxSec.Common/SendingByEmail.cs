using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;

namespace FoxSec.Common.SendMail
{
    /// <summary>
    /// Sending EMail
    /// </summary>
    public static class Logger4SendingEMail
    {
        private static readonly ILog logSender = LogManager.GetLogger(typeof(Logger4SendingEMail));

        public static ILog LogSender
        {
            get { return logSender; }
        }

        public static void InitLogger()
        {
            XmlConfigurator.Configure();
        }

        public static void InitSender(string sendTo)
        {
            ((SmtpAppender)(logSender.Logger.Repository.GetAppenders()[0])).To = sendTo;
            ((SmtpAppender)(logSender.Logger.Repository.GetAppenders()[0])).Subject = "Registration info";
            
            XmlConfigurator.Configure(logSender.Logger.Repository);
        }
    }
}
