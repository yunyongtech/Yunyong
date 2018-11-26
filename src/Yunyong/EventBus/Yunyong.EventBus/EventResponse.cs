using System;
using Newtonsoft.Json;
using Yunyong.Core;

namespace Yunyong.EventBus
{
    public abstract class EventResponse
    {
        protected EventResponse(Guid requestId, bool isSuccess = true, string resultDesc = "")
        {
            RequestId = requestId;
            Id = GuidUtil.NewSequentialId();
            IsSuccess = isSuccess;
            ResultDesc = resultDesc;
        }

        [JsonProperty]
        public Guid Id { get; protected set; }

        [JsonProperty]
        public Guid RequestId { get; protected set; }

        [JsonProperty]
        public bool IsSuccess { get; protected set; }

        [JsonProperty]
        public string ResultDesc { get; set; }
    }
}