using LitJson;
using ModelProject;
using ServiceProject;
using System;
using System.Web;
using System.Web.Mvc;
using WxPayAPI;

namespace XiangNingPhone.Controllers
{
    public class HomeController : Controller
    {
        private static readonly CustomerService NSer = new CustomerService();
        private static readonly ChinaService CSer = new ChinaService();
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
        public ActionResult PDropdownlist(int? areaParentId)
        {
            var models = CSer.GetPDropdownlist(areaParentId);
            return Json(models);
        }
        public ActionResult CDropdownlist(int? areaParentId)
        {
            string models = CSer.GetCoption(areaParentId).ToString();
            return Content(models);
        }
        public ActionResult ADropdownlist(int? areaParentId, int? Id)
        {
            string models = CSer.GetAoption(areaParentId).ToString();
            return Content(models);
        }
        
        public ActionResult Guanzhu()
        {
            return View();
        }
    }
}
