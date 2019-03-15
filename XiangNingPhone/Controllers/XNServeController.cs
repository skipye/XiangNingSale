using ServiceProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XiangNingPhone.Controllers
{
    public class XNServeController : Controller
    {
        private static readonly NewsService NSer = new NewsService();
        public ActionResult Index()
        {
            return View();
        }
        //右侧导航
        public ActionResult _RNav()
        {
            return View();
        }
        //促销活动
        public ActionResult CXHD()
        {
            return View();
        }
        //家具保养
        public ActionResult JJBY()
        {
            return View();
        }
        //售后服务
        public ActionResult SHFW()
        {
            return View();
        }
        //招商加盟
        public ActionResult ZSJM()
        {
            return View();
        }
        public ActionResult Detail(int Id)
        {
            var Models = NSer.GetDetailById(Id);
            return View(Models);
        }
    }
}
