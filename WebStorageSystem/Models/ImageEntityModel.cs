using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using WebStorageSystem.Areas.Defects.Models;
using WebStorageSystem.Areas.Products.Models;

namespace WebStorageSystem.Models
{
    public class ImageEntityModel : BaseEntityModelWithId
    {
        [DisplayName("Image Title")]
        public string Title { get; set; }

        public string ImageName { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }

        public IEnumerable<ProductModel> Products { get; set; }

        public IEnumerable<DefectModel> Defects { get; set; }

        public override DateTime CreatedDate { get; set; }
        public override DateTime ModifiedDate { get; set; }
        public override bool IsDeleted { get; set; }
        public override byte[] RowVersion { get; set; }
        public override Dictionary<string, string> Action { get; set; }
        public override int Id { get; set; }
    }
}
