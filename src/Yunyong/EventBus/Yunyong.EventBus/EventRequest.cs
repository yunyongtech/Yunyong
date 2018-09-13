using System;
using Newtonsoft.Json;
//using Rainbow.Core;
using Yunyong.Core;

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