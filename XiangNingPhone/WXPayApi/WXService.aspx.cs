using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Xml;

namespace YuChenWXWeb.WXPayApi
{
    public partial class WXService : System.Web.UI.Page
    {
        public string Token = "sandra8225xroger";
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentEncoding = Encoding.GetEncoding("utf-8");
            string postStr = "";
            if (Request.HttpMethod.ToLower() == "post")
            {
                Stream s = HttpContext.Current.Request.InputStream;
                byte[] b = new byte[s.Length];
                s.Read(b, 0, (int)s.Length);
                postStr = Encoding.UTF8.GetString(b);
                if (!string.IsNullOrEmpty(postStr))
                {
                    ResponseMsg(postStr);
                }
            }
            else
            {
                Valid();                        //服务器配置时需要打开，验证通过后可删除此行代码}
            }
        }
        /// <summary>
        /// 返回信息结果(微信信息返回)
        /// </summary>
        /// <param name="weixinXML"></param>
        private void ResponseMsg(string weixinXML)
        {
            //回复消息的部分:你的代码写在这里
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(weixinXML);
            XmlNodeList list = doc.GetElementsByTagName("xml");
            XmlNode xn = list[0];
            string FromUserName = xn.SelectSingleNode("//FromUserName").InnerText;   //关注用户的加密后openid
            string ToUserName = xn.SelectSingleNode("//ToUserName").InnerText;       //公众微信号原始ID
            string MsgType = xn.SelectSingleNode("//MsgType").InnerText;
            if (MsgType == "text")
            {
                /*TextMessageDeal tmd=new TextMessageDeal();
                Response.Write(tmd.DealResult(weixinXML));*///实现不同关键词不同处理的类
                string strresponse = "<xml>";
                strresponse = strresponse + "<ToUserName><![CDATA[" + FromUserName + "]]></ToUserName>";
                strresponse = strresponse + "<FromUserName><![CDATA[" + ToUserName + "]]></FromUserName>";
                strresponse = strresponse + "<CreateTime>" + DateTime.Now.Ticks.ToString() + "</CreateTime>";
                strresponse = strresponse + "<MsgType><![CDATA[text]]></MsgType>";
                strresponse = strresponse + "<Content><![CDATA[" + "你好，欢迎关注" + "]]></Content>";
                strresponse = strresponse + "<FuncFlag>0<FuncFlag>";
                strresponse = strresponse + "</xml>";
                Response.Write(strresponse);

            }
            else if (MsgType == "image")
            {
                //关注用户发送图片时的自动回复处理
            }
            //else if (MsgType == "event")
            //{
            //    EventMessageDeal tmd = new EventMessageDeal();  //用户关注和取关等事件处理类，测试的可以去掉
            //    Response.Write(tmd.DealResult(weixinXML));
            //}

        }
        /// <summary>
            /// 验证微信签名
            /// </summary>
            /// * 将token、timestamp、nonce三个参数进行字典序排序
            /// * 将三个参数字符串拼接成一个字符串进行sha1加密
            /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。
            /// <returns></returns>
        private bool CheckSignature()
        {
            string signature = Request.QueryString["signature"].ToString();
            string timestamp = Request.QueryString["timestamp"].ToString();
            string nonce = Request.QueryString["nonce"].ToString();
            string[] ArrTmp = { Token, timestamp, nonce };
            Array.Sort(ArrTmp);     //字典排序
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            if (tmpStr.Equals(signature))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void Valid()
        {
            string echoStr = Request.QueryString["echoStr"].ToString();
            if (CheckSignature())
            {
                if (!string.IsNullOrEmpty(echoStr))
                {
                    Response.Write(echoStr);
                    Response.End();
                }
            }
        }
        
    }
}