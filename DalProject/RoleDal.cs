using DataBase;
using ModelProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace DalProject
{
    public class RoleDal
    {
        public List<RoleModel> GetPageList(SRoleModel SModel)
        {
            using (var db = new XNArticleEntities())
            {
                var List = (from p in db.A_Role.Where(k => k.DeleteFlag == true)
                            where !string.IsNullOrEmpty(SModel.Name) ? p.Name.Contains(SModel.Name) : true
                            where SModel.TypeId != null && SModel.TypeId > 0 ? p.Id == SModel.TypeId : true
                            orderby p.Id
                            select new RoleModel
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Rank = p.Rank,
                                CreateTime = p.CreateTime,
                                ParentId = p.ParentId,
                                ParentName = p.A_Role2.Name,
                                Action=p.Action,
                                Controller=p.Controller,
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
                List<A_Role> model = db.A_Role.Where(b => b.DeleteFlag == true && b.ParentId == null).OrderBy(k => k.Rank).ToList();
                foreach (var item in model)
                {
                    items.Add(new SelectListItem() { Text = "╋" + item.Name, Value = item.Id.ToString(), Selected = pId.HasValue && item.Id.Equals(pId) });
                    List<A_Role> childrenmodel = db.A_Role.Where(b => b.DeleteFlag == true && b.ParentId == item.Id).OrderBy(k => k.Rank).ToList();
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
        public void AddOrUpdate(RoleModel Models)
        {
            using (var db = new XNArticleEntities())
            {
                if (Models.Id > 0)
                {
                    var table = db.A_Role.Where(k => k.Id == Models.Id).SingleOrDefault();
                    table.ParentId = Models.ParentId;
                    table.Name = Models.Name;
                    table.Rank = Models.Rank;
                    table.ParentId = Models.ParentId;
                    table.Action = Models.Action;
                    table.Controller = Models.Controller;
                    table.Remark = Models.Remark;
                }
                else
                {
                    A_Role table = new A_Role();
                    table.Name = Models.Name;
                    table.Rank = Models.Rank;
                    table.ParentId = Models.ParentId;
                    table.CreateTime = DateTime.Now;
                    table.DeleteFlag = true;
                    table.Action = Models.Action;
                    table.Controller = Models.Controller;
                    table.Remark = Models.Remark;
                    db.A_Role.Add(table);
                }
                db.SaveChanges();
            }
        }
        //根据文章Id获取内容
        public RoleModel GetDetailById(int Id)
        {
            using (var db = new XNArticleEntities())
            {
                var tables = (from p in db.A_Role.Where(k => k.Id == Id)
                              select new RoleModel
                              {
                                  Id = p.Id,
                                  Name = p.Name,
                                  Rank = p.Rank,
                                  CreateTime = p.CreateTime,
                                  ParentId = p.ParentId,
                                  Action = p.Action,
                                  Controller = p.Controller,
                                  Remark=p.Remark
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
                        var tables = db.A_Role.Where(k => k.Id == Id).SingleOrDefault();
                        tables.DeleteFlag = false;
                    }
                }
                db.SaveChanges();
            }
        }
        
    }
}
