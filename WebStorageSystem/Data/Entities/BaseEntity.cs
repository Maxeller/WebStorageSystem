using System;
using System.ComponentModel.DataAnnotations;

namespace WebStorageSystem.Data.Entities
{
    public class BaseEntity
    {
        [Display(Name = "Created")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Last Modification")]
        public DateTime ModifiedDate { get; set; }
        [Display(Name = "Deleted")]
        public bool IsDeleted { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }

    public class BaseEntityWithId : BaseEntity
    {
        public int Id { get; set; }
    }
}
