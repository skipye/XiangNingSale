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
        private static readonly RoleService RSer = new RoleService();
        private static readonly MenuService MSer = new MenuService();
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
            var Models = USer.GetCurrentUserName();
            var StrMenuList = RSer.GetUserMenuByUserId(Models.UserId);
            var MenuItemList = new List<ModelProject.MenuItemModel>();
            if (!string.IsNullOrEmpty(StrMenuList))
            { MenuItemList = MSer.GetMenuItemList(StrMenuList); }
            //if (Models.UserId == 1)
            //{
            //    MenuItemList = MSer.GetMenuItemList("");
            //}
            return View(MenuItemList);
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
