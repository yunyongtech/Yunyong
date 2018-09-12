using System;

namespace Yunyong.EventBus.Events
{
    public class EventPublishErrorRecord
    {
        public Guid Id { get; set; }
        public string EventType { get; set; }
        public string EventJson { get; set; }
        public string Token { get; set; }
        public string Topic { get; set; }
        public string Error { get; set; }
        public string SendType { get; set; }
        public DateTime Time { get; set; }
    }
}