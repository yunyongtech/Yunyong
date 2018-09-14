using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using Yunyong.EventBus.Events;

namespace Yunyong.EventBus.EasyNetQ
{
    /// <summary>
    ///     EventBus 通过RabbitMQ的实现
    /// </summary>
    /// <seealso cref="IEventBus" />
    /// <seealso cref="IDisposable" />
    internal class EventBusEasyNetQ : IEventBus, IDisposable
    {
        //private IServiceScope ServiceScope { get; }
        private readonly IMongoDatabase _db = MongoDbContext.GetMongoDB();

        public EventBusEasyNetQ(IBus bus, IServiceProvider provider, string prefix, ILoggerFactory loggerFactory, bool enableEventLog = true)
        {
            Bus = bus;
            Provider = provider;
            Prefix = prefix;
            EnableEventLog = enableEventLog;

            Logger = loggerFactory.CreateLogger<EventBusEasyNetQ>();
            //ServiceScope = provider.CreateScope();
        }

        private ILogger Logger { get; }
        private IBus Bus { get; }
        private IServiceProvider Provider { get; }

        private string Prefix { get; }
        private bool EnableEventLog { get; }

        private Dictionary<string, ISubscriptionResult> Subscriptions { get; } =
            new Dictionary<string, ISubscriptionResult>();

        public void Dispose()
        {
            //ServiceScope?.Dispose();
            foreach (var key in Subscriptions.Keys)
            {
                var queueName = Subscriptions[key].Queue.Name;
                //result.Dispose();
                Subscriptions[key].Dispose();

                Publish(new EventUnsubscribeEvent {EventType = key, QueueName = queueName, Prefix = Prefix});
            }
        }

