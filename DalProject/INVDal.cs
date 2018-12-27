using DataBase;
using ModelProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace DalProject
{
    public class INVDal
    {
        public List<LabelsModel> GetLabelsList(SLabelsModel SModel)
        {
            DateTime StartTime = Convert.ToDateTime("1999-12-31");
            DateTime EndTime = Convert.ToDateTime("2999-12-31");
            if (!string.IsNullOrEmpty(SModel.StartTime))
            {
                StartTime = Convert.ToDateTime(SModel.StartTime).AddDays(-1);
            }
            if (!string.IsNullOrEmpty(SModel.EndTime))
            {
                EndTime = Convert.ToDateTime(SModel.EndTime).AddDays(1);
            }
            using (var db = new XNERPEntities())
            {
                var List = (from p in db.INV_labels.Where(k => k.delete_flag == false && k.flag > 0 && k.status == 1)
                            where SModel.product_SN_id != null && SModel.product_SN_id > 0 ? SModel.product_SN_id == p.SYS_product.product_SN_id : true
                            where SModel.product_area_id != null && SModel.product_area_id > 0 ? SModel.product_area_id == p.SYS_product.product_area_id : true
                            where SModel.wood_id > 0 ? SModel.wood_id == p.wood_id : true
                            where SModel.inv_id > 0 ? SModel.inv_id == p.inv_id : true
                            where !string.IsNullOrEmpty(SModel.productName) ? p.SYS_product.name.Contains(SModel.productName) : true
                            where p.created_time > StartTime
                            where p.created_time < EndTime
                            orderby p.created_time descending
                            select new LabelsModel
                            {
                                id = p.id,
                                product_id = p.product_id,
                                ProductName = p.SYS_product.name,
                                ProductXL = p.SYS_product.SYS_product_SN.name,
                                wood_id = p.wood_id,
                                woodname = p.INV_wood_type.name,
                                inv_id = p.inv_id,
                                invname = p.INV_inventories.name,
                                input_date = p.created_time,
                                SN = p.SN,
                                product_SN_Name = p.product_SN,
                                color = p.color,
                                style = p.style,
                            }).ToList();
                return List;
            }
        }
        public List<SemiModel> GetSemiList(SSemiModel SModel)
        {
            DateTime StartTime = Convert.ToDateTime("1999-12-31");
            DateTime EndTime = Convert.ToDateTime("2999-12-31");
            if (!string.IsNullOrEmpty(SModel.StartTime))
            {
                StartTime = Convert.ToDateTime(SModel.StartTime).AddDays(-1);
            }
            if (!string.IsNullOrEmpty(SModel.EndTime))
            {
                EndTime = Convert.ToDateTime(SModel.EndTime).AddDays(1);
            }
            using (var db = new XNERPEntities())
            {
                var List = (from p in db.INV_semi.Where(k => k.delete_flag == false && k.WIP_id > 0 && k.status==1)
                            where SModel.product_SN_id != null && SModel.product_SN_id > 0 ? SModel.product_SN_id == p.SYS_product.product_SN_id : true
                            where SModel.product_area_id != null && SModel.product_area_id > 0 ? SModel.product_area_id == p.SYS_product.product_area_id : true
                            where SModel.wood_id > 0 ? SModel.wood_id == p.wood_id : true
                            where SModel.inv_id > 0 ? SModel.inv_id == p.inv_id : true
                            where !string.IsNullOrEmpty(SModel.productName) ? p.SYS_product.name.Contains(SModel.productName) : true
                            where p.created_time > StartTime
                            where p.created_time < EndTime
                            orderby p.created_time descending
                            select new SemiModel
                            {
                                id = p.id,
                                productName = p.SYS_product.name,
                                ProductXL = p.SYS_product.SYS_product_SN.name,
                                woodname = p.INV_wood_type.name,
                                inv_id = p.inv_id,
                                invname = p.INV_inventories.name,
                                areaName = p.SYS_product.SYS_product_area.name,
                                input_date = p.created_time,
                                length = p.length,
                                width = p.width,
                                height = p.height
                            }).ToList();
                return List;
            }
        }
        public List<SelectListItem> GetXLDrolist(int? pId)
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
        public List<SelectListItem> GetAreaDrolist(int? pId)
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
        public List<SelectListItem> GetCKDrolist(int? pId, int? CKType)
        {

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "请选择仓库", Value = "" });
            using (var db = new XNERPEntities())
            {
                List<INV_inventories> model = db.INV_inventories.Where(b => b.delete_flag == false).OrderBy(k => k.created_time).ToList();
                if (CKType != null && CKType > 0)
                {
                    model = db.INV_inventories.Where(b => b.delete_flag == false && b.type == CKType).OrderBy(k => k.created_time).ToList();
                }
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
        public List<SelectListItem> GetSHDrolist(int? pId)
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
    }
}
