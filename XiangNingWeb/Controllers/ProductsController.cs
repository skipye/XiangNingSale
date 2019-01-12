using ModelProject;
using ServiceProject;
using System.Web.Mvc;

namespace XiangNingWeb.Controllers
{
    public class ProductsController : Controller
    {
        private static readonly NewsService NSer = new NewsService();
        public ActionResult Index(SNewsModel SModel)
        {
            SModel.AreaList = NSer.GetWebArealist();
            SModel.TypeList = NSer.GetWebTypeList(2);
            return View(SModel);
        }
        public ActionResult Detail(int Id)
        {
            var Models = NSer.GetDetailById(Id);
            return View(Models);
        }
        public ActionResult _RecommendPro()
        {
            return View();
        }
        public ActionResult PageList(SNewsModel SModel)
        {
            ViewBag.SModel = SModel;
            var models = NSer.GetWebPageList(SModel, 2, SModel.PageIndex??1, SModel.PageSize??9);
            return View(models);
        }
    }
}
