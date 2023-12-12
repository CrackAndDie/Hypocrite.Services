using Abdrakov.Logging.Extensions;
using Abdrakov.Logging.Interfaces;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using System;

namespace Abdrakov.Logging.Services
{
    public class Log4netLoggingService : ILoggingService
    {
        public Log4netLoggingService()
        {
            var patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%date | %thread | %level | %message%newline";
            patternLayout.ActivateOptions();

            var fileAppender = new FileAppender()
            {
                Layout = patternLayout,
                Name = "fileAppender",
                Threshold = Level.All,
                AppendToFile = false,
                File = "app.log",
            };
            fileAppender.ActivateOptions();
            BasicConfigurator.Configure(fileAppender);

            logger = LogManager.GetLogger("Log4netLogger");
            this.LogWpfBindingErrors();
        }

        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Info(string message)
        {
            logger.Info(message);
        }

        public void Warn(string message)
        {
            logger.Warn(message);
        }

        public void Error(string message)
        {
            logger.Error(message);
        }

        public void Fatal(string message)
        {
            logger.Fatal(message);
        }

        public void Debug(string message, Exception exception)
        {
            logger.Debug(message, exception);
        }

        public void Info(string message, Exception exception)
        {
            logger.Info(message, exception);
        }

        public void Warn(string message, Exception exception)
        {
            logger.Warn(message, exception);
        }

        public void Error(string message, Exception exception)
        {
            logger.Error(message, exception);
        }

        public void Fatal(string message, Exception exception)
        {
            logger.Fatal(message, exception);
        }

        private readonly ILog logger;
    }
}
