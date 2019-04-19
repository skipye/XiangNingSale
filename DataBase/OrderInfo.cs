//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataBase
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderInfo
    {
        public int Id { get; set; }
        public string Ordernum { get; set; }
        public Nullable<int> AddressId { get; set; }
        public Nullable<decimal> YunFei { get; set; }
        public Nullable<bool> PayState { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public Nullable<decimal> SubtractPrice { get; set; }
        public string Remarks { get; set; }
        public string YunDan { get; set; }
        public Nullable<int> YState { get; set; }
        public string YSCompany { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<bool> State { get; set; }
        public Nullable<System.Guid> MemberId { get; set; }
        public Nullable<int> DKGold { get; set; }
        public Nullable<int> DKStock { get; set; }
        public Nullable<decimal> DKPrce { get; set; }
        public Nullable<System.DateTime> PayTime { get; set; }
    
        public virtual Address_Info Address_Info { get; set; }
        public virtual MemberInfo MemberInfo { get; set; }
        public virtual OrderInfo OrderInfo1 { get; set; }
        public virtual OrderInfo OrderInfo2 { get; set; }
    }
}