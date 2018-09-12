using System;
using System.Collections;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Yunyong.Core;

namespace Yunyong.EventBus.EasyNetQ
{
    internal static class VMJsonExtensions
    {
        public static JToken ParamJObject(this object vm)
        {
            try
            {
                if (vm == null)
                {
                    return null;
                }

                if (!vm.GetType().IsClass || vm.GetType().IsEnum || vm is string || vm is Guid)
                {
                    return vm.ToString();
                }

                var obj = new JObject();

                if (vm is IEnumerable)
                {
                    var arr = new JArray();
                    foreach (var item in vm as IEnumerable)
                    {
                        arr.Add(item.ParamJObject());
                    }

                    return arr;
                }

                var properties = vm.GetType().GetProperties();

                foreach (var property in properties)
                {
                    if (property.GetCustomAttribute<LogIgnoreAttribute>() != null)
                    {
                    }
                    else
                    {
                        var val = property.GetValue(vm);
                        if (val != null)
                        {
                            obj[property.Name] = ParamJObject(val);
                        }
                    }
                }

                return obj;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}