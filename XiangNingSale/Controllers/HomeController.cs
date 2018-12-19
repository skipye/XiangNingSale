using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XiangNingSale.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

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
            return View();
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
