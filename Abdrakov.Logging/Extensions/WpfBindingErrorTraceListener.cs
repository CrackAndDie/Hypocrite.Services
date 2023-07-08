using Abdrakov.Logging.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abdrakov.Logging.Extensions
{
    public class WpfBindingErrorTraceListener : TraceListener
    {
        public WpfBindingErrorTraceListener(ILoggingService loggingService)
        {
            this.loggingService = loggingService;
        }

        public override void Write(string message)
        {
            messageBuilder.Append(message);
        }

        public override void WriteLine(string message)
        {
            Write(message);
            loggingService.Error("Binding error: " + messageBuilder.ToString());
        }

        private readonly ILoggingService loggingService;
        private readonly StringBuilder messageBuilder = new StringBuilder();
    }
}
