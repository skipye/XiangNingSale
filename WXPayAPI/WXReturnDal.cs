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
                    var Total = OrderTable.TotalPrice;
                    var MemberId = OrderTable.MemberId;
                    if (OrderTable != null) {
                        OrderTable.PayState = true;
                        OrderTable.PayTime = DateTime.Now;

                        var IsRus = db.WX_Order_Commission_Logs.Where(k => k.OrderId == OrderTable.Id).SingleOrDefault();
                        if (IsRus == null)//判断是否结佣过
                        {
                            var Memtable = db.MemberInfo.Where(k => k.Id == MemberId).SingleOrDefault();
                            var MemberUser = Memtable.userName;//购买人的昵称
                            if (Memtable != null)
                            {
                                var RequRequestNumber_1 = Memtable.RequestNumber_1;
                                var RequRequestNumber_2 = Memtable.RequestNumber_2;

                                if (string.IsNullOrEmpty(RequRequestNumber_2) == false)
                                {
                                    var RMemtable = db.MemberInfo.Where(k => k.MemberNumber == RequRequestNumber_1).FirstOrDefault();
                                    var NewRPrice = Total * Convert.ToDecimal(0.04);
                                    var ORprice = RMemtable.Commission ?? 0;
                                    RMemtable.Commission = ORprice + NewRPrice;

                                    WX_Order_Commission_Logs ComTab = new WX_Order_Commission_Logs();
                                    ComTab.Id = Guid.NewGuid();
                                    ComTab.MemberId = RMemtable.Id;
                                    ComTab.MemberName = MemberUser;
                                    ComTab.OrderNum = OrderNum;
                                    ComTab.RequstPay = Total * Convert.ToDecimal(0.04);
                                    ComTab.OrderPrice = Total;
                                    ComTab.OrderId = OrderTable.Id;
                                    ComTab.CreateTime = DateTime.Now;
                                    db.WX_Order_Commission_Logs.Add(ComTab);


                                    var R1Memtable = db.MemberInfo.Where(k => k.MemberNumber == RequRequestNumber_2).FirstOrDefault();
                                    var NewR1Price = Total * Convert.ToDecimal(0.01);
                                    var OR1price = RMemtable.Commission ?? 0;
                                    R1Memtable.Commission = OR1price + NewR1Price;

                                    WX_Order_Commission_Logs ComTab1 = new WX_Order_Commission_Logs();
                                    ComTab1.Id = Guid.NewGuid();
                                    ComTab1.MemberId = R1Memtable.Id;
                                    ComTab1.MemberName = MemberUser;
                                    ComTab1.OrderNum = OrderNum;
                                    ComTab1.RequstPay = Total * Convert.ToDecimal(0.01);
                                    ComTab1.OrderPrice = Total;
                                    ComTab1.OrderId = OrderTable.Id;
                                    ComTab1.CreateTime = DateTime.Now;
                                    db.WX_Order_Commission_Logs.Add(ComTab1);


                                }
                                if (string.IsNullOrEmpty(RequRequestNumber_1) == false && string.IsNullOrEmpty(RequRequestNumber_2) == true)
                                {
                                    var RMemtable = db.MemberInfo.Where(k => k.MemberNumber == RequRequestNumber_1).FirstOrDefault();
                                    var NewRPrice = Total * Convert.ToDecimal(0.05);
                                    var ORprice = RMemtable.Commission ?? 0;
                                    RMemtable.Commission = ORprice + NewRPrice;

                                    WX_Order_Commission_Logs ComTab = new WX_Order_Commission_Logs();
                                    ComTab.Id = Guid.NewGuid();
                                    ComTab.MemberId = RMemtable.Id;
                                    ComTab.MemberName = MemberUser;
                                    ComTab.OrderNum = OrderNum;
                                    ComTab.RequstPay = Total * Convert.ToDecimal(0.05);
                                    ComTab.OrderPrice = Total;
                                    ComTab.OrderId = OrderTable.Id;
                                    ComTab.CreateTime = DateTime.Now;
                                    db.WX_Order_Commission_Logs.Add(ComTab);

                                }
                            }
                        }

                    }
                }
                
                db.SaveChanges();
            }
        }
        public static void AddWX_Shar_Return(WX_Shar_ReturnModel models)
        {
            using (var db = new XiangNingSaleEntities())
            {
                WX_Shar_Return tables = new WX_Shar_Return();
                tables.Id = Guid.NewGuid();
                tables.ToUserName = models.ToUserName;
                tables.FromUserName = models.FromUserName;
                tables.CreateTime = DateTime.Now;
                tables.MsgType = models.MsgType;
                tables.Event = models.Event;
                tables.EventKey = models.EventKey;
                tables.Ticket = models.Ticket;
                tables.Remarks = models.Remarks;
                db.WX_Shar_Return.Add(tables);
                db.SaveChanges();
            }
        }

    }
}
