using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModelProject;
using DataBase;

namespace WxPayAPI
{
    public class WXReturnDal
    {
        public void AddWXReturnInfo(WXReturnResultModel models)
        {
            using (var db = new XiangNingSaleEntities())
            {
                var tables = new WXReturnInfo();
                tables.Id = Guid.NewGuid();
                tables.out_trade_no = models.out_trade_no;
                tables.transaction_id = models.transaction_id;
                tables.openid = models.openid;
                tables.trade_type = models.trade_type;
                tables.bank_type = models.bank_type;
                tables.total_fee = models.total_fee;
                tables.fee_type = models.fee_type;
                tables.time_end = models.time_end;
                tables.CreateTime = DateTime.Now;
                db.WXReturnInfo.Add(tables);

                var OrderNum = models.out_trade_no;
                if (models.out_trade_no.Contains("abc") == true)
                {
                    OrderNum = models.out_trade_no.Split('a')[0];
                }
                if (OrderNum.Contains("XN"))
                {
                    var OrderTable = db.OrderInfo.Where(k => k.Ordernum == OrderNum).SingleOrDefault();
                    if (OrderTable != null) {
                        OrderTable.PayState = true;
                        OrderTable.PayTime = DateTime.Now;
                    }
                }
                
                db.SaveChanges();
            }
        }
    }
}
