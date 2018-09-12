using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Yunyong.Core;
using Yunyong.EventBus.Events;

namespace Yunyong.EventBus.EasyNetQ
{
    public class ServiceActionInvokeTrackAttribute : AbstractInterceptorAttribute
    {
        public ServiceActionInvokeTrackAttribute(IEventBus bus)
        {
            Bus = bus;
        }

        //[FromContainer]
        public IEventBus Bus { get; }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            if (context.Proxy is IService service)
            {
                var obj = GetParamsObj(context);

                //try
                //{
                //    var stopWatch = new Stopwatch();
                //    stopWatch.Start();
                //    stopWatch.Stop();
                //    //stopWatch.Elapsed
                //    var e = new ServiceActionInvokingEvent
                //    {
                //        Model = context.ImplementationMethod.DeclaringType.FullName,
                //        Method = context.ImplementationMethod.Name,
                //        UserId = GetUserId(context),
                //        ClientIp = GetClientIp(context),
                //        ArgumentJson = JsonConvert.SerializeObject(obj),
                //        Elapsed = stopWatch.Elapsed,
                //    };
                //    Bus.Publish(e);

                //}
                //catch
                //{
                //    // ignored
                //    Console.Error.WriteLine($"[ServiceInvokeTrack Issue: {e.Method}]");
                //}

                try
                {
                    var stopWatch = new Stopwatch();
                    stopWatch.Start();

                    await next(context);

                    stopWatch.Stop();
                    var e = new ServiceActionInvokingEvent
                    {
                        Model = context.ImplementationMethod.DeclaringType.FullName,
                        Method = context.ImplementationMethod.Name,
                        UserId = GetUserId(context),
                        ClientIp = GetClientIp(context),
                        ArgumentJson = JsonConvert.SerializeObject(obj),
                        Elapsed = stopWatch.Elapsed
                    };
                    Bus.Publish(e);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"[ServiceInvokeTrack Issue: {context.ImplementationMethod.Name}]");
                    try
                    {
                        var err = new ServiceActionInvokedErrorEvent
                        {
                            Model = context.ImplementationMethod.DeclaringType.FullName,
                            Method = context.ImplementationMethod.Name,
                            UserId = GetUserId(context),
                            ArgumentJson = JsonConvert.SerializeObject(obj),
                            ClientIp = GetClientIp(context),
                            ExceptionStr = JsonConvert.SerializeObject(new
                            {
                                ex.Message,
                                ex.StackTrace
                            })
                        };
                        Bus.Publish(err);
                    }
                    catch
                    {
                        // ignored
                        //Console.Error.WriteLine($"[ServiceInvokeTrack Error Issue: {e.Method}]");
                    }
                }
            }
        }

        private string GetClientIp(AspectContext context)
        {
            if (context.Implementation is Controller controller)
            {
                controller.GetClientIp();
            }
            else
            {
                return string.Empty;
            }

            return string.Empty;
        }

        private Guid? GetUserId(AspectContext context)
        {
            var contextProperty = context.Implementation.GetType().GetProperty("Context",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            //context.Implementation.GetType().GetProperty("Context", BindingFlags.NonPublic|BindingFlags.GetProperty|BindingFlags.Default);
            try
            {
                if (contextProperty != null)
                {
                    return Guid.Empty;
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

        private JObject GetParamsObj(AspectContext context)
        {
            if (context.Parameters.Any())
            {
                var obj = new JObject();
                var arguments = context.ImplementationMethod.GetParameters();
                for (var i = 0; i < arguments.Length; i++)
                {
                    try
                    {
                        var arg = arguments[i];

                        obj[arg.Name] = context.Parameters[i].ParamJObject();
                        //JsonConvert.DeserializeObject<JToken>(JsonConvert.SerializeObject(context.Parameters[i]));
                    }
                    catch (Exception e)
                    {
                    }
                }

                return obj;
            }

            return null;
        }

        //private string ParamJson(object par)
        //{
        //    if (par is VMBase vm)
        //    {
        //        return vm.ParamJObject().ToString();
        //    }
        //    return string.Empty;
        //}
    }
}