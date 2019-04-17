using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WxPayAPI;

namespace XiangNingPhone.Controllers
{
    public class WXPayController : Controller
    {
        //
        // GET: /WXPay/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Obtain()
        {
            //构造网页授权获取code的URL
            string host = Request.Url.Host;
            string path = "/WXPay/ObtainOpenId";
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
                //触发微信返回code码         
                Response.Redirect(url, true);//Redirect函数会抛出ThreadAbortException异常，不用处理这个异常
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                throw new Exception(ex.Message);
            }
            return View();
        }
        public ActionResult ObtainOpenId(string code)
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

                Session["openId"] = openid;
            }
            catch (Exception ex)
            {
                throw new WxPayException(ex.ToString());
            }
            //return Content("<script>alert('" + openid + "');window.location.href = '/Home';</script>");
            return RedirectToAction("Index", "Home");
            //return Content(openid + "&&" + code);
        }
        public ActionResult GetOpenId(string orderId)
        {
            //判断订单是否存在，存在在存入缓存里面
            if (string.IsNullOrEmpty(orderId))
            {
                return Content("<script>alert('订单错误！');history.back();</script>");
            }
            else { Session["orderId"] = orderId; }
            //判断是否存在OpenId
            if (Session["openid"] != null)
            {
                return RedirectToAction("Index", "WXPay");
            }
            //构造网页授权获取code的URL
            string host = Request.Url.Host;
            string path = "/ObtainWXOpenId/OrderOpenId";
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
            return View();
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
            return RedirectToAction("Index", "WXPay");
            //return Content(openid + "&&" + code);
        }
    }
}