        /// <summary>
        ///     异步消息（匿名）
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="e">The e.</param>
        /// <param name="topic">The topic.</param>
        public void Publish<TEvent>(TEvent e, string topic = null) where TEvent : class, IEvent
        {
            var sw = new Stopwatch();
            sw.Start();
            try
            {
                if (string.IsNullOrEmpty(topic))
                {
                    Bus.Publish(e);
                }
                else
                {
                    Bus.Publish(e, topic);
                }

                if (!(e is EventUnsubscribeEvent))
                {
                    TraceEventPublishRecordLog(new EventPublishRecord
                    {
                        Id = e.Id,
                        Token = e.Token,
                        EventType = GetEventKey<TEvent>(),
                        EventJson = JsonConvert.SerializeObject(e,
                            new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore}),
                        Topic = topic,
                        SendType = "Publish",
                        Time = DateTime.Now.ToUniversalTime()
                    });
                }
            }
            catch (Exception ex)
            {
                if (!(e is EventUnsubscribeEvent))
                {
                    TraceEventPublishRecordLog(new EventPublishErrorRecord
                    {
                        Id = e.Id,
                        Token = e.Token,
                        EventType = GetEventKey<TEvent>(),
                        EventJson = JsonConvert.SerializeObject(e,
                            new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore}),
                        Topic = topic,
                        SendType = "Publish",
                        Error = ex.ToString(),
                        Time = DateTime.Now.ToUniversalTime()
                    });
                }
            }
            finally
            {
                sw.Stop();
                Logger.LogWarning($"{sw.Elapsed}\t{GetType().Name}.Publish({e.GetType().Name} e, string topic = {topic})");
            }
        }


        ///// <summary>
        /////     异步消息（上下文用户请求（无上下文时，赋给平台用户上下文））
        ///// </summary>
        ///// <typeparam name="TEvent">The type of the event.</typeparam>
        ///// <typeparam name="TUserContext">The type of the user context.</typeparam>
        ///// <param name="e">The e.</param>
        ///// <param name="userContext">The user context.</param>
        ///// <param name="topic">The topic.</param>
        //public void Publish<TEvent, TUserContext>(TEvent e, TUserContext userContext, string topic = null)
        //    where TEvent : class, IEvent where TUserContext : IUserContext
        //{
        //    try
        //    {
        //        //e.Token = GetToken(userContext);
        //        if (string.IsNullOrEmpty(topic))
        //            Bus.Publish(e);
        //        else
        //            Bus.Publish(e, topic);

        //        if (!(e is EventUnsubscribeEvent))
        //            TraceEventPublishRecordLog(new EventPublishRecord
        //            {
        //                Id = e.Id,
        //                Token = e.Token,
        //                EventType = GetEventKey<TEvent>(),
        //                EventJson = JsonConvert.SerializeObject(e,
        //                    new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
        //                Topic = topic,
        //                SendType = "Publish",
        //                Time = DateTime.Now.ToUniversalTime()
        //            });
        //    }
        //    catch (Exception ex)
        //    {
        //        if (!(e is EventUnsubscribeEvent))
        //            TraceEventPublishRecordLog(new EventPublishErrorRecord
        //            {
        //                Id = e.Id,
        //                Token = e.Token,
        //                EventType = GetEventKey<TEvent>(),
        //                EventJson = JsonConvert.SerializeObject(e,
        //                    new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
        //                Topic = topic,
        //                SendType = "Publish",
        //                Error = ex.ToString(),
        //                Time = DateTime.Now.ToUniversalTime()
        //            });
        //    }
        //}


        public void Subscribe<TEvent, TEventHandler>(string topic = null) where TEvent : class, IEvent
            where TEventHandler : class, IEventHandler<TEvent>
        {
            ISubscriptionResult result;
            try
            {
                if (string.IsNullOrEmpty(topic))
                {
                    result = Bus.Subscribe<TEvent>(GetEventKey<TEvent>(),
                        e =>
                        {
                            try
                            {
                                using (var scope = Provider.CreateScope())
                                {
                                    var handler = scope.ServiceProvider.GetService<TEventHandler>();
                                    //<TEventHandler>();
                                    Logger.LogWarning($"{handler.GetType().Name}.Handle({e.GetType().Name} e)");
                                    handler?.Handle(e);
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.LogError(ex, ex.Message);
                                // ignore
                            }
                        });
                }
                else
                {
                    result = Bus.Subscribe<TEvent>(GetEventKey<TEvent>(),
                        e =>
                        {
                            try
                            {
                                using (var scope = Provider.CreateScope())
                                {
                                    var handler = scope.ServiceProvider.GetService<TEventHandler>();
                                    Logger.LogWarning($"{handler.GetType().Name}.Handle({e.GetType().Name} e)");
                                    handler?.Handle(e);
                                }
                            }
                            catch
                            {
                                // ignore
                            }
                        },
                        x => x.WithTopic(topic));
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                throw;
            }

            if (result != null)
            {
                Subscriptions[GetEventKey<TEvent>()] = result;
                if (typeof(TEvent) != typeof(EventSubscribeEvent))
                {
                    Publish(new EventSubscribeEvent
                    {
                        EventType = GetEventKey<TEvent>(),
                        QueueName = result.Queue.Name,
                        Prefix = Prefix,
                        Handler = typeof(TEventHandler).FullName
                    });
                }
            }
        }

        public void Unsubscribe<TEvent, TEventHandler>() where TEvent : class, IEvent
            where TEventHandler : class, IEventHandler<TEvent>, new()
        {
        }

        /// <summary>
        ///     同步请求（匿名）
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public TResponse Request<TRequest, TResponse>(TRequest request)
            where TRequest : EventRequest where TResponse : EventResponse
        {
            try
            {
                var response = Bus.Request<TRequest, TResponse>(request);

                TraceEventPublishRecordLog(new EventPublishRecord
                {
                    Id = request.Id,
                    Token = request.Token,
                    EventType = typeof(TRequest).FullName,
                    EventJson = JsonConvert.SerializeObject(request,
                        new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore}),
                    //Topic = topic,
                    SendType = "Request",
                    Time = DateTime.Now.ToUniversalTime()
                });
                return response;
            }
            catch (Exception ex)
            {
                TraceEventPublishRecordLog(new EventPublishErrorRecord
                {
                    Id = request.Id,
                    Token = request.Token,
                    EventType = typeof(TRequest).FullName,
                    EventJson = JsonConvert.SerializeObject(request,
                        new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore}),
                    //Topic = topic,
                    SendType = "Request",
                    Error = ex.ToString(),
                    Time = DateTime.Now.ToUniversalTime()
                });
                return null;
            }
        }

        ///// <summary>
        /////     同步请求（上下文用户请求（无上下文时，赋给平台用户上下文））
        ///// </summary>
        ///// <typeparam name="TRequest">The type of the request.</typeparam>
        ///// <typeparam name="TResponse">The type of the response.</typeparam>
        ///// <typeparam name="TUserContext">The type of the user context.</typeparam>
        ///// <param name="request">The request.</param>
        ///// <param name="userContext">The user context.</param>
        ///// <returns></returns>
        //public TResponse Request<TRequest, TResponse, TUserContext>(TRequest request, TUserContext userContext)
        //    where TRequest : EventRequest where TResponse : EventResponse where TUserContext : IUserContext
        //{
        //    try
        //    {
        //        var response = Bus.Request<TRequest, TResponse>(request);

        //        TraceEventPublishRecordLog(new EventPublishRecord
        //        {
        //            Id = request.Id,
        //            Token = request.Token,
        //            EventType = typeof(TRequest).FullName,
        //            EventJson = JsonConvert.SerializeObject(request,
        //                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
        //            //Topic = topic,
        //            SendType = "Request",
        //            Time = DateTime.Now.ToUniversalTime()
        //        });
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        TraceEventPublishRecordLog(new EventPublishErrorRecord
        //        {
        //            Id = request.Id,
        //            Token = request.Token,
        //            EventType = typeof(TRequest).FullName,
        //            EventJson = JsonConvert.SerializeObject(request,
        //                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
        //            //Topic = topic,
        //            SendType = "Request",
        //            Error = ex.ToString(),
        //            Time = DateTime.Now.ToUniversalTime()
        //        });
        //        return null;
        //    }
        //}

        public async Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request)
            where TRequest : EventRequest where TResponse : EventResponse
        {
            //return await Bus.RequestAsync<TRequest, TResponse>(request);

            try
            {
                var response = await Bus.RequestAsync<TRequest, TResponse>(request);

                TraceEventPublishRecordLog(new EventPublishRecord
                {
                    Id = request.Id,
                    Token = request.Token,
                    EventType = typeof(TRequest).FullName,
                    EventJson = JsonConvert.SerializeObject(request,
                        new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore}),
                    //Topic = topic,
                    SendType = "RequestAsync",
                    Time = DateTime.Now.ToUniversalTime()
                });
                return response;
            }
            catch (AggregateException ae)
            {
                ae.Flatten().Handle(it => true);
                return null;
            }
            catch (Exception ex)
            {
                TraceEventPublishRecordLog(new EventPublishErrorRecord
                {
                    Id = request.Id,
                    Token = request.Token,
                    EventType = typeof(TRequest).FullName,
                    EventJson = JsonConvert.SerializeObject(request,
                        new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore}),
                    //Topic = topic,
                    SendType = "RequestAsync",
                    Error = ex.ToString(),
                    Time = DateTime.Now.ToUniversalTime()
                });
                return null;
            }
        }

        public void Respond<TRequest, TResponse, TRequestHandler>() where TRequest : EventRequest
            where TResponse : EventResponse
            where TRequestHandler : IRequestHandler<TRequest, TResponse>
        {
            try
            {
                //Bus.Respond<TRequest, TResponse>(handler.Handle);
                Bus.Respond<TRequest, TResponse>(request =>
                {
                    using (var scope = Provider.CreateScope())
                    {
                        var handler = scope.ServiceProvider.GetService<TRequestHandler>();
                        var sw = new Stopwatch();
                        sw.Start();

                        try
                        {
                            return handler.Handle(request);
                        }
                        finally
                        {
                            sw.Stop();
                            Logger.LogWarning($"{sw.Elapsed}\t{handler.GetType().Name}.Handle({request.GetType().Name} request)");
                        }
                    }
                });

                Publish(new EventSubscribeEvent
                {
                    EventType = GetEventKey<TRequest>(),
                    //QueueName = result.Queue.Name,
                    Prefix = Prefix,
                    Handler = typeof(TRequestHandler).FullName
                });
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                // ignored
            }
        }


        public void Send<TEvent>(string queue, TEvent e) where TEvent : class, IEvent
        {
            try
            {
                Bus.Send(queue, e);

                TraceEventPublishRecordLog(new EventPublishRecord
                {
                    Id = e.Id,
                    Token = e.Token,
                    EventType = typeof(TEvent).FullName,
                    EventJson = JsonConvert.SerializeObject(e,
                        new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore}),
                    //Topic = topic,
                    SendType = "Send",
                    Time = DateTime.Now.ToUniversalTime()
                });
            }
            catch (Exception ex)
            {
                TraceEventPublishRecordLog(new EventPublishErrorRecord
                {
                    Id = e.Id,
                    Token = e.Token,
                    EventType = GetEventKey<TEvent>(),
                    EventJson = JsonConvert.SerializeObject(e,
                        new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore}),
                    //Topic = topic,
                    SendType = "Send",
                    Error = ex.ToString(),
                    Time = DateTime.Now.ToUniversalTime()
                });
            }
        }

        public void Receive<TEvent, TEventHandler>(string queue, TEventHandler handler) where TEvent : class, IEvent
            where TEventHandler : IEventHandler<TEvent>
        {
            Bus.Receive<TEvent>(queue, handler.Handle);
        }

        public void RespondAsync<TRequest, TResponse, TRequestHandler>()
            where TRequest : EventRequest
            where TResponse : EventResponse
            where TRequestHandler : IAsyncRequestHandler<TRequest, TResponse>
        {
            try
            {
                Bus.RespondAsync<TRequest, TResponse>(request =>
                {
                    using (var scope = Provider.CreateScope())
                    {
                        var handler = scope.ServiceProvider.GetService<TRequestHandler>();

                        var sw = new Stopwatch();
                        sw.Start();

                        try
                        {
                            return handler.Handle(request);
                        }
                        finally
                        {
                            sw.Stop();
                            Logger.LogWarning($"{sw.Elapsed}\t{handler.GetType().Name}.Handle({request.GetType().Name} request)");
                        }
                    }
                });
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                // ignored
            }
        }

        private void TraceEventPublishRecordLog<TEventPublishRecord>(TEventPublishRecord record)
        {
            if (EnableEventLog)
            {
                var recordCollection = _db?.GetCollection<TEventPublishRecord>(typeof(TEventPublishRecord).Name);
                Logger.LogInformation(JsonConvert.SerializeObject(record, Formatting.Indented,
                    new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore}));
                recordCollection?.InsertOne(record);
            }
        }

        private string GetEventKey<T>()
        {
            return $"{Prefix}.{typeof(T).FullName}";
        }
    }
}