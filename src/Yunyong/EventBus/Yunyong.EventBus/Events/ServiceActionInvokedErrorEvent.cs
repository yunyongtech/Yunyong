using System;

namespace Yunyong.EventBus.Events
{
    public class ServiceActionInvokedErrorEvent : EventBase
    {
        public string Model { get; set; }
        public string Method { get; set; }
        public Guid? UserId { get; set; }
        public string ArgumentJson { get; set; }
        public string ClientIp { get; set; }
        public string ExceptionStr { get; set; }
    }
}