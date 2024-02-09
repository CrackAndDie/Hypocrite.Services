using Hypocrite.Core.Logging.Interfaces;
using System.Diagnostics;

namespace Hypocrite.Core.Logging.Extensions
{
    public static class LoggingServiceExtensions
    {
        public static void LogWpfBindingErrors(this ILoggingService loggingService, SourceLevels sourceLevels = SourceLevels.Error)
        {
#if NET462 || NET472 || NETFRAMEWORK
            PresentationTraceSources.DataBindingSource.Listeners.Add(new WpfBindingErrorTraceListener(loggingService));
            PresentationTraceSources.DataBindingSource.Switch.Level = sourceLevels;
#endif
        }
    }
}
