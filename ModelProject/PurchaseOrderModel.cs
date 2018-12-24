using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelProject
{
    public class PurchaseOrderModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SN { get; set; }
        public int? ApplyUserId { get; set; }
        public string ApplyUserName { get; set; }
        public DateTime? ApplyDateTime { get; set; }
        public int? Qty { get; set; }
        public int? CheckedStatus { get; set; }
        public int? CheckedUserId { get; set; }
        public string CheckedName { get; set; }
        public DateTime? CheckedDateTime { get; set; }
        public decimal? Price { get; set; }
        public string Remark { get; set; }
    }
    public class SPurchaseOrderModel
    {
        public int Id { get; set; }
        public int? CheckedStatus { get; set; }
        public string Name { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int? ApplyUserId { get; set; }
    }
}
