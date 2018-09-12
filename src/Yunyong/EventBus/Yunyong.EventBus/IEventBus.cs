using System.Threading.Tasks;
using Yunyong.EventBus.Events;

namespace Yunyong.EventBus
{
    /// <summary>
    ///     IEventBus
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        ///     异步消息（匿名）
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="e">The e.</param>
        /// <param name="topic">The topic.</param>
        void Publish<TEvent>(TEvent e, string topic = null) where TEvent : class, IEvent;

        ///// <summary>
        /////     异步消息（上下文用户请求（无上下文时，赋给平台用户上下文））
        ///// </summary>
        ///// <typeparam name="TEvent">The type of the event.</typeparam>
        ///// <typeparam name="TUserContext">The type of the user context.</typeparam>
        ///// <param name="e">The e.</param>
        ///// <param name="userContext">The user context.</param>
        ///// <param name="topic">The topic.</param>
        //void Publish<TEvent, TUserContext>(TEvent e, TUserContext userContext, string topic = null)
        //    where TEvent : class, IEvent where TUserContext : IUserContext;

        void Subscribe<TEvent, TEventHandler>(string topic = null)
            where TEvent : class, IEvent
            where TEventHandler : class, IEventHandler<TEvent>;

        void Unsubscribe<TEvent, TEventHandler>()
            where TEvent : class, IEvent
            where TEventHandler : class, IEventHandler<TEvent>, new();

        /// <summary>
        ///     同步请求（匿名）
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        TResponse Request<TRequest, TResponse>(TRequest request)
            where TRequest : EventRequest
            where TResponse : EventResponse;

        ///// <summary>
        /////     同步请求（上下文用户请求（无上下文时，赋给平台用户上下文））
        ///// </summary>
        ///// <typeparam name="TRequest">The type of the request.</typeparam>
        ///// <typeparam name="TResponse">The type of the response.</typeparam>
        ///// <typeparam name="TUserContext">The type of the user context.</typeparam>
        ///// <param name="request">The request.</param>
        ///// <param name="userContext">The user context.</param>
        ///// <returns></returns>
        //TResponse Request<TRequest, TResponse, TUserContext>(TRequest request, TUserContext userContext)
        //    where TRequest : EventRequest where TResponse : EventResponse where TUserContext : IUserContext;

        Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request)
            where TRequest : EventRequest
            where TResponse : EventResponse;

        void Respond<TRequest, TResponse, TRequestHandler>()
            where TRequest : EventRequest
            where TResponse : EventResponse
            where TRequestHandler : IRequestHandler<TRequest, TResponse>;

        /// <summary>
        ///     请求异步响应
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <typeparam name="TRequestHandler"></typeparam>
        /// <returns></returns>
        void RespondAsync<TRequest, TResponse, TRequestHandler>()
            where TRequest : EventRequest
            where TResponse : EventResponse
            where TRequestHandler : IAsyncRequestHandler<TRequest, TResponse>;

        void Send<TEvent>(string queue, TEvent e) where TEvent : class, IEvent;

        void Receive<TEvent, TEventHandler>(string queue, TEventHandler handler)
            where TEventHandler : IEventHandler<TEvent>
            where TEvent : class, IEvent;
    }
}