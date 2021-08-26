using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class RedisConfiguration
    {
        public string RedisEndPoint { get; set; }
        public int RedisPort { get; set; }
        public int RedisTimeout { get; set; }
    }
}
