using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModelProject;
using DataBase;
using System.Web.Security;
using System.Web.Mvc;

namespace DalProject
{
    public class UserDal
    {
        //操作日志
        public void AddWorkLogs(WorkLogsModel tables)
        {
            using (var db = new XiangNingSaleEntities())
            {
                var WorkLogs = new WorkLogs();
                WorkLogs.UserId = tables.UserId;
                WorkLogs.UserName = tables.UserName;
                WorkLogs.MSG = tables.MSG;
                WorkLogs.MSGStatus = tables.MSGStatus;
                WorkLogs.CreateTime = DateTime.Now;
                db.WorkLogs.Add(WorkLogs);
                db.SaveChanges();
            }
        }
        public List<UsersModel> GetPageList(SUsersModel SModel)
        {
            
            using (var db = new SaleHREntities())
            {
                var List = (from p in db.ehr_employee.Where(k => k.status == 1)
                            where !string.IsNullOrEmpty(SModel.Name) ? p.name.Contains(SModel.Name) : true
                            orderby p.lawdate descending
                            select new UsersModel
                            {
                                Id = p.id,
                                Name = p.name,
                                Telphone = p.tel,
                                jobtime = p.jobtime,
                                officialtime = p.officialtime,
                                departmentname = p.departmentname,
                            }).ToList();
                return List;
            }
        }
        //用户登录
        public LoginModel IsLogin(LoginModel models)
        {
            using (var db = new SaleHREntities())
            {
                var Tables = (from p in db.ehr_employee.Where(k => k.password == models.PassWord && k.status == 1)
                              where !string.IsNullOrEmpty(models.UserName) ? p.number == models.UserName : true
                              where !string.IsNullOrEmpty(models.Telephone) ? p.tel == models.Telephone : true
                              select p
                             ).FirstOrDefault();
                LoginModel ReturnModel = new LoginModel();
                if (Tables != null)
                {
                    ReturnModel.UserName = Tables.name;
                    ReturnModel.IsLogin = true;
                    ReturnModel.UserId = Tables.id;
                    ReturnModel.departmentId = Tables.department;
                    ReturnModel.department = Tables.departmentname;
                    db.SaveChanges();

                    WorkLogsModel WModels = new WorkLogsModel();
                    WModels.UserId = Tables.id;
                    WModels.UserName= Tables.name;
                    WModels.MSG = "用户登录";
                    WModels.MSGStatus = 1;
                    AddWorkLogs(WModels);
                }
                else { ReturnModel.IsLogin = false; }
               
                return ReturnModel;
            }
        }

        public UserCurrentModel GetCurrentUserName()
        {
            UserCurrentModel models = new UserCurrentModel();
            var List = FormsAuthentication.GetAuthCookie("MyCookie", false);
            if (System.Web.HttpContext.Current.User.Identity.Name.Contains("|") == false)
            {
                return new UserCurrentModel();
            }
            else
            {
                var CurrentModels = System.Web.HttpContext.Current.User.Identity.Name.Split('|');

                models.UserName = CurrentModels[0];
                models.UserId = Convert.ToInt32(CurrentModels[1]);
                models.departmentId = Convert.ToInt32(CurrentModels[2]);
                models.department = CurrentModels[3];
                return models;
            }
        }
        public void AddOrUpdate(UsersModel Models)
        {
            using (var db = new SaleHREntities())
            {
                if (Models.Id > 0)
                {
                    var table = db.ehr_employee.Where(k => k.id == Models.Id).SingleOrDefault();
                    table.tel = Models.Telphone;
                    table.name = Models.Name;
                    table.password = Models.Password;

                }
                else
                {
                    ehr_employee table = new ehr_employee();
                    table.tel = Models.Telphone;
                    table.name = Models.Name;
                    table.password = Models.Password;
                    db.ehr_employee.Add(table);
                }
                db.SaveChanges();
            }
        }
        public UsersModel GetDetailById(int Id)
        {
            using (var db = new SaleHREntities())
            {
                var tables = (from p in db.ehr_employee.Where(k => k.id == Id)
                              select new UsersModel
                              {
                                  Id = p.id,
                                  Name = p.name,
                                  Telphone = p.tel,
                                  Password = p.password,
                                  Password2 = p.password,
                              }).SingleOrDefault();
                return tables;
            }
        }
        public void Delete(string ListId)
        {
            using (var db = new XNArticleEntities())
            {
                string[] ArrId = ListId.Split('$');
                foreach (var item in ArrId)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        int Id = Convert.ToInt32(item);
                        var tables = db.A_User.Where(k => k.Id == Id).SingleOrDefault();
                        tables.State = false;
                    }
                }
                db.SaveChanges();
            }
        }
        public List<SelectListItem> GetUserDrolist(int? pId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "请选择用户", Value = "" });
            using (var db = new SaleHREntities())
            {
                List<ehr_employee> model = db.ehr_employee.Where(b => b.status == 1).OrderBy(k => k.department).ToList();
                foreach (var item in model)
                {
                    items.Add(new SelectListItem() { Text = "╋" + item.name, Value = item.id.ToString(), Selected = pId.HasValue && item.id.Equals(pId) });
                }
            }
            return items;
        }
        public List<SelectListItem> GetDepartmentDrolist(int? pId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "请选择部门", Value = "" });
            using (var db = new SaleHREntities())
            {
                List<ehr_department> model = db.ehr_department.OrderBy(k => k.id).ToList();
                foreach (var item in model)
                {
                    items.Add(new SelectListItem() { Text = "╋" + item.name, Value = item.id.ToString(), Selected = pId.HasValue && item.id.Equals(pId) });
                }
            }
            return items;
        }
    }
}
