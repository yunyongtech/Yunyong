﻿using Yunyong.EventBus.Events;

namespace Yunyong.EventBus
{
    public class EventUnsubscribeEvent : EventBase
    {
        public string EventType { get; set; }
        public string QueueName { get; set; }
        public string Prefix { get; set; }
        public string Handler { get; set; }
    }
}