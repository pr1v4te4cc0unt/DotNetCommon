using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class Product
    {
        public int ID { get; set; }

        public int ProductID { get; set; }

        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public string LongDescription { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public decimal Tax { get; set; }

        public decimal Discount { get; set; }

        public string Category { get; set; }
    }
}
