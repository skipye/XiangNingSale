using DataBase;
using ModelProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace DalProject
{
    public class PurchaseOrderDal
    {
        public List<PurchaseOrderModel> GetPageList(SPurchaseOrderModel SModel)
        {
            DateTime StartTime = Convert.ToDateTime("1900-01-01");
            DateTime EndTime = Convert.ToDateTime("2900-12-30");
            if (!string.IsNullOrEmpty(SModel.StartTime))
            {
                StartTime = Convert.ToDateTime(SModel.StartTime);
            }
            if (!string.IsNullOrEmpty(SModel.EndTime))
            {
                EndTime = Convert.ToDateTime(SModel.EndTime);
            }
            using (var db = new XiangNingSaleEntities())
            {
                var List = (from p in db.Purchase_Order.Where(k => k.DeleteFlag == false)
                            where !string.IsNullOrEmpty(SModel.Name) ? p.Name.Contains(SModel.Name) : true
                            where SModel.CheckedStatus >= 0 ? p.CheckedStatus == SModel.CheckedStatus : true
                            where SModel.ApplyUserId > 0 ? p.ApplyUserId == SModel.ApplyUserId : true
                            where p.CreateTime >= StartTime
                            where p.CreateTime < EndTime
                            orderby p.CreateTime
                            select new PurchaseOrderModel
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Remark = p.Remark,
                                ApplyDateTime = p.ApplyDateTime,
                                ApplyUserName = p.ApplyUserName,
                                Qty = p.Qty,
                                SN = p.SN,
                                CheckedStatus = p.CheckedStatus,
                                CheckedName = p.CheckedName,
                                CheckedDateTime = p.CheckedDateTime,
                                Price = p.Price,
                            }).ToList();
                return List;
            }
        }
       
        public void AddOrUpdate(PurchaseOrderModel Models)
        {
            using (var db = new XiangNingSaleEntities())
            {
                if (Models.Id > 0)
                {
                    var table = db.Purchase_Order.Where(k => k.Id == Models.Id).SingleOrDefault();
                    table.Qty = Models.Qty;
                    table.Name = Models.Name;
                    table.Remark = Models.Remark;
                    table.Price = Models.Price;

                }
                else
                {
                    Purchase_Order table = new Purchase_Order();
                    table.SN = "PO" + getNo();
                    table.Qty = Models.Qty;
                    table.Name = Models.Name;
                    table.Remark = Models.Remark;
                    table.Price = Models.Price;
                    table.ApplyUserName = Models.ApplyUserName;
                    table.ApplyUserId = Models.ApplyUserId;
                    table.ApplyDateTime = DateTime.Now;
                    table.CheckedStatus = 0;
                    table.CreateTime = DateTime.Now;
                    table.DeleteFlag = false;
                    db.Purchase_Order.Add(table);
                }
                db.SaveChanges();
            }
        }
        public PurchaseOrderModel GetDetailById(int Id)
        {
            using (var db = new XiangNingSaleEntities())
            {
                var tables = (from p in db.Purchase_Order.Where(k => k.Id == Id)
                              select new PurchaseOrderModel
                              {
                                  Id = p.Id,
                                  Name = p.Name,
                                  Remark = p.Remark,
                                  Qty = p.Qty,
                                  Price = p.Price,
                                  ApplyUserId=p.ApplyUserId,
                                  ApplyUserName = p.ApplyUserName,
                              }).SingleOrDefault();
                return tables;
            }
        }

        public void DeleteMore(string ListId)
        {
            using (var db = new XiangNingSaleEntities())
            {
                string[] ArrId = ListId.Split('$');
                foreach (var item in ArrId)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        int Id = Convert.ToInt32(item);
                        var tables = db.Purchase_Order.Where(k => k.Id == Id).SingleOrDefault();
                        tables.DeleteFlag = true;
                    }
                }
                db.SaveChanges();
            }
        }
        public static String getNo()
        {
            Random r = new Random();
            DateTime Now = DateTime.Now;
            return Now.ToString("yyyyMMdd") + r.Next(10000, 100000); ;
        }
        public void CheckedMore(string ListId, int CheckedId, int UserId,string UserName)
        {
            using (var db = new XiangNingSaleEntities())
            {
                string[] ArrId = ListId.Split('$');
                foreach (var item in ArrId)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        int Id = Convert.ToInt32(item);
                        var tables = db.Purchase_Order.Where(k => k.Id == Id).SingleOrDefault();
                        if (tables.CheckedStatus == 0)
                        {
                            tables.CheckedStatus = CheckedId;
                            tables.CheckedUserId = UserId;
                            tables.CheckedName = UserName;
                        }
                    }
                }
                db.SaveChanges();
            }
        }
    }
}
