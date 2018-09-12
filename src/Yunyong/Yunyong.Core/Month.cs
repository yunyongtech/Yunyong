using System.ComponentModel.DataAnnotations;

namespace Yunyong.Core
{
    /// <summary>
    ///     月份
    /// </summary>
    //[JsonConverter(typeof(StringEnumConverter))]
    public enum Month
    {
        /// <summary>
        ///     一月
        /// </summary>
        [Display(Name = "一月")] January = 1,

        /// <summary>
        ///     二月
        /// </summary>
        [Display(Name = "二月")] February = 2,

        /// <summary>
        ///     三月
        /// </summary>
        [Display(Name = "三月")] March = 3,

        /// <summary>
        ///     四月
        /// </summary>
        [Display(Name = "四月")] April = 4,

        /// <summary>
        ///     五月
        /// </summary>
        [Display(Name = "五月")] May = 5,

        /// <summary>
        ///     六月
        /// </summary>
        [Display(Name = "六月")] June = 6,

        /// <summary>
        ///     七月
        /// </summary>
        [Display(Name = "七月")] July = 7,

        /// <summary>
        ///     八月
        /// </summary>
        [Display(Name = "八月")] August = 8,

        /// <summary>
        ///     九月
        /// </summary>
        [Display(Name = "九月")] September = 9,

        /// <summary>
        ///     十月
        /// </summary>
        [Display(Name = "十月")] October = 10,

        /// <summary>
        ///     十一月
        /// </summary>
        [Display(Name = "十一月")] November = 11,

        /// <summary>
        ///     十二月
        /// </summary>
        [Display(Name = "十二月")] December = 12
    }
}