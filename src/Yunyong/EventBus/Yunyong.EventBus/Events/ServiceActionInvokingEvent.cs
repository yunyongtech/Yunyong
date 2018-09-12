using System;

namespace Yunyong.EventBus.Events
{
    public class ServiceActionInvokingEvent : EventBase
    {
        public string Model { get; set; }
        public string Method { get; set; }
        public Guid? UserId { get; set; }
        public string ArgumentJson { get; set; }
        public string ClientIp { get; set; }

        /// <summary>
        ///     方法调用处理时长
        /// </summary>
        public TimeSpan Elapsed { get; set; }
    }
}