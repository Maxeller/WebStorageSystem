using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStorageSystem.Models
{
    public class BaseModel
    {
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
