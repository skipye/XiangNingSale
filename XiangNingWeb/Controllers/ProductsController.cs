using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XiangNingWeb.Controllers
{
    public class ProductsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Detail(int? Id)
        {
            return View();
        }
        public ActionResult _RecommendPro()
        {
            return View();
        }
    }
}
