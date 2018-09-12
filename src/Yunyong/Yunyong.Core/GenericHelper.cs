using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace Yunyong.Core
{
    /// <summary>
    ///     泛型 Helper
    /// </summary>
    public class GenericHelper : ClassInstance<GenericHelper>
    {
        public Attribute GetAttribute<A>(Type mType, PropertyInfo prop)
        {
            try
            {
                return mType.GetMember(prop.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)[0]
                    .GetCustomAttribute(typeof(A), false);
            }
            catch (Exception ex)
            {
                throw new Exception("方法GetAttribute<A>出错:" + ex.Message);
            }
        }

        public RM GetPropertyValue<M, RM>(M m, string properyName)
        {
            try
            {
                if (m is ExpandoObject)
                {
                    var dic = m as IDictionary<string, object>;
                    return (RM) dic[properyName];
                }

                return (RM) m.GetType().GetProperty(properyName).GetValue(m, null);
            }
            catch (Exception ex)
            {
                throw new Exception("方法GetPropertyValue<M, RM>出错:" + ex.Message);
            }
        }

        public string GetPropertyValue<M>(M m, string properyName)
        {
            try
            {
                if (m is ExpandoObject)
                {
                    var dic = m as IDictionary<string, object>;
                    return ConvertType(dic[properyName]);
                }

                return m.GetType().GetProperty(properyName).GetValue(m, null).ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("方法GetPropertyValue<M>出错:" + "请向方法 [GenericHelper.ConvertType] 中添加类型解析 " + ex.Message);
            }
        }

        public void SetPropertyValue<M>(M m, string propertyName, object value)
        {
            try
            {
                if (value != null)
                {
                    m.GetType().GetProperty(propertyName).SetValue(m, value, null);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("方法SetPropertyValue<M>出错:" + ex.Message);
            }
        }

        private string ConvertType(object value)
        {
            if (value.GetType() == typeof(DateTime))
            {
                return Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss");
            }

            if (value.GetType() == typeof(Guid))
            {
                return value.ToString();
            }

            return value.ToString();
        }
    }
}