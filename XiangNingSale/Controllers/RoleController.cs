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
    public class RoleController : Controller
    {
        private static readonly RoleService CSer = new RoleService();
        public ActionResult Index()
        {
            SRoleModel SModels = new SRoleModel();
            SModels.TypeDroList = CSer.GetParentType(SModels.TypeId);
            return View(SModels);
        }
        public ActionResult PageList(SRoleModel SModels)
        {
            var PageList = CSer.GetPageList(SModels);
            return new ContentResult
            {
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(PageList),
                ContentType = "application/json"
            };
        }
        public ActionResult Add(int? Id)
        {
            RoleModel Models = new RoleModel();
            if (Id != null && Id > 0)
            {
                Models = CSer.GetDetailById(Id.Value);
            }
            Models.TypeDroList = CSer.GetParentType(Models.TypeId);
            return View(Models);
        }
        [ValidateInput(false)]
        public ActionResult PostAdd(RoleModel Models)
        {
            if (CSer.AddOrUpdate(Models) == true)
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
                if (CSer.DeleteMore(ListId) == true)
                {
                    return Content("True");
                }
                else return Content("False");
            }
        }

    }
}
