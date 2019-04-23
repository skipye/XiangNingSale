using LitJson;
using ModelProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WxPayAPI;
using ServiceProject;
using ThoughtWorks.QRCode.Codec;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Text;

namespace XiangNingPhone.Controllers
{
    public class WXPayController : Controller
    {
        public WxPayData unifiedOrderResult { get; set; }
        private static readonly OrderService  OSer = new OrderService();
        public ActionResult WXPayOrder(string orderId,string openid)
        {
            WXPayModel models = new WXPayModel();
            models.openid = openid;
            models.OrderId = orderId;
            return View(models);
        }
        public ActionResult Index()
        {
            string openid = "";
            string orderId = "";
            WXPayModel models = new WXPayModel();
            //检测是否给当前页面传递了相关参数
            if (Session["openid"] != null)
            { openid = Session["openid"].ToString(); }
            else
            {
                return Content("<script>alert('数据错误！请重新支付');history.back();</script>");
            }
            if (Session["orderId"] != null) { orderId = Session["orderId"].ToString(); }
            else { return Content("<script>alert('数据错误！请重新支付');history.back();</script>"); }
            models.openid = openid;
            models.OrderId = orderId;
            return View(models);
        }
        
        public void GetOpenId(string orderId)
        {
            Session["orderId"] = orderId;  //先把当前的OrderId储存在缓存里
            //构造网页授权获取code的URL
            string host = Request.Url.Host;
            string path = "/WXPay/OrderOpenId";
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
            return RedirectToAction("Index", "WXPay");
            //return Content(openid + "&&" + code);
        }
        //JsApiPay
        public ActionResult JsApiPay(string OrderId,string OpenId)
        {
            object objResult = "";
            int totalPrice = 0;
            string orderId = OrderId;
            string openid = OpenId;
            if (Session["orderId"] != null)
            {
                orderId = Session["orderId"].ToString();
                openid= Session["openId"].ToString();
            }

            var models = OSer.GetOrderDetail(orderId);//正常订单
            if (models.TotalPrice != null && models.TotalPrice > 0)
            {
                totalPrice = Convert.ToInt32(models.TotalPrice * 100);
            }
            if (models.SubtractPrice != null && models.SubtractPrice > 0)
            {
                totalPrice = Convert.ToInt32((models.TotalPrice - models.SubtractPrice) * 100);
            }

            //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
            try
            {
                WxPayData unifiedOrderResult = GetUnifiedOrderResult(orderId, openid, totalPrice);
                WxPayData wxJsApiParam = GetJsApiParameters();//获取H5调起JS API参数
                ModelForOrder aOrder = new ModelForOrder()
                {
                    appId = wxJsApiParam.GetValue("appId").ToString(),
                    nonceStr = wxJsApiParam.GetValue("nonceStr").ToString(),
                    packageValue = wxJsApiParam.GetValue("package").ToString(),
                    paySign = wxJsApiParam.GetValue("paySign").ToString(),
                    timeStamp = wxJsApiParam.GetValue("timeStamp").ToString(),
                    msg = "1"
                };
                objResult = aOrder;
            }
            catch (Exception ex)
            {
                ModelForOrder aOrder = new ModelForOrder()
                {
                    appId = "",
                    nonceStr = "",
                    packageValue = "",
                    paySign = "",
                    timeStamp = "",
                    //msg = "请重试,多次失败,请联系管理员."
                    msg= ex+","
                };
                objResult = aOrder;
            }
            return Json(objResult);

        }
        public WxPayData GetJsApiParameters()
        {
            Log.Debug(this.GetType().ToString(), "JsApiPay::GetJsApiParam is processing...");

            WxPayData jsApiParam = new WxPayData();
            jsApiParam.SetValue("appId", unifiedOrderResult.GetValue("appid"));
            jsApiParam.SetValue("timeStamp", WxPayApi.GenerateTimeStamp());
            jsApiParam.SetValue("nonceStr", WxPayApi.GenerateNonceStr());
            jsApiParam.SetValue("package", "prepay_id=" + unifiedOrderResult.GetValue("prepay_id"));
            jsApiParam.SetValue("signType", "MD5");
            jsApiParam.SetValue("paySign", jsApiParam.MakeSign());

            string parameters = jsApiParam.ToJson();

            Log.Debug(this.GetType().ToString(), "Get jsApiParam : " + parameters);
            return jsApiParam;
        }
        public WxPayData GetUnifiedOrderResult(string OrderNum, string openid, int totalPrice)
        {
            var NewOrder = WxPayApi.GetOutNewOrder(OrderNum);
            //统一下单
            WxPayData data = new WxPayData();
            data.SetValue("body", "香凝工艺");
            //data.SetValue("attach", "购物款");//附加数据
            data.SetValue("out_trade_no", NewOrder);
            //data.SetValue("out_trade_no", DateTime.Now.Millisecond + "$" + models.Ordernum+"JS");
            data.SetValue("total_fee", totalPrice);
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
            data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
            //data.SetValue("goods_tag", "test");//商品标记，代金券或立减优惠功能的参数
            data.SetValue("trade_type", "JSAPI");
            data.SetValue("openid", openid);

            WxPayData result = WxPayApi.UnifiedOrder(data);//统一下单
            if (!result.IsSet("appid") || !result.IsSet("prepay_id") || result.GetValue("prepay_id").ToString() == "")
            {
                WxPayApi.CloseOrder(data);//如果报错，关闭当前订单
                Log.Error(this.GetType().ToString(), "UnifiedOrder response error!");
                throw new WxPayException("UnifiedOrder response error!");
            }

            unifiedOrderResult = result;
            return result;
        }
        //二维码支付
        public ActionResult NativePay(string OrderId)
        {
            Log.Info(this.GetType().ToString(), "page load");
            int totalPrice = 0;
            string OrderNum = OrderId;
            if (string.IsNullOrEmpty(OrderNum))
            {
                if (Session["orderId"] != null) { OrderNum = Session["orderId"].ToString(); }
                else { return Content("暂无订单！"); }
            }
            
            var models = OSer.GetOrderDetail(OrderNum);//正常订单
            if (models.TotalPrice != null && models.TotalPrice > 0)
            {
                totalPrice = Convert.ToInt32(models.TotalPrice * 100);
            }
            if (models.SubtractPrice != null && models.SubtractPrice > 0)
            {
                totalPrice = Convert.ToInt32((models.TotalPrice - models.SubtractPrice) * 100);
            }
           
            
            NativePay nativePay = new NativePay();

            //生成扫码支付模式一url
            //string url1 = nativePay.GetPrePayUrl("123456789");

            //生成扫码支付模式二url
            string url2 = nativePay.GetPayUrl(OrderNum, totalPrice);

            //初始化二维码生成工具
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            qrCodeEncoder.QRCodeVersion = 0;
            qrCodeEncoder.QRCodeScale = 4;

            //将字符串生成二维码图片
            Bitmap image = qrCodeEncoder.Encode(url2, Encoding.Default);

            //保存为PNG到内存流  
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);
            return File(ms.GetBuffer(), @"image/Png");
            //输出二维码图片
            //Response.BinaryWrite(ms.GetBuffer());

        }
    }

 }
