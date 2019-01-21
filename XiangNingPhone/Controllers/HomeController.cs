using ModelProject;
using ServiceProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XiangNingPhone.Controllers
{
    public class HomeController : Controller
    {
        private static readonly CustomerService NSer = new CustomerService();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult _RightTopNav()
        {
            return View();
        }
        public ActionResult KanDian()
        {
            CustomerModel models = new CustomerModel();
            return View(models);
        }
        public ActionResult AddKanDian(CustomerModel Models)
        {
            if (NSer.AddWebCustomer(Models) == true)
            {
                return Content("1");
            }
            else { return Content("0"); }
        }
    }
}
