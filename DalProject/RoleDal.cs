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
                            where !string.IsNullOrEmpty(SModel.Name) ? p.UserName.Contains(SModel.Name) : true
                            orderby p.Id
                            select new RoleModel
                            {
                                Id = p.Id,
                                UserId = p.UserId,
                                UserName = p.UserName,
                                CreateTime = p.CreateTime,
                                MenuList = p.MenuList,
                            }).ToList();
                return List;
            }
        }
       
        //新增和修改仓库设置
        public void AddOrUpdate(RoleModel Models)
        {
            using (var db = new XNArticleEntities())
            {
                if (Models.Id > 0)
                {
                    var table = db.A_Role.Where(k => k.Id == Models.Id).SingleOrDefault();
                    table.UserId = Models.UserId;
                    table.UserName = Models.UserName;
                    table.MenuList = Models.MenuList;
                }
                else
                {
                    A_Role table = new A_Role();
                    table.UserId = Models.UserId;
                    table.UserName = Models.UserName;
                    table.MenuList = Models.MenuList;
                    table.CreateTime = DateTime.Now;
                    table.DeleteFlag = true;
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
                                  UserId = p.UserId,
                                  UserName = p.UserName,
                                  CreateTime = p.CreateTime,
                                  MenuList = p.MenuList,
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
        public string GetUserMenuByUserId(int UserId)
        {
            using (var db = new XNArticleEntities())
            {
                var StrMenu = db.A_Role.Where(k => k.UserId == UserId).FirstOrDefault();
                return StrMenu.MenuList;
            }
        }
        
    }
}
