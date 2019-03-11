using ModelProject;
using ServiceProject;
using System;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace XiangNingSale.Controllers
{
    public class PurchaseController : Controller
    {
        private static readonly UserService USer = new UserService();
        private static readonly PurchaseOrderService NSer = new PurchaseOrderService();

        public ActionResult Index()
        {
            SPurchaseOrderModel SModels = new SPurchaseOrderModel();
            return View(SModels);
        }
        public ActionResult PageList(SPurchaseOrderModel SRmodels)
        {
            var PageList = NSer.GetPageList(SRmodels);
            return new ContentResult
            {
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(PageList),
                ContentType = "application/json"
            };
        }
        public ActionResult Add(int? Id)
        {
            PurchaseOrderModel Models = new PurchaseOrderModel();
            if (Id != null && Id > 0)
            {
                Models = NSer.GetDetailById(Id.Value);
            }
            return View(Models);
        }
        public ActionResult AddContent(int? Id)
        {
            PurchaseOrderModel Models = new PurchaseOrderModel();
            if (Id != null && Id > 0)
            {
                Models = NSer.GetDetailById(Id.Value);
            }
            return View(Models);
        }
        [ValidateInput(false)]
        public ActionResult PostAdd(PurchaseOrderModel Models)
        {
            Models.ApplyUserId = USer.GetCurrentUserName().UserId;
            Models.ApplyUserName = USer.GetCurrentUserName().UserName;
            Models.ApplyDepartmnet = USer.GetCurrentUserName().department;
            Models.ApplyDepartmnetId = USer.GetCurrentUserName().departmentId;
            if (NSer.AddOrUpdate(Models) == true)
            {
                return Content("1");
            }
            else { return View(Models); }
        }
        public ActionResult UserIndex()
        {
            SPurchaseOrderModel SModels = new SPurchaseOrderModel();
            SModels.ApplyUserId = USer.GetCurrentUserName().UserId;
            return View(SModels);
        }
        //删除多个
        public ActionResult Delete(string ListId)
        {
            if (string.IsNullOrEmpty(ListId) == true)
            {
                return Content("False");
            }
            else
            {
                if (NSer.Delete(ListId) == true)
                {
                    return Content("True");
                }
                else return Content("False");
            }
        }
        public ActionResult Checked(string ListId, int CheckedId)
        {
            var UserId= USer.GetCurrentUserName().UserId;
            var UserName= USer.GetCurrentUserName().UserName;
            if (string.IsNullOrEmpty(ListId) == true)
            {
                return Content("False");
            }
            else
            {
                if (NSer.Checked(ListId, CheckedId, UserId, UserName) == true)
                {
                    return Content("True");
                }
                else return Content("False");
            }
        }
       
        public ActionResult CWChecked(string ListId)
        {
            SPurchaseOrderModel SModels = new SPurchaseOrderModel();
            SModels.ListId = ListId;
            return View(SModels);
        }
        public ActionResult CWCheckedPost(string ListId, int CheckedStatus, string Remarks)
        {
            if (string.IsNullOrEmpty(ListId) == true)
            {
                return Content("False");
            }
            else
            {
                if (NSer.CWCheckedMore(ListId, CheckedStatus, Remarks) == true)
                {
                    return Content("True");
                }
                else return Content("False");
            }
        }
        public ActionResult Accounts(string ListId)
        {
            if (NSer.CWAccounts(ListId) == true)
            {
                return Content("True");
            }
            else { return Content("False"); }
        }
        public ActionResult Show(int Id)
        {
            var Models = NSer.GetDetailById(Id);
            return View(Models);
        }
    }
}
