using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelProject
{
   
    //微信销售合同
    public class WXOrderModel
    {
        public int Id { get; set; }
        public string Ordernum { get; set; }
        public Guid? CustomerId { get; set; }
        public string Customer { get; set; }
        public string TelPhone { get; set; }
        public decimal? TotalPrice { get; set; }//合同总金额
        public decimal? SubtractPrice { get; set; }//减免金额
        public decimal? YunFei { get; set; }//运费
        public bool? PayState { get; set; }//付款状态
        public string DeliveryAddress { get; set; }//送货地址
        public string DeliveryName { get; set; }
        public DateTime? CreateTime { get; set; }
        public string Remarks { get; set; }
        public IEnumerable<OrderProductsModels> OrderProductList { get; set; }
    }
    public class SWXOrderModel
    {
        public int? FR_flag { get; set; }
        public string SN { get; set; }
        public int CheckState { get; set; }
        public string UserName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int status { get; set; }
        public int FRstatus { get; set; }
        public int? DepartmentId { get; set; }
        public int? SaleUserId { get; set; }
    }
    public class WXOrderDataModel
    {
        public List<WXOrderModel> data { get; set; }
        public decimal? HTTotail { get; set; }
    }
}
