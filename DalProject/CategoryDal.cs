using DataBase;
using ModelProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace DalProject
{
    public class CategoryDal
    {
        public List<CategoryModel> GetPageList(SCategoryModel SModel)
        {
            using (var db = new XNArticleEntities())
            {
                var List = (from p in db.A_NewsType.Where(k => k.State == true)
                            where !string.IsNullOrEmpty(SModel.Name) ? p.Name.Contains(SModel.Name) : true
                            where SModel.TypeId != null && SModel.TypeId > 0 ? p.Id == SModel.TypeId : true
                            orderby p.CreateTime descending
                            select new CategoryModel
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Rank = p.Rank,
                                CreateTime = p.CreateTime,
                                ParentId = p.ParentId,
                                ParentName = p.A_NewsType2.Name,
                            }).ToList();
                return List;
            }
        }
        public List<SelectListItem> GetParentType(int? pId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "请选择父类别", Value = "" });
            using (var db = new XNArticleEntities())
            {
                List<A_NewsType> model = db.A_NewsType.Where(b => b.State == true && b.ParentId == 0).OrderBy(k => k.Id).ToList();
                foreach (var item in model)
                {
                    items.Add(new SelectListItem() { Text = "╋" + item.Name, Value = item.Id.ToString(), Selected = pId.HasValue && item.Id.Equals(pId) });
                    List<A_NewsType> childrenmodel = db.A_NewsType.Where(b => b.State == true && b.ParentId == item.Id).OrderBy(k => k.Id).ToList();
                    if (childrenmodel != null && childrenmodel.Any())
                    {
                        foreach (var Citem in childrenmodel)
                        {
                            items.Add(new SelectListItem() { Text = "----└" + Citem.Name, Value = Citem.Id.ToString(), Selected = pId.HasValue && Citem.Id.Equals(pId) });
                        }
                    }
                }
            }
            return items;
        }
       
        //新增和修改仓库设置
        public void AddOrUpdate(CategoryModel Models)
        {
            using (var db = new XNArticleEntities())
            {
                if (Models.Id > 0)
                {
                    var table = db.A_NewsType.Where(k => k.Id == Models.Id).SingleOrDefault();
                    table.Name = Models.Name;
                    table.Rank = Models.Rank;
                    table.ParentId = Models.ParentId;
                    
                }
                else
                {
                    A_NewsType table = new A_NewsType();
                    table.Name = Models.Name;
                    table.Rank = Models.Rank;
                    table.ParentId = Models.ParentId;
                    table.CreateTime = DateTime.Now;
                    table.State = true;
                    db.A_NewsType.Add(table);
                }
                db.SaveChanges();
            }
        }
        //根据文章Id获取内容
        public CategoryModel GetDetailById(int Id)
        {
            using (var db = new XNArticleEntities())
            {
                var tables = (from p in db.A_NewsType.Where(k => k.Id == Id)
                              select new CategoryModel
                              {
                                  Id = p.Id,
                                  Name = p.Name,
                                  Rank = p.Rank,
                                  CreateTime = p.CreateTime,
                                  ParentId = p.ParentId,
                              }).SingleOrDefault();
                return tables;
            }
        }

        public void DeleteMore(string ListId)
        {
            using (var db = new XNArticleEntities())
            {
                string[] ArrId = ListId.Split('$');
                foreach (var item in ArrId)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        int Id = Convert.ToInt32(item);
                        var tables = db.A_NewsType.Where(k => k.Id == Id).SingleOrDefault();
                        tables.State = false;
                    }
                }
                db.SaveChanges();
            }
        }
        
    }
}
