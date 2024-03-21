using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        [DisplayName("Tên Sản Phẩm")]

        public string Title { get; set; }

        public decimal? Price { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public int Quantity { get; set; }

        public decimal TotalMoney
        {
            get
            {
                return (decimal)(Quantity * Price);
            }
        }
    }
}