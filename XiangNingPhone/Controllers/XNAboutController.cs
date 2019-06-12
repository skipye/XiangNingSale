using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XiangNingPhone.Controllers
{
    public class XNAboutController : Controller
    {
        //
        // GET: /XNAbout/

        //右侧导航
        public ActionResult _RNav()
        {
            return View();
        }
        //联系我们
        public ActionResult LXWM()
        {
            return View();
        }
        //企业理念
        public ActionResult QYLN()
        {
            return View();
        }
        //香凝资讯
        public ActionResult XNZX()
        {
            return View();
        }
        //往期回顾
        public ActionResult Review()
        {
            return View();
        }
    }
}
