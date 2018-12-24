using ServiceProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XiangNingSale.Controllers
{
    public class HomeController : Controller
    {
        private static readonly UserService USer = new UserService();
        [Authorize]

        public ActionResult Index()
        {
            return View();
        }
        //欢迎页面
        public ActionResult Welcome()
        {
            return View();
        }
        public ActionResult _Header()
        {
            var Models = USer.GetCurrentUserName();
            return View(Models);
        }
        public ActionResult _Menu()
        {
            return View();
        }
        public ActionResult _Footer()
        { 
            return View(); 
        }
        public ActionResult _Meta()
        {
            return View();
        }
    }
}
