using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModelProject;
using DataBase;
using System.Web.Security;

namespace DalProject
{
    public class UserDal
    {
        //用户登录
        public LoginModel IsLogin(LoginModel models)
        {
            using (var db = new XNArticleEntities())
            {
                var Tables = (from p in db.A_User.Where(k => k.Password == models.PassWord && k.State==true)
                              where !string.IsNullOrEmpty(models.UserName) ? p.Name == models.UserName : true
                              where !string.IsNullOrEmpty(models.Telephone) ? p.Telphone == models.Telephone : true
                              select p
                             ).FirstOrDefault();
                LoginModel ReturnModel = new LoginModel();
                if (Tables != null)
                {
                    ReturnModel.UserName = Tables.Name;
                    ReturnModel.IsLogin = true;
                    ReturnModel.UserId = Tables.Id;
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
                return models;
            }
        }
    }
}
