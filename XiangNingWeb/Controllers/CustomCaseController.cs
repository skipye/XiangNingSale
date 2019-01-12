using ModelProject;
using ServiceProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XiangNingWeb.Controllers
{
    public class CustomCaseController : Controller
    {
        private static readonly NewsService NSer = new NewsService();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Detail(int Id)
        {
            var Models = NSer.GetDetailById(Id);
            return View(Models);
        }
        public ActionResult PageList(SNewsModel SModel)
        {
            ViewBag.SModel = SModel;
            SModel.PageSize = 9;
            var models = NSer.GetWebPageList(SModel, 3);
            return View(models);
        }
    }
}
