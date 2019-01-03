using ModelProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataBase;
using System.Web.Mvc;

namespace DalProject
{
    public class ContractHeaderDal
    {
        public List<ContractHeaderModel> GetPageList(SContractHeaderModel SModel)
        {
            DateTime StartTime = Convert.ToDateTime("1999-12-31");
            DateTime EndTime = Convert.ToDateTime("2999-12-31");
            if (!string.IsNullOrEmpty(SModel.StartTime))
            {
                StartTime = Convert.ToDateTime(SModel.StartTime);
            }
            if (!string.IsNullOrEmpty(SModel.EndTime))
            {
                EndTime = Convert.ToDateTime(SModel.EndTime).AddDays(1);
            }
            using (var db = new XiangNingSaleEntities())
            {
                var List = (from p in db.Sale_Contract_Header.Where(k => k.DeleteFlag == false)
                            where !string.IsNullOrEmpty(SModel.SN) ? p.SN.Contains(SModel.SN) : true
                            where SModel.CheckState == 1 ? p.Status == SModel.CheckState : true
                            where SModel.DepartmentId>0 ? p.SaleDepartmentId == SModel.DepartmentId : true
                            where !string.IsNullOrEmpty(SModel.UserName) ? p.Sale_Customers.Name.Contains(SModel.UserName) : true
                            where SModel.FR_flag >= 0 ? p.FRFlag == SModel.FR_flag : true
                            where p.CreateTime > StartTime
                            where p.CreateTime < EndTime
                            orderby p.CreateTime descending
                            select new ContractHeaderModel
                            {
                                Id = p.Id,
                                SN = p.SN,
                                Customer = p.Sale_Customers.Name,
                                CustomerId = p.CustomerId,
                                TelPhone = p.Sale_Customers.LinkTel,
                                delivery_date = p.delivery_date,
                                Amount = p.Amount,
                                Status = p.Status,
                                delivery_channel = p.delivery_channel,
                                FreightCarrier = p.FreightCarrier,
                                Prepay = p.Prepay,
                                MeasureFlag = p.MeasureFlag,
                                DeliveryAddress = p.DeliveryAddress,
                                SaleUserId = p.SaleUserId,
                                SaleUserName = p.SaleUserName,
                                SaleDepartmentId = p.SaleDepartmentId,
                                SaleDepartment = p.SaleDepartment,
                                CheckedUserName = p.CheckedUserName,
                                FRFlag = p.FRFlag,
                                OrderTime = p.HTDate,
                                CheckedTime=p.CheckedTime,
                                CreateTime = p.CreateTime,
                                CWCheckStatus=p.CWCheckStatus
                            }).ToList();
                
                return List;
            }
        }
        
        public void AddOrUpdate(ContractHeaderModel Models)
        {
            using (var db = new XiangNingSaleEntities())
            {
                if (Models.Id > 0)
                {
                    var table = db.Sale_Contract_Header.Where(k => k.Id == Models.Id).SingleOrDefault();
                    table.CustomerId = Models.CustomerId;
                    table.delivery_date = Models.delivery_date;
                    table.delivery_channel = Models.delivery_channel;
                    table.FreightCarrier = Models.FreightCarrier;
                    table.Prepay = Models.Prepay;
                    table.MeasureFlag = Models.MeasureFlag;
                    table.DeliveryAddress = Models.DeliveryAddress;
                    table.SaleUserId = Models.SaleUserId;
                    table.SaleUserName = Models.SaleUserName;
                    table.SaleDepartmentId = Models.SaleDepartmentId;
                    table.SaleDepartment = Models.SaleDepartment;
                    table.HTDate = Convert.ToDateTime(Models.HTDate);
                }
                else
                {
                    Sale_Contract_Header table = new Sale_Contract_Header();
                    table.SN = Models.SN;
                    table.CustomerId = Models.CustomerId;
                    table.delivery_date = Models.delivery_date;
                    table.delivery_channel = Models.delivery_channel;
                    table.Amount = 0;
                    table.AmountRecommend = 0;
                    table.FreightCarrier = Models.FreightCarrier;
                    table.Prepay = Models.Prepay;
                    table.MeasureFlag = Models.MeasureFlag;
                    table.DeliveryAddress = Models.DeliveryAddress;
                    table.SaleUserId = Models.SaleUserId;
                    table.SaleUserName = Models.SaleUserName;
                    table.SaleDepartmentId = Models.SaleDepartmentId;
                    table.SaleDepartment = Models.SaleDepartment;
                    table.Status = 0;
                    table.HTDate = Convert.ToDateTime(Models.HTDate);
                    table.FRFlag = 0;
                    table.CreateTime = DateTime.Now;
                    table.DeleteFlag = false;
                    table.CWCheckStatus = false;
                    db.Sale_Contract_Header.Add(table);
                }
                db.SaveChanges();

            }
        }

