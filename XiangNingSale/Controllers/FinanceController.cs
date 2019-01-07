using ModelProject;
using ServiceProject;
using System;
using System.Web.Mvc;

namespace XiangNingSale.Controllers
{
    public class FinanceController : Controller
    {
        //
        // GET: /Finance/
        private static readonly UserService USer = new UserService();
        public ActionResult Purchase()
        {
            SPurchaseOrderModel Models = new SPurchaseOrderModel();
            return View(Models);
        }
        public ActionResult Order()
        {
            SContractHeaderModel Models = new SContractHeaderModel();
            DateTime datetime = DateTime.Now;
            if (string.IsNullOrEmpty(Models.StartTime))
            {
                Models.StartTime = datetime.AddDays(1 - datetime.Day).ToString("yyyy-MM-dd");
            }
            if (string.IsNullOrEmpty(Models.EndTime))
            {
                Models.EndTime = datetime.AddDays(1 - datetime.Day).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            }
            Models.DepartmentDroList = USer.GetDepartmentDrolist(Models.DepartmentId);
            return View(Models);
        }
    }
}
