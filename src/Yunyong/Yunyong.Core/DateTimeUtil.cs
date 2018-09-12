using System;

namespace Yunyong.Core
{
    /// <summary>
    ///     DateTimeUtil
    /// </summary>
    public static class DateTimeUtil
    {
        /// <summary>
        ///     获取当前时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }
        /// <summary>
        /// 获取当前年份
        /// </summary>
        /// <returns></returns>
        public static int GetThisYear()
        {
            return DateTime.Today.Year;
        }
        /// <summary>
        /// 获取当前月份
        /// </summary>
        /// <returns></returns>
        public static Month GetThisMonth()
        {
            return (Month) DateTime.Today.Month;
        }
    }
}