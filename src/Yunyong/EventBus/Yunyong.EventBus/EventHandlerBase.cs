using Microsoft.Extensions.Logging;

namespace Yunyong.EventBus
{
    public class EventHandlerBase
    {
        public EventHandlerBase(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        protected ILogger Logger { get; }
    }
}