using ServiceProject;
using ModelProject;
using System.Web.Mvc;
using WxPayAPI;
using LitJson;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Net;
using System.IO;
using Common;
using System.Collections.Generic;
using System.Web;

namespace XiangNingPhone.Controllers
{
    public class ProductsController : BaseController
    {
        private static readonly NewsService NSer = new NewsService();
        private static readonly MemberService Mser = new MemberService();
        public ActionResult Index(SNewsModel SModel)
        {
            SModel.AreaList = NSer.GetWebArealist();
            //SModel.TypeList = NSer.GetWebTypeList(2);
            return View(SModel);
        }
        public ActionResult _RecommendPro(int? TypeId, int? PageSize)
        {
            var models = NSer.GetRandomNewsList(TypeId ?? 2, PageSize ?? 3);
            return View(models);
        }
        public ActionResult PageList(SNewsModel SModel,int? Type,int? PageSize)
        {
            SModel.PageSize = PageSize??9;
            var models = NSer.GetWebPageList(SModel, Type??2);
            return View(models);
        }
        public ActionResult List(SNewsModel SModel, int? Type, int? PageIndex, int? PageSize)
        {
            SModel.PageSize = PageSize ?? 10;
            SModel.PageIndex = PageIndex ?? 0;
            var Models = NSer.GetNewsTypeList(SModel, Type??2);
            return View(Models);
        }
        public ActionResult Detail(int Id, string t)
        {
            Session["ProId"] = Id;
            string RTel = "";
            string RMemNum = "";
            if (!string.IsNullOrEmpty(t))
            {
                Session["t"] = t;
                RMemNum = t;
            }
            string userAgent = Request.UserAgent;
            if (userAgent.IndexOf("MicroMessenger") > -1)//不是微信浏览器
            {
                if (Session["openId"] == null)
                {
                    GetOpenId();
                }
            }
            if (Session["openId"] != null)
            {
                if (Session["access_token"] == null)
                {
                    Getaccesstoken();
                }
                if (judgeIsFollow(Session["access_token"].ToString(), Session["openId"].ToString()) == false)
                {
                    return Content("<script>alert('您还没关注我们公众号！先去关注！');window.location.href = '/Home/Guanzhu';</script>");
                    //return Content("<script>alert('您还没关注我们公众号！先去关注！');window.location.href = 'https://mp.weixin.qq.com/mp/profile_ext?action=home&__biz=MzI3MjIxOTM4NA==#wechat_redirect';</script>");
                }
            }
            
            if (string.IsNullOrEmpty(RMemNum))
            {
                RTel = Mser.GetTelPhoneByMemberNum(RMemNum);
            }
            var Models = NSer.GetDetailById(Id);
            var MemberNumber = "";
            string tel = RTel;
            if (Session["User"] != null)
            {
                string UserModel = Session["User"].ToString();
                MemberNumber = UserModel.Split('|')[2].ToString();
            }
            Models.MemberNumber = MemberNumber;
            Models.tel = tel;
            var existingCart = this.Carts;
            if (existingCart != null && existingCart.Count>0)
            {
                int CartCount = 0;
                foreach (var item in existingCart)
                {
                    CartCount += item.Amount;
                }
                Models.CartCount = CartCount;
            }
            return View(Models);
        }
        public ActionResult Search(string keyWord)
        { 

            SNewsModel SModels = new SNewsModel();
            SModels.Name = keyWord;
            SModels.PageIndex = 0;
            SModels.PageSize = 100;
            var Models = NSer.GetNewsTypeList(SModels, 2);
            return View(Models);
        }
        public ActionResult WXProductsDetail(int Id)
        {
            var MemberNumber = "";
            if (Session["User"] != null)
            {
                string UserModel = Session["User"].ToString();
                MemberNumber = UserModel.Split('|')[2].ToString();
            }
            var RURL = "http://m.xianginghm.com/Products/Detail/" + Id + "?t=" + MemberNumber;
            var Models = NSer.GetDetailById(Id);
            Models.appId = WxPayConfig.APPID;
            Models.timestamp = WxPayApi.GenerateTimeStamp();
            Models.nonceStr = WxPayApi.GenerateNonceStr();
            Models.finalsign = Getsignature(Models.nonceStr, Models.timestamp, RURL);
            Models.ticket = Session["ticket"].ToString();
            Models.RUrl = RURL;
            return new ContentResult
            {
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(Models),
                ContentType = "application/json"
            };
        }

