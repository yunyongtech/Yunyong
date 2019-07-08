using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yunyong.Core
{
    /// <summary>
    ///     实体类基类
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        ///     主键
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; internal set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        public DateTime CreatedOn { get;internal set; }

        [Timestamp]
        public DateTime? UpdatedOn { get; set; }
    }
}