        public ContractHeaderModel GetDetailById(int Id)
        {
            using (var db = new XiangNingSaleEntities())
            {
                var tables = (from p in db.Sale_Contract_Header.Where(k => k.DeleteFlag == false && k.Id == Id)

                              orderby p.CreateTime descending
                              select new ContractHeaderModel
                              {
                                  Id = p.Id,
                                  SN = p.SN,
                                  Customer = p.Sale_Customers.Name,
                                  CustomerId = p.CustomerId,
                                  delivery_date = p.delivery_date,
                                  Amount = p.Amount,
                                  Status = p.Status,
                                  delivery_channel = p.delivery_channel,
                                  FreightCarrier = p.FreightCarrier,
                                  Prepay = p.Prepay,
                                  MeasureFlag = p.MeasureFlag,
                                  DeliveryAddress = p.DeliveryAddress,
                                  SaleUserId = p.SaleUserId,
                                  SaleUserName = p.SaleUserName,
                                  SaleDepartmentId = p.SaleDepartmentId,
                                  SaleDepartment = p.SaleDepartment,
                                  CheckedUserName = p.CheckedUserName,
                                  FRFlag = p.FRFlag,
                                  OrderTime = p.HTDate,
                                  CheckedTime = p.CheckedTime,
                              }).SingleOrDefault();
                return tables;
            }
        }
       
