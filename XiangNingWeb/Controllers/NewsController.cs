using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceProject;
using ModelProject;

namespace XiangNingWeb.Controllers
{
    public class NewsController : Controller
    {
        private static readonly NewsService NSer = new NewsService();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Detail()
        {
            return View();
        }
        public ActionResult PageList(SNewsModel SModel)
        {
            ViewBag.SModel = SModel;
            var models = NSer.GetWebPageList(SModel,2);
            return View(models);
        }
    }
}
