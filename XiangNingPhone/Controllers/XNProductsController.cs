using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XiangNingPhone.Controllers
{
    public class XNProductsController : Controller
    {
        //
        // GET: /XNProducts/

        public ActionResult Index()
        {
            return View();
        }
        //右侧菜单
        public ActionResult _RNav()
        {
            return View();
        }
        //底部菜单
        public ActionResult _FNav()
        {
            return View();
        }
        //家具维修
        public ActionResult JJWX()
        {
            return View();
        }
        //整屋定制
        public ActionResult ZWDZ()
        {
            return View();
        }
        //工艺礼品
        public ActionResult GYLP()
        {
            return View();
        }
    }
}
