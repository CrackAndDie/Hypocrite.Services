using Abdrakov.Logging.Extensions;
using Abdrakov.Logging.Interfaces;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abdrakov.Logging.Services
{
    public class Log4netLoggingService : ILoggingService
    {
        public Log4netLoggingService()
        {
            logger = LogManager.GetLogger("FlowLogger");
        }

        public Log4netLoggingService(string name)
        {
            logger = LogManager.GetLogger(name);
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

        public static Log4netLoggingService GetMainInstance()
        {
            if (mainInstance == null)
            {
                var patternLayout = new PatternLayout();
                patternLayout.ConversionPattern = "%date | %thread | %level | %message%newline";
                patternLayout.ActivateOptions();

                // creating the regular console appender
                var fileAppender = new FileAppender()
                {
                    Layout = patternLayout,
                    Name = "fileAppender",
                    Threshold = Level.All,
                    AppendToFile = false,
                    File = "cadlog.log",
                };
                fileAppender.ActivateOptions();
                BasicConfigurator.Configure(fileAppender);

                mainInstance = new Log4netLoggingService();
                mainInstance.LogWpfBindingErrors();
            }

            return mainInstance;
        }

        private static Log4netLoggingService mainInstance = null;
        private readonly ILog logger;
    }
}
