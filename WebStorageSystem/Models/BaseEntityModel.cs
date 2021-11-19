using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace WebStorageSystem.Models
{
    public abstract class BaseEntityModel
    {
        [Display(Name = "Creation Date")]
        public abstract DateTime CreatedDate { get; set; }

        [Display(Name = "Last Modification")]
        public abstract DateTime ModifiedDate { get; set; }

        [Display(Name = "Deleted")]
        public abstract bool IsDeleted { get; set; }

        [Timestamp]
        public abstract byte[] RowVersion { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), XmlIgnore]
        public abstract Dictionary<string, string> Action { get; set; }
    }

    public abstract class BaseEntityModelWithId : BaseEntityModel
    {
        public abstract int Id { get; set; }
    }
}
