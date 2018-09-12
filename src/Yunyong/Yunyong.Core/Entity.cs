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
        public Guid Id { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        [Timestamp]
        public DateTime CreatedOn { get; set; }

        //public DateTime? UpdatedOn { get; set; }
    }
}