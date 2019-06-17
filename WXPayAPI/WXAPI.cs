using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.SessionState;
using WxPayAPI;

namespace WXPayAPI
{
    public class WXAPI
    {
        /// <summary>
        /// 获取access_token
        /// </summary>
        /// <returns></returns>
        public static string Getaccesstoken()
        {
            string urljson = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + WxPayConfig.APPID + "&secret=" + WxPayConfig.APPSECRET;
            string strjson = "";
            try
            {
                //请求url以获取数据
                string result = HttpService.Get(urljson);
                JsonData jd = JsonMapper.ToObject(result);
                strjson = (string)jd["access_token"];
                HttpContext.Current.Session["access_token"] = strjson;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return strjson;
        }

        /// <summary>
        /// 获得jsapi_ticket
        /// </summary>
        /// <returns></returns>
        public static string Getjsapi_ticket()
        {
            if (HttpContext.Current.Session["access_token"] == null)
            {
                Getaccesstoken();
            }
            string accesstoken = (string)HttpContext.Current.Session["access_token"];
            string urljson = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + accesstoken + "&type=jsapi";
            string strjson = "";
            try
            {
                //请求url以获取数据
                string result = HttpService.Get(urljson);
                JsonData jd = JsonMapper.ToObject(result);
                strjson = (string)jd["ticket"];
                HttpContext.Current.Session["ticket"] = strjson;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return strjson;
        }
        /// <summary>
        /// 生成signature
        /// </summary>
        /// <param name="nonceStr"></param>
        /// <param name="timespanstr"></param>
        /// <returns></returns>
        public static string Getsignature(string nonceStr, string timespanstr, string RUrl)
        {
            if (HttpContext.Current.Session["access_token"] == null)
            {
                Getaccesstoken();
            }
            if (HttpContext.Current.Session["ticket"] == null)
            {
                Getjsapi_ticket();
            }

            string str = "jsapi_ticket=" + (string)HttpContext.Current.Session["ticket"] + "&noncestr=" + nonceStr +
                "&timestamp=" + timespanstr + "&url=" + RUrl;// +"&wxref=mp.weixin.qq.com";
            string singature = getSha1(str).ToLower();
            string ss = singature;
            return ss;
        }
        public static String getSha1(String str)
        {
            //建立SHA1对象
            SHA1 sha = new SHA1CryptoServiceProvider();
            //将mystr转换成byte[] 
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] dataToHash = enc.GetBytes(str);
            //Hash运算
            byte[] dataHashed = sha.ComputeHash(dataToHash);
            sha.Dispose();
            //将运算结果转换成string
            string hash = BitConverter.ToString(dataHashed).Replace("-", "");
            return hash;
        }

        /**
         * 判断用户是否关注了公众号
         * @param token
         * @param openid
         * @return
         */
        public static bool judgeIsFollow(string token, string openid)
        {
            bool IsGuanzhu = true;
            string url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + token + "&openid=" + openid + "&lang=zh_CN";
            try
            {
                string result = HttpService.Get(url);
                JsonData jd = JsonMapper.ToObject(result);
                int subscribe = (int)jd["subscribe"];
                if (subscribe == 0)
                { IsGuanzhu = false; }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return IsGuanzhu;
        }
    }
}
