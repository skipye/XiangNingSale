using ModelProject;
using ServiceProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XiangNingPhone.Controllers
{
    public class AccountController : Controller
    {
        private static readonly ValidateCode VC = new ValidateCode();
        //private static readonly LogsService LSer = new LogsService();
        private static readonly MemberService USer = new MemberService();
        public ActionResult Login(string ReturnUrl)
        {

            if (string.IsNullOrEmpty(ReturnUrl))
            {
                if (Session["ReturnUrl"] != null)
                {
                    ReturnUrl = Session["ReturnUrl"].ToString();
                }
            }
            else { Session["ReturnUrl"] = ReturnUrl; }
            ViewBag.ReturnUrl = ReturnUrl;
            
            return View();

        }
        public ActionResult LogOn(string userCode, string passWord)
        {
            Guid UserId = Guid.Empty;
            string UserName = "";
            bool IsUserInfo = false;
            string openId = "";
            if (Session["openId"] != null)
            { openId = Session["openId"].ToString(); }
            if (USer.IsLogin(userCode, passWord, out UserId, out IsUserInfo, openId, out UserName) == true)
            {
                string UserAuthority = UserName + "|" + UserId;
                //System.Web.Security.FormsAuthentication.SetAuthCookie(UserAuthority, false);
                Session["User"] = UserAuthority;
                //return RedirectToAction(returnUrl);
                return Content("1&" + IsUserInfo);
            }
            else { return Content("0"); }
        }
        public ActionResult Register(string MemberNumber)
        {
            ViewBag.MemberNumber = MemberNumber;
            if (!string.IsNullOrEmpty(MemberNumber))
            {
                Session["MemberNumber"] = MemberNumber;
            }
            return View();
        }
        public ActionResult HZhuCe(MemberModel models)
        {
            bool IsSubmit = true;
            string CheckCode = Session["ValidatorCode"].ToString();
            if (Session["ValidatorCode"] == null || CheckCode != models.CheckCode)
            {
                IsSubmit = false;
            }
            if (IsSubmit == true)
            {
                Guid UserId = Guid.Empty;
                if (USer.AddUser(models, out UserId) == true)
                {
                    string UserAuthority = models.Name + "|" + UserId;
                    System.Web.Security.FormsAuthentication.SetAuthCookie(UserAuthority, false);

                    return Content("True");
                }
                else { return Content("False"); }
            }
            else { return Content("False"); }
        }
        public ActionResult IsSameName(string Name)
        {
            if (USer.IsSameName(Name) == true)
            { return Content("1"); }
            else { return Content("0"); }
        }
        public ActionResult IsSamePhone(string Phone)
        {
            if (USer.IsSamePhone(Phone) == true)
            { return Content("1"); }
            else { return Content("0"); }
        }
        public ActionResult GetValidatorGraphics()
        {
            string code = VC.NewValidateCode(4);
            Session["ValidatorCode"] = code;
            return Content(code);
            //byte[] graphic = VC.NewValidateCodeGraphic(code);
            //return File(graphic, @"image/jpeg");
        }
        //找回密码
        public ActionResult BackPassword()
        {
            return View();
        }
        public ActionResult HBackPassword(string TelPhone, string newPassword, string yzm)
        {

            bool IsSubmit = true;
            string CheckCode = Session["ValidatorCode"].ToString();
            if (Session["ValidatorCode"] == null || CheckCode != yzm)
            {
                IsSubmit = false;
            }
            if (IsSubmit == true)
            {
                bool IsBack = USer.BackPassword(TelPhone, newPassword);
                Guid UserId = Guid.Empty;
                if (IsBack == true)
                {
                    return Content("1");
                }
                else { return Content("0"); }
            }
            else { return Content("0"); }
        }
        //输入手机
        public ActionResult SubmitPhone()
        {
            return View();
        }
        public ActionResult HPhone(string Phone, string password, string yzm)
        {

            string CheckCode = Session["ValidatorCode"].ToString();
            if (Session["ValidatorCode"] == null || CheckCode != yzm)
            {
                return Content("2&验证码错误！");
            }
            string ReturnUrl = "/Home";
            if (Session["ReturnUrl"] != null)
            {
                ReturnUrl = Session["ReturnUrl"].ToString();
            }
            Guid UserId = Guid.Empty;
            if (USer.GetUserIdOrName() != null)
            {
                var UserModel = USer.GetUserIdOrName();
                UserId = UserModel.MemberId;
            }
            if (USer.UpdatePhone(Phone, password, UserId) == true)
            {
                return Content("1&" + ReturnUrl);
            }
            else { return Content("0&网络错误！"); }
        }
        public ActionResult LoginOut()
        {
            //System.Web.Security.FormsAuthentication.SignOut();//清除登录记录
            Session["User"] = null;
            Session.Remove("User");
            //return RedirectToAction("Logon", "Account", new { area = "" });
            return RedirectToAction("Index", "Home");
        }

    }
}
