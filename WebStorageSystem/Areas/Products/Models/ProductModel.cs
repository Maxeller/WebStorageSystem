using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebStorageSystem.Models;

namespace WebStorageSystem.Areas.Products.Models
{
    public class ProductModel : BaseEntityModelWithId
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(100), Display(Name = "Product Number")]
        public string ProductNumber { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(200)]
        public string Webpage { get; set; }

        [Required, Display(Name = "Product Type")]
        public int ProductTypeId { get; set; }

        [Display(Name = "Product Type")]
        public ProductTypeModel ProductType { get; set; }

        [Required, Display(Name = "Manufacturer")]
        public int ManufacturerId { get; set; }

        public ManufacturerModel Manufacturer { get; set; }
       
        /* //TODO FINISH IMAGE
        [Required]
        public ImageEntityModel Image { get; set; }

        public int? ImageId { get; set; }
        */

        [JsonIgnore]
        public IEnumerable<UnitModel> Units { get; set; }

        [Display(Name = "Creation Date")]
        public override DateTime CreatedDate { get; set; }

        [Display(Name = "Last Modification")]
        public override DateTime ModifiedDate { get; set; }

        [Display(Name = "Deleted")]
        public override bool IsDeleted { get; set; }
        public override byte[] RowVersion { get; set; }
        public override Dictionary<string, string> Action { get; set; }
        public override int Id { get; set; }
    }
}