        public void Delete(string ListId)
        {
            using (var db = new XiangNingSaleEntities())
            {
                string[] ArrId = ListId.Split('$');
                foreach (var item in ArrId)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        int Id = Convert.ToInt32(item);
                        var tables = db.Sale_Contract_Header.Where(k => k.Id == Id).SingleOrDefault();
                        tables.DeleteFlag = true;
                    }
                }
                db.SaveChanges();
            }
        }
        public void Checked(string ListId)
        {
            using (var db = new XiangNingSaleEntities())
            {
                string[] ArrId = ListId.Split('$');
                foreach (var item in ArrId)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        int Id = Convert.ToInt32(item);
                        var tables = db.Sale_Contract_Header.Where(k => k.Id == Id).SingleOrDefault();
                        tables.Status = 1;
                        tables.CheckedUserId = new UserDal().GetCurrentUserName().UserId;
                        tables.CheckedUserName = new UserDal().GetCurrentUserName().UserName;
                        tables.CheckedTime = DateTime.Now;
                    }
                }

                db.SaveChanges();
            }
        }
        
        //根据日期获取合同总个数
        public int GetCRMHTCount(DateTime CreateTime)
        {
            using (var db = new XiangNingSaleEntities())
            {
                var List = db.Sale_Contract_Header.Where(k => k.CreateTime > CreateTime && k.DeleteFlag==false).Count();
                return List;
            }
        }
        public List<SelectListItem> GetProSNDrolist(int? pId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "请选择产品系列", Value = "" });
            using (var db = new XNERPEntities())
            {
                List<SYS_product_SN> model = db.SYS_product_SN.Where(b => b.delete_flag == false).OrderBy(k => k.created_time).ToList();
                foreach (var item in model)
                {
                    items.Add(new SelectListItem() { Text = "╋" + item.name, Value = item.id.ToString(), Selected = pId.HasValue && item.id.Equals(pId) });
                }
            }
            return items;
        }
        //产品区域
        public List<SelectListItem> GetProAreaDrolist(int? pId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "请选择产品区域", Value = "" });
            using (var db = new XNERPEntities())
            {
                List<SYS_product_area> model = db.SYS_product_area.Where(b => b.delete_flag == false).OrderBy(k => k.created_time).ToList();
                foreach (var item in model)
                {
                    items.Add(new SelectListItem() { Text = "╋" + item.name, Value = item.id.ToString(), Selected = pId.HasValue && item.id.Equals(pId) });
                }
            }
            return items;
        }
        public List<SelectListItem> GetWoodDrolist(int? pId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "请选择材质", Value = "" });
            using (var db = new XNERPEntities())
            {
                List<INV_wood_type> model = db.INV_wood_type.Where(b => b.delete_flag == false).OrderBy(k => k.created_time).ToList();
                foreach (var item in model)
                {
                    items.Add(new SelectListItem() { Text = "╋" + item.name, Value = item.id.ToString(), Selected = pId.HasValue && item.id.Equals(pId) });
                }
            }
            return items;
        }
        public List<SelectListItem> GetColorDrolist(int? pId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "请选择色号", Value = "" });
            using (var db = new XNERPEntities())
            {
                List<SYS_colors> model = db.SYS_colors.Where(b => b.delete_flag == false).OrderBy(k => k.created_time).ToList();
                foreach (var item in model)
                {
                    items.Add(new SelectListItem() { Text = "╋" + item.name, Value = item.id.ToString(), Selected = pId.HasValue && item.id.Equals(pId) });
                }
            }
            return items;
        }
        public string GetProNameDrolistBySNAndArea(int? ProSN, int? ProProArea)
        {
            using (var db = new XNERPEntities())
            {
                var list = (from p in db.SYS_product.Where(k => k.delete_flag == false)
                            where ProSN > 0 ? p.product_SN_id == ProSN : true
                            where ProProArea > 0 ? p.product_area_id == ProProArea : true
                            orderby p.created_time descending
                            select new CRMItem
                            {
                                Id = p.id,
                                Name = p.name,
                                length = p.length,
                                width = p.width,
                                height = p.height
                            }).ToList();
                string NewItme = "";
                foreach (var item in list)
                {
                    var strText = item.Name + "_" + item.length + "_" + item.width + "_" + item.height;
                    var IstrValue = item.Id;
                    NewItme += "<option value=" + IstrValue + ">" + strText + "</option>";
                }
                return NewItme;
            }
        }
        public void AddProducts(ContractProductsModel Models)
        {
            using (var db = new XiangNingSaleEntities())
            {
                decimal price = 0;
                if (Models.price > 0)
                { price = Models.price.Value; }

                Sale_Contract_Detail table = new Sale_Contract_Detail();
                table.SaleContractHeadId = Models.SaleContractHeadId;
                table.ProductId = Models.ProductId;
                table.ProductArea = Models.ProductArea;
                table.ProductSN = Models.ProductSN;
                table.ProductName = Models.ProductName;
                table.Color = Models.Color;
                table.WoodName = Models.WoodName;
                table.CustomFlag = Models.CustomFlag;
                table.length = Models.length;
                table.width = Models.width;
                table.height = Models.height;
                table.Price = price;
                table.price_recommend = 0;
                table.Qty = Models.qty;
                table.Unit = "件";
                table.hardware_part = Models.hardware_part;
                table.decoration_part = Models.decoration_part;
                table.req_others = Models.req_others;
                table.CreateTime = DateTime.Now;
                table.DeleteFlag = false;
                table.Status = 0;
                db.Sale_Contract_Detail.Add(table);
                
                var HeadTable = db.Sale_Contract_Header.Where(k => k.Id == Models.SaleContractHeadId).SingleOrDefault();
                HeadTable.Amount = HeadTable.Amount + price * Models.qty;
                db.SaveChanges();

            }
        }
        public void DeleteOne(int Id)
        {
            using (var db = new XiangNingSaleEntities())
            {
                var tables = db.Sale_Contract_Detail.Where(k => k.Id == Id).SingleOrDefault();
                tables.DeleteFlag = true;

                var HeadId = tables.SaleContractHeadId;
                var HeadTable = db.Sale_Contract_Header.Where(k => k.Id == HeadId).FirstOrDefault();
                HeadTable.Amount = HeadTable.Amount - tables.Qty * tables.Price.Value;

                db.SaveChanges();
            }
        }
        public List<ContractProductsModel> GetProductListByOrder(int HTId)
        {
            using (var db = new XiangNingSaleEntities())
            {
                var List = (from p in db.Sale_Contract_Detail.Where(k => k.DeleteFlag == false && k.SaleContractHeadId == HTId)
                            orderby p.CreateTime descending
                            select new ContractProductsModel
                            {
                                Id = p.Id,
                                SaleContractHeadId = p.SaleContractHeadId,
                                ProductName = p.ProductName,
                                ProductSN = p.ProductSN,
                                Color = p.Color,
                                WoodName = p.WoodName,
                                CustomFlag = p.CustomFlag,
                                length = p.length,
                                width = p.width,
                                height = p.height,
                                price = p.Price,
                                qty = p.Qty,
                                hardware_part = p.hardware_part,
                                decoration_part = p.decoration_part,
                                req_others = p.req_others,
                                status = p.Sale_Contract_Header.Status,
                            }).ToList();
                return List;
            }
        }
    }
}
