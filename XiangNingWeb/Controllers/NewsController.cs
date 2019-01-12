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
            SNewsModel SModel = new SNewsModel();
            
            return View(SModel);
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
            var models = NSer.GetWebPageList(SModel, 1);
            return View(models);
        }
        public ActionResult List(int? TypeId, int? PageSize)
        {
            SNewsModel SModel = new SNewsModel();
            SModel.PageSize = 4;
            var models = NSer.GetWebPageList(SModel, 1);
            return View(models);
        }
    }
}
