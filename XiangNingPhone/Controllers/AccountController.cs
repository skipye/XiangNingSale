using LitJson;
using ModelProject;
using ServiceProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WxPayAPI;

namespace XiangNingPhone.Controllers
{
    public class AccountController : Controller
    {
        private static readonly ValidateCode VC = new ValidateCode();
        //private static readonly LogsService LSer = new LogsService();
        private static readonly MemberService USer = new MemberService();
        public ActionResult Login(string ReturnUrl)
        {
            string userAgent = Request.UserAgent;
            if (string.IsNullOrEmpty(ReturnUrl))
            {
                if (Session["ReturnUrl"] != null)
                {
                    ReturnUrl = Session["ReturnUrl"].ToString();
                }
                else { ReturnUrl = "/Member/Index"; }
            }
            else { Session["ReturnUrl"] = ReturnUrl; }
            ViewBag.ReturnUrl = ReturnUrl;
            if (Session["openId"] != null)
            {
                string openId=Session["openId"].ToString();
                var Models = USer.IsWXLogin(openId);
                if (Models!=null && Models.IsLogin == true)
                {
                    string UserAuthority = Models.UserName + "|" + Models.MemberId + "|" + Models.MemberNumber;
                    Session["User"] = UserAuthority;
                    return Redirect(ReturnUrl);
                }
                else { return View(); }
            }
            else {
                if (userAgent.IndexOf("MicroMessenger") <= -1)//不是微信浏览器
                {
                    return View();
                }
                else { GetOpenId(); }
            }

            return View();
        }
        public ActionResult LogOn(string userCode, string passWord)
        {
            string openId = "";
            if (Session["openId"] != null)
            { openId = Session["openId"].ToString(); }
            var Models = USer.IsLogin(userCode, passWord, openId);
            if (Models.IsLogin == true)
            {
                string UserAuthority = Models.UserName + "|" + Models.MemberId + "|" + Models.MemberNumber;
                //System.Web.Security.FormsAuthentication.SetAuthCookie(UserAuthority, false);
                Session["User"] = UserAuthority;
                //return RedirectToAction(returnUrl);
                return Content("1");
            }
            else { return Content("0"); }
        }
        public ActionResult Register(string MemberNumber)
        {
            if (!string.IsNullOrEmpty(MemberNumber))
            {
                Session["MemberNumber"] = MemberNumber;
            }
            else
            {
                if (Session["t"] != null)
                { MemberNumber = Session["t"].ToString(); }
            }
            ViewBag.MemberNumber = MemberNumber;
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
            if (Session["openId"] != null)
            {
                models.OpenId = Session["openId"].ToString();
            }
            if (IsSubmit == true)
            {
                Guid UserId = Guid.Empty;
                string MemNum = "";
                if (USer.AddUser(models, out UserId, out MemNum) == true)
                {
                    string UserAuthority = models.Name + "|" + UserId + "" + MemNum;
                    Session["User"] = UserAuthority;
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
        public void GetOpenId()
        {
            //先把当前的OrderId储存在缓存里
            //构造网页授权获取code的URL
            string host = Request.Url.Host;
            string path = "/Account/OrderOpenId";
            string redirect_uri = HttpUtility.UrlEncode("http://" + host + path);
            WxPayData data = new WxPayData();
            data.SetValue("appid", WxPayConfig.APPID);
            data.SetValue("redirect_uri", redirect_uri);
            data.SetValue("response_type", "code");
            data.SetValue("scope", "snsapi_base");
            data.SetValue("state", "STATE" + "#wechat_redirect");
            string url = "https://open.weixin.qq.com/connect/oauth2/authorize?" + data.ToUrl();
            try
            {
                Response.Redirect(url, true);//Redirect函数会抛出ThreadAbortException异常，不用处理这个异常
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public ActionResult OrderOpenId(string code)
        {
            string openid = "";
            try
            {
                //构造获取openid及access_token的url
                WxPayData data = new WxPayData();
                data.SetValue("appid", WxPayConfig.APPID);
                data.SetValue("secret", WxPayConfig.APPSECRET);
                data.SetValue("code", code);
                data.SetValue("grant_type", "authorization_code");
                string url = "https://api.weixin.qq.com/sns/oauth2/access_token?" + data.ToUrl();

                //请求url以获取数据
                string result = HttpService.Get(url);
                //保存access_token，用于收货地址获取
                JsonData jd = JsonMapper.ToObject(result);
                //获取用户openid
                openid = (string)jd["openid"];
                if (string.IsNullOrEmpty(openid))//如果没获取到OPenId
                {
                    return Content("<script>alert('网络错误！');history.go(-2);</script>");
                }
                Session["openId"] = openid;
            }
            catch (Exception ex)
            {
                throw new WxPayException(ex.ToString());
            }
            //return Content("<script>alert('" + openid + "');window.location.href = '/Home';</script>");
            return RedirectToAction("Login", "Account");
            //return Content(openid + "&&" + code);
        }
    }
}
