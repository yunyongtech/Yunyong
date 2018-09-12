using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Yunyong.EventBus.EasyNetQ
{
    public static class ControllerExtensions
    {
        public static string GetClientIp(this Controller controller)
        {
            var ip = controller.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = controller.HttpContext.Connection.RemoteIpAddress.ToString();
            }

            return ip;
        }
    }
}