using System;
using Newtonsoft.Json;

namespace Yunyong.EventBus.Events
{
    public abstract class EventBase : IEvent
    {
        protected EventBase()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        [JsonProperty]
        public DateTime CreationDate { get; private set; }

        [JsonProperty]
        public Guid Id { get; protected set; }

        /// <summary>
        ///     Gets or sets the token.
        /// </summary>
        /// <value>
        ///     The token.
        /// </value>
        public string Token { get; set; }
    }
}