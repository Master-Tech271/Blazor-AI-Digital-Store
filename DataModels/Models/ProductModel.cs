using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Models
{
    public class ProductModel
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; } = 0;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
    }
}
