using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ModelProject
{
    public class CartModel
    {
        [DisplayName("选购商品")]
        [Required]
        public NewsModel Product { get; set; }

        [DisplayName("选购数量")]
        [Required]
        public int Amount { get; set; }
    }
    public class CartAddressModel
    {
        public List<CartModel> CartModel { get; set; }
        public AddressModel AddModel { get; set; }
        //public UserModel UserModel { get; set; }
        public double ShangPinTolit { get; set; }
        public double YLGole { get; set; }
        public double YLStock { get; set; }
    }
}
