using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yunyong.Core;

namespace Yunyong.DataExchange.Tests.Models
{
    [Table("UserInfo")]
    public class UserInfo:Entity
    {
        public string Name { get; set; }
    }
}