        /// <summary>
        /// 获取access_token
        /// </summary>
        /// <returns></returns>
        public string Getaccesstoken()
        {
            string urljson = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + WxPayConfig.APPID + "&secret=" + WxPayConfig.APPSECRET;
            string strjson = "";
            try
            {
                //请求url以获取数据
                string result = HttpService.Get(urljson);
                JsonData jd = JsonMapper.ToObject(result);
                strjson = (string)jd["access_token"];
                Session["access_token"] = strjson;
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
        public string Getjsapi_ticket()
        {
            string accesstoken = (string)Session["access_token"];
            string urljson = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + accesstoken + "&type=jsapi";
            string strjson = "";
            try
            {
                //请求url以获取数据
                string result = HttpService.Get(urljson);
                JsonData jd = JsonMapper.ToObject(result);
                strjson = (string)jd["ticket"];
                Session["ticket"] = strjson;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return strjson;
        }
        //获取创建二维码的ticket
        public string Geterm_ticket()
        {
            string accesstoken = (string)Session["access_token"];
            string urljson = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + accesstoken;
            string strjson = "";
            try
            {
                //请求url以获取数据
                string result = HttpService.Get(urljson);
                JsonData jd = JsonMapper.ToObject(result);
                strjson = (string)jd["ticket"];
                Session["ermticket"] = strjson;
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
        public string Getsignature(string nonceStr, string timespanstr, string RUrl)
        {
            if (Session["access_token"] == null)
            {
                Getaccesstoken();
            }
            if (Session["ticket"] == null)
            {
                Getjsapi_ticket();
            }

            string str = "jsapi_ticket=" + (string)Session["ticket"] + "&noncestr=" + nonceStr +
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
        //获取带参数的微信二维码
        public string GetWXERMPic()
        {
            string erm_token = "";
            if (Session["access_token"] == null)
            {
                Getaccesstoken();
            }
            erm_token = Session["access_token"].ToString();
            //if (Session["ermticket"] == null)
            //{
            //    Geterm_ticket();
            //}
            //string str = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket="+Session["ermticket"].ToString();
            return erm_token;
        }
        public string GetWXSharPic(int Id,string GalleryItems,string t)
        {
            int PicCount = 0;
            string[] strBg = { };
            List<string> strList = new List<string>();

            if (!string.IsNullOrEmpty(GalleryItems))
            {
                var StrArr = GalleryItems.Split(';');
                foreach (var item in StrArr)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        string Name = "rebg" + PicCount + ".jpg";
                        var BigPic = "http://sale.xiangninghm.com/" + item.Replace("Thumbnails", "Published");
                        string NewbgPath = DownloadImg(BigPic, Name);
                        Bitmap b = new Bitmap(NewbgPath);
                        Image i = resizeImage(b, new Size(390, 390));
                        
                        string Path = Server.MapPath("/UpLoads/") + Name;
                        i.Save(Path);
                        i.Dispose();
                        b.Dispose();
                        strList.Add(Path);
                        PicCount++;
                    }
                }
            }
            string qrPath = Server.MapPath("/bg/logo.png");

            //调整图像大小
            string WXSharContent = "http://m.xiangninghm.com/Products/Detail/"+Id+"?t="+t;
            Bitmap b1 = QRCodeHelper.CreateQRCodeWithLogo(WXSharContent, qrPath);
            
            Image i1 = resizeImage(b1, new Size(390, 390));
            string reqrPath = Server.MapPath("/UpLoads") + "\\qrCode.jpg";
            i1.Save(reqrPath);
            i1.Dispose();
            b1.Dispose();
            strBg = strList.ToArray();
            MergeImage(strBg, reqrPath);

            string NewPicpath = "/UpLoads/new.jpg";


            return NewPicpath;
        }

        //下载图片
        private string DownloadImg(string strPath, string strName)
        {
            WebClient my = new WebClient();
            byte[] mybyte;
            mybyte = my.DownloadData(strPath);
            MemoryStream ms = new MemoryStream(mybyte);
            Image img;
            img = Image.FromStream(ms);
            string filePath = Server.MapPath("/UpLoads") + "\\" + strName;
            img.Save(filePath, ImageFormat.Jpeg);   //保存
            return filePath;
        }
        //拼图函数
        private void MergeImage(string[] strBg, string strQr)
        {
            // 数组元素个数(即要拼图的图片个数)
            int lenth = strBg.Length+1;
            // 图片集合
            Bitmap[] maps = new Bitmap[lenth];
            
            // 初始化背景图片的宽高
            Bitmap bitMap = new Bitmap(790, 790);
            // 初始化画板
            Graphics g1 = Graphics.FromImage(bitMap);
            ////设置画布背景颜色为白色
            SolidBrush b = new SolidBrush(Color.White);
            g1.FillRectangle(b, 0, 0, 790, 790);
            for (int k = 0; k < lenth; k++)
            {
                Bitmap map = null;
                if (k == lenth - 1)
                {
                    map = new Bitmap(strQr);
                    maps[lenth - 1] = map;
                }
                else {
                    
                    map = new Bitmap(strBg[k]);
                    maps[k] = map;
                }
                int YPx = 0;
                int XPx = 0;
                switch (k)
                {
                    case 0: YPx = 0; XPx = 0; break;
                    case 1: YPx = 0; XPx = 400; break;
                    case 2: YPx = 400; XPx = 0; break;
                    case 3: YPx = 400; XPx = 400; break;
                    default: YPx = 0; XPx = 0; break;
                }
                for (int i = 0; i < maps[k].Width; i++)
                {
                    for (int j = 0; j < maps[k].Height; j++)
                    {
                        // 以像素点形式绘制(将要拼图的图片上的每个坐标点绘制到拼图对象的指定位置，直至所有点都绘制完成)
                        var temp = maps[k].GetPixel(i, j);
                        // 将图片画布的点绘制到整体画布的指定位置
                        bitMap.SetPixel(XPx+i, YPx+j, temp);
                    }
                }
                maps[k].Dispose();

                //Graphics g2 = Graphics.FromImage(bitMap);
                //g2.FillRectangle(Brushes.LightGreen, new Rectangle(XPx, YPx, 260, 260));
            }

            bitMap.Save(Server.MapPath("/UpLoads") + "/new.jpg");
            g1.Dispose();
            bitMap.Dispose();
        }
        //调整图像大小
        private static Image resizeImage(Image imgToResize, Size size)
        {
            //获取图片宽度
            int sourceWidth = imgToResize.Width;
            //获取图片高度
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            //计算宽度的缩放比例
            nPercentW = (float)size.Width ;
            //计算高度的缩放比例
            nPercentH = (float)size.Height;

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;
            //期望的宽度
            int destWidth = (int)(nPercentH);
            //期望的高度
            int destHeight = (int)(nPercentH);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //绘制图像
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            imgToResize.Dispose();
            return (Image)b;
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
        public void GetOpenId()
        {
            //构造网页授权获取code的URL
            string host = Request.Url.Host;
            string path = "/Products/RequstOpenId";
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
        public ActionResult RequstOpenId(string code)
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
                if (!string.IsNullOrEmpty(openid))//如果没获取到OPenId
                {
                    Session["openId"] = openid;
                }
            }
            catch (Exception ex)
            {
                throw new WxPayException(ex.ToString());
            }
            if (Session["ProId"] != null)
            {
                return RedirectToAction("Detail", "Products", new { Id = (int)Session["ProId"] });
            }
            else { return RedirectToAction("Index", "Home"); }
            //return Content("<script>alert('" + openid + "');window.location.href = '/Home';</script>");
            
            //return Content(openid + "&&" + code);
        }
    }
}


//switch (k)
//{
//    case 0: YPx = 0; XPx = 0; break;
//    case 1: YPx = 0; XPx = 265; break;
//    case 2: YPx = 0; XPx = 530; break;
//    case 3: YPx = 265; XPx = 0; break;
//    case 4: YPx = 265; XPx = 265; break;
//    case 5: YPx = 265; XPx = 530; break;
//    case 6: YPx = 530; XPx = 0; break;
//    case 7: YPx = 530; XPx = 265; break;
//    case 8: YPx = 530; XPx = 530; break;
//    default: YPx = 0; XPx = 0; break;
//}