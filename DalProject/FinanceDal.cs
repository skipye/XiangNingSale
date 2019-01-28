using DataBase;
using ModelProject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DalProject
{
    public class FinanceDal
    {
        
        public void AddOrUpdate(FinanceModel Models)
        {
            using (var db = new XiangNingSaleEntities())
            {
                var OrderTable = db.Sale_Contract_Header.Where(k => k.Id == Models.Id).FirstOrDefault();
                var Total = OrderTable.Amount;
                var Prepay = OrderTable.Prepay;
                var Pay = Models.Amount;
                //添加付款操作日志
                Sale_Finance_FR_Logs SFLtable = new Sale_Finance_FR_Logs();
                SFLtable.HTId = Models.Id;
                SFLtable.HTSN = OrderTable.SN;
                SFLtable.Customer = OrderTable.Sale_Customers.Name;
                SFLtable.Amount = Models.Amount;
                SFLtable.operator_id = Models.operator_id;
                SFLtable.operator_name = Models.operator_name;
                SFLtable.CreateTime = DateTime.Now;
                SFLtable.Remaks = Models.Remaks;
                db.Sale_Finance_FR_Logs.Add(SFLtable);

                var table = db.Sale_Finance_FR.Where(k => k.Sale_Contract_HeaderId == Models.Id).SingleOrDefault();//判断是否存在
                if (table != null && table.Id > 0)
                {
                    table.Amount = Models.Amount + table.Amount;
                    table.operator_id = Models.operator_id;
                    table.operator_name = Models.operator_name;

                    Pay = table.Amount;
                }
                else
                {
                    Sale_Finance_FR Stable = new Sale_Finance_FR();
                    Stable.Sale_Contract_HeaderId = Models.Id;
                    Stable.Sale_Contract_HeaderSN = OrderTable.SN;
                    Stable.Customer = OrderTable.Sale_Customers.Name;
                    Stable.Total = Total;
                    Stable.Prepay = Prepay;
                    Stable.Amount = Models.Amount;
                    Stable.operator_id = Models.operator_id;
                    Stable.operator_name = Models.operator_name;
                    Stable.CreateTime = DateTime.Now;
                    db.Sale_Finance_FR.Add(Stable);
                }
                if (Models.Amount > 0 && Pay >= Prepay && Pay < Total)//更新合同的付款状态
                {
                    OrderTable.FRFlag = 1;
                    
                }
                if (Pay >= Total)
                { OrderTable.FRFlag = 2; }


                db.SaveChanges();
            }
        }
        public void AddOrder(int Id)
        {
            ContractHeaderModel CHModels = new ContractHeaderModel();
            List<ContractProductsModel> CHDetailModels = new List<ContractProductsModel>();
            using (var db = new XiangNingSaleEntities())
            {
                var CHTabl = (from p in db.Sale_Contract_Header.Where(k => k.Id == Id)
                              select new ContractHeaderModel
                              {
                                  SN = p.SN,
                                  OrderTime = p.HTDate,
                                  delivery_date =p.delivery_date,
                                  delivery_channel=p.delivery_channel,
                                  FreightCarrier=p.FreightCarrier,
                                  MeasureFlag=p.MeasureFlag,
                                  DeliveryAddress=p.DeliveryAddress,
                                  Customer=p.Sale_Customers.Name,
                                  TelPhone=p.Sale_Customers.LinkTel
                              }).SingleOrDefault();
                CHModels = CHTabl;

                var CHDetail = (from p in db.Sale_Contract_Detail.Where(k => k.SaleContractHeadId == Id)
                                orderby p.CreateTime descending
                                select new ContractProductsModel
                                {
                                    ColorId = p.ColorId,
                                    Color = p.Color,
                                    ProductId = p.ProductId,
                                    WoodId = p.WoodId,
                                    CustomFlag = p.CustomFlag,
                                    length = p.length,
                                    width = p.width,
                                    height = p.height,
                                    price = p.Price,
                                    qty = p.Qty,
                                    hardware_part = p.hardware_part,
                                    decoration_part = p.decoration_part,
                                    req_others = p.req_others,
                                }).ToList();
                CHDetailModels = CHDetail;
            }

            using (var db = new XNERPEntities())
            {
                CRM_contract_header table = new CRM_contract_header();
                table.SN = CHModels.SN;
                table.HTDate = Convert.ToDateTime(CHModels.OrderTime);
                table.customer_id = 213;
                table.delivery_date = CHModels.delivery_date;
                table.amount = 0;
                table.delivery_channel = CHModels.delivery_channel;
                table.freight_carrier = CHModels.FreightCarrier;
                table.prepay = 0;
                table.measure_flag = CHModels.MeasureFlag;
                table.delivery_address = CHModels.DeliveryAddress;
                table.Linkman = CHModels.Customer;
                table.Linktel = CHModels.TelPhone;
                table.signed_user_id = 0;
                table.signed_department_id = 0;
                table.department = "唐锐";
                table.SaleName = "唐锐";
                table.status = 0;
                table.created_time = DateTime.Now;
                table.delete_flag = false;
                db.CRM_contract_header.Add(table);
                db.SaveChanges();
                var HeaderId = table.id;
                if (CHDetailModels!=null && CHDetailModels.Any())
                {
                    foreach (var item in CHDetailModels)
                    {
                        CRM_contract_detail Detailtable = new CRM_contract_detail();
                        Detailtable.header_id = HeaderId;
                        Detailtable.color_id = item.ColorId??0;
                        Detailtable.color = item.Color;
                        Detailtable.product_id = item.ProductId;
                        Detailtable.wood_type_id = item.WoodId??0;
                        Detailtable.custom_flag = item.CustomFlag;
                        Detailtable.length = item.length;
                        Detailtable.width = item.width;
                        Detailtable.height = item.height;
                        Detailtable.price = 0;
                        Detailtable.qty = item.qty??0;
                        Detailtable.hardware_part = item.hardware_part;
                        Detailtable.decoration_part = item.decoration_part;
                        Detailtable.req_others = item.req_others;
                        Detailtable.created_time = DateTime.Now;
                        Detailtable.delete_flag = false;
                        Detailtable.status = 0;
                        db.CRM_contract_detail.Add(Detailtable);
                    }
                    
                }
                db.SaveChanges();
            }

        }
    }
}
