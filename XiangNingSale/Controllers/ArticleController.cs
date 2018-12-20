using ServiceProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ModelProject;
using System.Web.Script.Serialization;

namespace XiangNingSale.Controllers
{
    public class ArticleController : Controller
    {
        //
        // GET: /Article/
        private static readonly NewsService NSer = new NewsService();
        public ActionResult Index(SNewsModel Smodels)
        {
            Smodels.TypeDroList = NSer.GetNewTypeDrolist(Smodels.TypeId);
            return View(Smodels);
        }
        public ActionResult PageList(SNewsModel SRmodels)
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
            NewsModel Models = new NewsModel();
            if (Id != null && Id > 0)
            {
                Models = NSer.GetDetailById(Id.Value);
            }
            Models.TypeDroList = NSer.GetNewTypeDrolist(Models.TypeId);
            return View(Models);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(NewsModel Models)
        {
            if (NSer.AddOrUpdate(Models) == true)
            {
                return RedirectToAction("Index", "News");
            }
            else { return View(Models); }
        }
        //删除单个
        public ActionResult DeleteOne(int Id)
        {
            string ListId = Id + "$";
            if (NSer.DeleteMore(ListId) == true)
            {
                return Content("True");
            }
            else return Content("False");
        }
        //删除多个
        public ActionResult DeleteMore(string ListId)
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
        //获取所有的图片列表
        public ActionResult Basket(int Id)
        {
            var list = NSer.GetFileInfoList(Id);
            return View(list);
        }
    }
}
