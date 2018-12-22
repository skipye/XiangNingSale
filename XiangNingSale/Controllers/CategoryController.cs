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
    public class CategoryController : Controller
    {
        private static readonly CategoryService CSer = new CategoryService();
        public ActionResult Index()
        {
            SCategoryModel SModels = new SCategoryModel();
            SModels.TypeDroList = CSer.GetParentType(SModels.TypeId);
            return View(SModels);
        }
        public ActionResult PageList(SCategoryModel SModels)
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
            CategoryModel Models = new CategoryModel();
            if (Id != null && Id > 0)
            {
                Models = CSer.GetDetailById(Id.Value);
            }
            Models.TypeDroList = CSer.GetParentType(Models.TypeId);
            return View(Models);
        }
        [ValidateInput(false)]
        public ActionResult PostAdd(CategoryModel Models)
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
