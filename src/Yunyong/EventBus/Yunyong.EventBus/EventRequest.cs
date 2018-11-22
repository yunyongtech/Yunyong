using System;
using Newtonsoft.Json;
using Yunyong.Core;
//using Rainbow.Core;

namespace Yunyong.EventBus
{
    public abstract class EventRequest
    {
        protected EventRequest()
        {
            Id = GuidUtil.NewSequentialId();
        }

        [JsonProperty]
        public Guid Id { get; protected set; }

        public string Token { get; set; }
    }

    //public abstract class EventRequest<TEventUserContext> : EventRequest
    //    where TEventUserContext : IEventUserContext
    //{
    //}
}