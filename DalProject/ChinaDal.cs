using DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace DalProject
{
    public class ChinaDal
    {
        public List<SelectListItem> GetPDropdownlist(int? pId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "-请选择 省区-", Value = "" });
            using (var db = new ChinaEntities())
            {
                List<S_Province> model = db.S_Province.OrderBy(k => k.autoId).ToList();
                foreach (var item in model)
                {
                    items.Add(new SelectListItem() { Text = item.name, Value = item.autoId.ToString(), Selected = pId.HasValue && item.autoId.Equals(pId) });
                }
            }
            return items;
        }
        public List<SelectListItem> GetCDropdownlist(int? pId, int? Id)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "-请选择 城市-", Value = "" });
            using (var db = new ChinaEntities())
            {
                List<S_City> model = db.S_City.Where(k => k.provinceId == pId).OrderBy(k => k.autoId).ToList();
                foreach (var item in model)
                {
                    items.Add(new SelectListItem() { Text = item.name, Value = item.autoId.ToString(), Selected = pId.HasValue && item.autoId.Equals(Id) });
                }
            }
            return items;
        }
        public string GetCoption(int? pId)
        {
            string StrApp = "";
            using (var db = new ChinaEntities())
            {
                List<S_City> model = db.S_City.Where(k => k.provinceId == pId).OrderBy(k => k.autoId).ToList();
                foreach (var item in model)
                {
                    StrApp += "<option value=" + item.autoId + ">" + item.name + "</option>";
                }
            }
            return StrApp;
        }
        public List<SelectListItem> GetADropdownlist(int? pId, int? Id)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "-请选择 区县-", Value = "" });
            using (var db = new ChinaEntities())
            {
                List<S_Area> model = db.S_Area.Where(k => k.cityId == pId).OrderBy(k => k.autoId).ToList();
                foreach (var item in model)
                {
                    items.Add(new SelectListItem() { Text = item.name, Value = item.autoId.ToString(), Selected = pId.HasValue && item.autoId.Equals(Id) });
                }
            }
            return items;
        }
        public string GetAoption(int? pId)
        {
            string StrApp = "";
            using (var db = new ChinaEntities())
            {
                List<S_Area> model = db.S_Area.Where(k => k.cityId == pId).OrderBy(k => k.autoId).ToList();
                foreach (var item in model)
                {
                    StrApp += "<option value=" + item.autoId + ">" + item.name + "</option>";
                }
            }
            return StrApp;
        }
    }
}
