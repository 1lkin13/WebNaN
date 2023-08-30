using System;
using System.Collections.Generic;

namespace WebNaN.Models
{
    public partial class Product
    {
        public Product()
        {
            Photos = new HashSet<Photo>();
        }

        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductInfo { get; set; }
        public int? ProductCategoryId { get; set; }

        public virtual Category? ProductCategory { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
    }
}
