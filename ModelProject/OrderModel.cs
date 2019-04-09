using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelProject
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string Ordernum { get; set; }
        public int? AddressId { get; set; }
        public Guid? MemberId { get; set; }
        public long? ProductsId { get; set; }
        public int? DKGold { get; set; }
        public int? DKStock { get; set; }
        public Decimal? DKPrce { get; set; }
        public Decimal? YunFei { get; set; }
        public Decimal? TotalPrice { get; set; }
        public bool? PayState { get; set; }
        public List<CartModel> Carts { get; set; }
        public bool TimeOut { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ProductName { get; set; }
        public string openid { get; set; }
        public Decimal? SubtractPrice { get; set; }
        public IEnumerable<OrderProductsModels> OrderProductList { get; set; }
        public string Remarks { get; set; }
        public string YunDan { get; set; }
        public int? YState { get; set; }
        public string YSCompany { get; set; }
        //public SodModel SodModels { get; set; }
        public AddressModel Addreess { get; set; }
    }
    public class OrderProductsModels
    {
        public string Ordernum { get; set; }
        public int? ProductsId { get; set; }
        public int? Amount { get; set; }
        public Decimal? Saleprice { get; set; }
        public string ProductsName { get; set; }
        public string ProductsConvertImg { get; set; }

    }
}
