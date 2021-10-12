using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStorageSystem.Areas.Products.Data.Entities;

namespace WebStorageSystem.Data.Entities
{
    public class ImageEntity : BaseEntityWithId
    {
        public string Title { get; set; }

        public string ImageName { get; set; }

        public IEnumerable<Product> Products { get; set; }

        public override DateTime CreatedDate { get; set; }
        public override DateTime ModifiedDate { get; set; }
        public override bool IsDeleted { get; set; }
        public override byte[] RowVersion { get; set; }
        public override int Id { get; set; }
    }
}
