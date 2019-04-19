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
        private static readonly FinanceService FSer = new FinanceService();
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
        public ActionResult Show(int Id)
        {
            var List = FSer.GetFinanceFRLogs(Id);
            return View(List);
        }
        public ActionResult WXFKShow(int Id)
        {
            var List = FSer.GetWXFinanceFRLogs(Id);
            return View(List);
        }
        public ActionResult FR(int Id)
        {
            FinanceModel Models = new FinanceModel();
            Models.Id = Id;
            return View(Models);
        }
        public ActionResult WXSR(int Id)
        {
            FinanceModel Models = new FinanceModel();
            Models.Id = Id;
            return View(Models);
        }
        public ActionResult FostFR(FinanceModel Models)
        {
            Models.operator_id = USer.GetCurrentUserName().UserId;
            Models.operator_name = USer.GetCurrentUserName().UserName;
            if (FSer.AddOrUpdate(Models) == true)
            {
                return Content("1");
            }
            else { return View(Models); }
        }
        public ActionResult FostWXFR(FinanceModel Models)
        {
            Models.operator_id = USer.GetCurrentUserName().UserId;
            Models.operator_name = USer.GetCurrentUserName().UserName;
            if (FSer.AddOrUpdateWX(Models) == true)
            {
                return Content("1");
            }
            else { return View(Models); }
        }
        public ActionResult WXOrder()
        {
            SWXOrderModel Models = new SWXOrderModel();
            DateTime datetime = DateTime.Now;
            if (string.IsNullOrEmpty(Models.StartTime))
            {
                Models.StartTime = datetime.AddDays(1 - datetime.Day).ToString("yyyy-MM-dd");
            }
            if (string.IsNullOrEmpty(Models.EndTime))
            {
                Models.EndTime = datetime.AddDays(1 - datetime.Day).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            }
            return View(Models);
        }
        public ActionResult WXPageList()
        {
            return View();
        }
    }
}
