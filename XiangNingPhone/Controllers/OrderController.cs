using ModelProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceProject;

namespace XiangNingPhone.Controllers
{
    public class OrderController : BaseController
    {
        private static readonly OrderService OSer = new OrderService();
        private static readonly UserService USer = new UserService();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddOrder(OrderModel models)
        {
            models.Ordernum = "XN" + DateTime.Now.ToString("yyyyMMddHHmmss");
            if (Session["User"] != null)
            {
                string UserModel = Session["User"].ToString();
                models.MemberId = new Guid(UserModel.Split('|')[1]);
            }
            else { return Content("3"); }
            if (this.Carts != null)
            {
                models.Carts = this.Carts;
            }
            else
            {
                return Content("2");
            }
            //return Content("0");
            if (OSer.AddOrder(models) == true)
            {
                //Session.Abandon();//清除全部Session
                //清除某个Session
                Session["Carts"] = null;
                Session.Remove("Carts");
                Session["orderId"] = models.Ordernum;
                return Content("1&" + models.Ordernum);
            }
            else { return Content("0"); }
        }
        public ActionResult OrderList(string KeyWord, int PageSize, int PageIndex, bool? TimeOut, bool? PayState)
        {
            Guid MemberId = Guid.Empty;
            if (Session["User"] != null)
            {
                string UserModel = Session["User"].ToString();
                MemberId = new Guid(UserModel.Split('|')[1]);
            }
            //else { return RedirectToAction("Login", "Account",new { ReturnUrl ="/Member"}); }
            var models = OSer.GetOrderList(KeyWord, MemberId, TimeOut, PageSize, PageIndex, PayState);
            return View(models);
        }
        public ActionResult DelOrder(int Id)
        {
            if (OSer.DelOrderById(Id) == true)
            {
                return Content("1");
            }
            else { return Content("0"); }
        }
    }
}
