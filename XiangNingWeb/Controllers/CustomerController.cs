using ModelProject;
using ServiceProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XiangNingWeb.Controllers
{
    public class CustomerController : Controller
    {
        private static readonly CustomerService NSer = new CustomerService();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add(int Id)
        {
            CustomerModel Models = new CustomerModel();
            Models.Id = Id;
            return View(Models);
        }
        public ActionResult PostAdd(CustomerModel Models)
        {
            if (NSer.AddWebCustomer(Models) == true)
            {
                return Content("1");
            }
            else { return Content("0"); }
        }
    }
}
