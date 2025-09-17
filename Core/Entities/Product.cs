using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Product:BaseEntity
    {
        public required string Title { get; set; }
        public required string Description  { get; set; }
        public required string  ProductImage { get; set; }
        public decimal Price { get; set; }
        public int AvailableInStock { get; set; }
        public required  string Type { get; set; }
        public required string Brand  { get; set; }
    }
}
