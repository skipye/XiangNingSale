using ModelProject;
using ServiceProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace XiangNingSale.Controllers
{
    public class CustomerController : Controller
    {
        private static readonly UserService USer = new UserService();
        private static readonly CustomerService NSer = new CustomerService();

        public ActionResult Index()
        {
            SCustomerModel SModels = new SCustomerModel();
            SModels.DepartmentDroList = USer.GetDepartmentDrolist(SModels.DepartmentId);
            return View(SModels);
        }
        public ActionResult PageList(SCustomerModel SRmodels)
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
            CustomerModel Models = new CustomerModel();
            if (Id != null && Id > 0)
            {
                Models = NSer.GetDetailById(Id.Value);
            }
            Models.DepartmentDroList = USer.GetDepartmentDrolist(Models.DepartmentId);
            return View(Models);
        }
        [ValidateInput(false)]
        public ActionResult PostAdd(CustomerModel Models)
        {
            Models.BelongUserId = USer.GetCurrentUserName().UserId;
            Models.BelongUserName = USer.GetCurrentUserName().UserName;
            Models.DepartmentId = USer.GetCurrentUserName().departmentId;
            Models.Department = USer.GetCurrentUserName().department;
            if (NSer.AddOrUpdate(Models) == true)
            {
                return Content("1");
            }
            else { return View(Models); }
        }
        public ActionResult UserIndex()
        {
            SCustomerModel SModels = new SCustomerModel();
            SModels.BelongUserId = USer.GetCurrentUserName().UserId;
            return View(SModels);
        }
        public ActionResult DepartmentIndex()
        {
            SCustomerModel SModels = new SCustomerModel();
            SModels.DepartmentId = USer.GetCurrentUserName().departmentId;
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
                if (NSer.DeleteMore(ListId) == true)
                {
                    return Content("True");
                }
                else return Content("False");
            }
        }
        public ActionResult Belong(int Id)
        {
            BelongModel Models = new BelongModel();
            Models.Id = Id;
            Models.DepartmentDroList= USer.GetDepartmentDrolist(Models.DepartmentId);
            Models.UserDroList = USer.GetUserDrolist(Models.BelongUserId);
            return View(Models);
        }
        [ValidateInput(false)]
        public ActionResult PostBelong(BelongModel Models)
        {
            if (NSer.UpdateBelong(Models) == true)
            {
                return Content("1");
            }
            else { return View(Models); }
        }
    }
}
