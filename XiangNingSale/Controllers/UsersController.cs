using ModelProject;
using ServiceProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace XiangNingSale.Controllers
{
    public class UsersController : Controller
    {
        private static readonly UserService USer = new UserService();
        public ActionResult Index()
        {
            SUsersModel SModels = new SUsersModel();
            return View(SModels);
        }
        public ActionResult PageList(SUsersModel SModels)
        {
            var PageList = USer.GetPageList(SModels);
            return new ContentResult
            {
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(PageList),
                ContentType = "application/json"
            };
        }
        public ActionResult Add(int? Id)
        {
            UsersModel Models = new UsersModel();
            if (Id != null && Id > 0)
            {
                Models = USer.GetDetailById(Id.Value);
            }
            return View(Models);
        }
        [ValidateInput(false)]
        public ActionResult PostAdd(UsersModel Models)
        {
            if (USer.AddOrUpdate(Models) == true)
            {
                return Content("1");
            }
            else { return View(Models); }
        }
        //删除多个
        public ActionResult Delete(string ListId)
        {
            if (string.IsNullOrEmpty(ListId) == true)
            {
                return Content("False");
            }
            else
            {
                if (USer.Delete(ListId) == true)
                {
                    return Content("True");
                }
                else return Content("False");
            }
        }
    }
}
