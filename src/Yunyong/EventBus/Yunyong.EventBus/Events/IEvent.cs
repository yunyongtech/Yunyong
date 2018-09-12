using System;
using Newtonsoft.Json;

namespace Yunyong.EventBus.Events
{
    public interface IEvent
    {
        [JsonProperty]
        Guid Id { get; }

        /// <summary>
        ///     Gets or sets the token.
        /// </summary>
        /// <value>
        ///     The token.
        /// </value>
        [JsonProperty]
        string Token { get; set; }
    }

    //[BsonIgnoreExtraElements]
}