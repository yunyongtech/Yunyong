namespace Yunyong.EventBus
{
    public interface IRequestHandler<in TRequst, out TResponse> where TResponse : EventResponse
    {
        TResponse Handle(TRequst request);
    }
}