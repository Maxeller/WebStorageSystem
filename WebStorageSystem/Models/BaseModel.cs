using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStorageSystem.Models
{
    public class BaseModel
    {
        [Display(Name = "Created")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Last Modification")]
        public DateTime ModifiedDate { get; set; }
        [Display(Name = "Deleted")]
        public bool IsDeleted { get; set; }

        [Timestamp]
        [ScaffoldColumn(false)]
        public byte[] RowVersion { get; set; }
    }

    public class BaseModelWithId : BaseModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }
    }
}
