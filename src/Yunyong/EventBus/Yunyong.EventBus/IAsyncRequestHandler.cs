using System.Threading.Tasks;

namespace Yunyong.EventBus
{
    public interface IAsyncRequestHandler<in TRequest, TResponse>
        where TRequest : EventRequest
        where TResponse : EventResponse
    {
        Task<TResponse> Handle(TRequest request);
    }
}