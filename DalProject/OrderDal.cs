using DataBase;
using ModelProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DalProject
{
    public class OrderDal
    {
        public void AddOrder(OrderModel models)
        {
            using (var db = new XiangNingSaleEntities())
            {
                if (models.Carts != null)
                {
                    var tables = new OrderInfo();
                    tables.Ordernum = models.Ordernum;
                    tables.AddressId = models.AddressId;
                    tables.MemberId = models.MemberId;
                    tables.DKGold = models.DKGold;
                    tables.DKStock = models.DKStock;
                    tables.DKPrce = models.DKPrce;
                    tables.YunFei = models.YunFei;
                    tables.CreateTime = DateTime.Now;
                    tables.PayState = false;
                    tables.State = true;
                    tables.TotalPrice = models.TotalPrice;
                    tables.SubtractPrice = 0;
                    tables.Remarks = models.Remarks;
                    db.OrderInfo.Add(tables);
                    OrderProductsModels OPModels = new OrderProductsModels();
                    foreach (var item in models.Carts)
                    {
                        OPModels.Ordernum = models.Ordernum;
                        OPModels.ProductsId = item.Product.Id;
                        OPModels.Amount = item.Amount;
                        OPModels.Saleprice = Convert.ToDecimal(item.Product.SalePrice);
                        OPModels.ProductsName = item.Product.Name;
                        OPModels.ProductsConvertImg = item.Product.ConvertImg;
                        AddOrderProducts(db, OPModels);

                        using (var prdb = new XNArticleEntities())
                        {
                            var proTab = prdb.A_News.Where(k => k.Id == item.Product.Id).SingleOrDefault();
                            if (proTab != null)
                            {
                                proTab.SaleCount = proTab.SaleCount ?? 0 + item.Amount;
                            }
                            prdb.SaveChanges();
                        }
                        
                        
                    }
                    db.SaveChanges();
                }
            }
        }
        
        public void AddOrderProducts(XiangNingSaleEntities db, OrderProductsModels OPModels)
        {
            var PrTables = new OrderProductsInfo();
            PrTables.Id = Guid.NewGuid();
            PrTables.Ordernum = OPModels.Ordernum;
            PrTables.ProductsId = OPModels.ProductsId;
            PrTables.Amount = OPModels.Amount;
            PrTables.SalePrice = OPModels.Saleprice;
            PrTables.ProductsName = OPModels.ProductsName;
            PrTables.ProductsConvertImg = OPModels.ProductsConvertImg;
            db.OrderProductsInfo.Add(PrTables);
        }
        public List<OrderModel> GetOrderList(string KeyWord, Guid MemberId, bool? IsTimeOut, int PageSize, int PageIndex, bool? PayState)
        {
            DateTime TimeNow = DateTime.Now;
            string OrderNum = "";
           
            using (var db = new XiangNingSaleEntities())
            {
                var orderlist = (from p in db.OrderInfo.Where(k => k.MemberId == MemberId && k.State == true)
                                 where !string.IsNullOrEmpty(OrderNum) ? p.Ordernum.Contains(OrderNum) : true
                                 where PayState != null ? p.PayState == PayState : true
                                 orderby p.CreateTime descending
                                 select new OrderModel
                                 {
                                     Id = p.Id,
                                     Ordernum = p.Ordernum,
                                     CreateTime = p.CreateTime,
                                     TotalPrice = p.TotalPrice,
                                     YunFei = p.YunFei,
                                     DKPrce = p.DKPrce,
                                     PayState = p.PayState,
                                     AddressId = p.AddressId,
                                     Addreess = new AddressModel { Id = p.Address_Info.Id, Name = p.Address_Info.Name, Telphone = p.Address_Info.Telphone, Province = p.Address_Info.Province, City = p.Address_Info.City, Region = p.Address_Info.Region, addressNo = p.Address_Info.addressNo },
                                     YunDan = p.YunDan,
                                     YState = p.YState,
                                     SubtractPrice = p.SubtractPrice,
                                     YSCompany = p.YSCompany
                                 }).ToList();
                if (orderlist != null && orderlist.Any())
                {
                    foreach (var item in orderlist)
                    {
                        var TimeOutTime = item.CreateTime.Value.AddDays(1);
                        if (TimeOutTime > TimeNow)
                        { item.TimeOut = true; }
                        else { item.TimeOut = false; }
                        item.OrderProductList = (from m in db.OrderProductsInfo.Where(k => k.Ordernum == item.Ordernum)
                                                 select new OrderProductsModels
                                                 {
                                                     ProductsId = m.ProductsId,
                                                     ProductsName = m.ProductsName,
                                                     ProductsConvertImg = m.ProductsConvertImg,
                                                     Saleprice = m.SalePrice,
                                                     Amount = m.Amount
                                                 }).ToList();
                    }
                }
                if (IsTimeOut != null && IsTimeOut == true)
                {
                    return orderlist.Where(k => k.TimeOut == true).Skip(PageIndex * PageSize).Take(PageSize).ToList();
                }
                else { return orderlist.Skip(PageIndex * PageSize).Take(PageSize).ToList(); }

            }
        }
        //
        public OrderModel GetOrderDetail(string OrderId)
        {
            using (var db = new XiangNingSaleEntities())
            {
                var tabel = (from p in db.OrderInfo.Where(k => k.Ordernum == OrderId)
                             select new OrderModel
                             {
                                 Ordernum = p.Ordernum,
                                 TotalPrice = p.TotalPrice,
                                 SubtractPrice = p.SubtractPrice
                             }).FirstOrDefault();
                return tabel;
            }
        }
       
        public void DelOrderById(int Id)
        {
            using (var db = new XiangNingSaleEntities())
            {
                var tables = db.OrderInfo.Where(k => k.Id == Id).SingleOrDefault();
                tables.State = false;
                db.SaveChanges();
            }
        }
        public void UpdateOrder(string OrderNumber)
        {
            using (var db = new XiangNingSaleEntities())
            {
                var tables = db.OrderInfo.Where(k => k.Ordernum == OrderNumber).SingleOrDefault();
                tables.PayState = true;
                var Usertables = db.MemberInfo.Where(k => k.Id == tables.MemberId).SingleOrDefault();
                Usertables.Gold = Usertables.Gold - tables.DKGold;
                Usertables.ZStock = Usertables.ZStock - tables.DKStock;
                
                db.SaveChanges();
            }
        }
        
        public List<OrderModel> GetReturnOrderList(Guid MemberId, int PageSize, int PageIndex)
        {

            using (var db = new XiangNingSaleEntities())
            {
                var orderlist = (from p in db.OrderInfo.Where(k => k.MemberId == MemberId && k.State == true && k.PayState == true)
                                 orderby p.CreateTime descending
                                 select new OrderModel
                                 {
                                     Id = p.Id,
                                     Ordernum = p.Ordernum,
                                     CreateTime = p.CreateTime,
                                     TotalPrice = p.TotalPrice,
                                     YunFei = p.YunFei,
                                     DKPrce = p.DKPrce,
                                     PayState = p.PayState,
                                     AddressId = p.AddressId,
                                     YunDan = p.YunDan,
                                     YState = p.YState,
                                     YSCompany = p.YSCompany,
                                     OrderProductList = (from m in db.OrderProductsInfo.Where(k => k.Ordernum == p.Ordernum)
                                                         select new OrderProductsModels
                                                         {
                                                             ProductsId = m.ProductsId,
                                                             ProductsName = m.ProductsName,
                                                             ProductsConvertImg = m.ProductsConvertImg,
                                                             Saleprice = m.SalePrice,
                                                             Amount = m.Amount
                                                         })
                                 }).ToList();

                return orderlist.Skip(PageIndex * PageSize).Take(PageSize).ToList();

            }
        }
        
    }
}
