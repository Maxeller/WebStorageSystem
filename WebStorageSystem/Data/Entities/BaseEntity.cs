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

        protected BaseEntity()
        {

        }

        protected BaseEntity(BaseEntity entity)
        {
            CreatedDate = entity.CreatedDate;
            ModifiedDate = entity.ModifiedDate;
            IsDeleted = entity.IsDeleted;
            RowVersion = entity.RowVersion;
        }
    }

    public class BaseEntityWithId : BaseEntity
    {
        public int Id { get; set; }

        protected BaseEntityWithId()
        {
        }

        protected BaseEntityWithId(BaseEntityWithId entityWithId) : base(entityWithId)
        {
            Id = entityWithId.Id;
        }
    }
}
