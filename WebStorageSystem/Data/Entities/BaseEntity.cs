using System;
using System.ComponentModel.DataAnnotations;

namespace WebStorageSystem.Data.Entities
{
    public abstract class BaseEntity
    {
        public abstract DateTime CreatedDate { get; set; }

        public abstract DateTime ModifiedDate { get; set; }

        public abstract bool IsDeleted { get; set; }

        [Timestamp]
        public abstract byte[] RowVersion { get; set; }
    }

    public abstract class BaseEntityWithId : BaseEntity
    {
        public abstract int Id { get; set; }
    }
}
