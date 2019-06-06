using ModelProject;
using ServiceProject;
using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml;
using WxPayAPI;

namespace XiangNingPhone.Controllers
{
    public class WXServiceController : Controller
    {
        const string Token = "sandra8225xroger";
        
        /// <summary> 按照api说明对signature进行校验，校验成功返回参数echostr </summary>
        /// <returns></returns>
        public ActionResult CheckSign()
        {
            WX_Shar_ReturnModel models = new WX_Shar_ReturnModel();
            string StrRequest = "";
            if (Request.InputStream.Length > 0)
            {
                StreamReader reader = new StreamReader(Request.InputStream, Encoding.UTF8);
                string s = reader.ReadToEnd();//xml字符串
                reader.Close();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(s);
                XmlNodeList xnl = doc.GetElementsByTagName("Event");
                XmlNodeList list = doc.GetElementsByTagName("xml");
                XmlNode xn = list[0];
                string FromUserName = xn.SelectSingleNode("//FromUserName").InnerText;   //关注用户的加密后openid
                string ToUserName = xn.SelectSingleNode("//ToUserName").InnerText;       //公众微信号原始ID
                string MsgType = xn.SelectSingleNode("//MsgType").InnerText;
                string Event = xn.SelectSingleNode("//Event").InnerText;
                string EventKey = xn.SelectSingleNode("//EventKey").InnerText;
                string Ticket= xn.SelectSingleNode("//Ticket").InnerText;

                models.EventKey = EventKey;
                models.ToUserName = ToUserName;
                models.FromUserName = FromUserName;
                models.MsgType = MsgType;
                models.Event = Event;
                WXReturnDal.AddWX_Shar_Return(models);

                if (xnl.Count > 0 && xnl[0].InnerText == "SCAN")//扫码事件
                {
                    xnl = doc.GetElementsByTagName("EventKey");
                    if (xnl.Count > 0)
                    {
                        string scene_id = xnl[0].InnerText;///
                        
                        StrRequest = scene_id;

                        
                    }
                }
            }
            if (!string.IsNullOrEmpty(StrRequest))
            {
                return RedirectToAction(StrRequest);
            }
            else { return RedirectToAction("/Home"); }
        }
        public ActionResult Index()
        {
            CheckSign();//token验证
            string StrRequest = "";
            if (Request.InputStream.Length > 0)
            {
                StreamReader reader = new StreamReader(Request.InputStream, Encoding.UTF8);
                string s = reader.ReadToEnd();//xml字符串
                reader.Close();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(s);
                XmlNodeList xnl = doc.GetElementsByTagName("Event");
                if (xnl.Count > 0 && xnl[0].InnerText == "SCAN")//扫码事件
                {
                    xnl = doc.GetElementsByTagName("EventKey");
                    if (xnl.Count > 0)
                    {
                        string scene_id = xnl[0].InnerText;///
                        StrRequest = scene_id;
                    }
                }
            }
            if (!string.IsNullOrEmpty(StrRequest))
            {
                return RedirectToAction(StrRequest);
            }
            else { return RedirectToAction("/Home"); }
        }
    }
}
