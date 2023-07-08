using Abdrakov.Logging.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abdrakov.Logging.Extensions
{
    public static class LoggingServiceExtensions
    {
        public static void LogWpfBindingErrors(this ILoggingService loggingService, SourceLevels sourceLevels = SourceLevels.Error)
        {
            PresentationTraceSources.DataBindingSource.Listeners.Add(new WpfBindingErrorTraceListener(loggingService));
            PresentationTraceSources.DataBindingSource.Switch.Level = sourceLevels;
        }
    }
}
