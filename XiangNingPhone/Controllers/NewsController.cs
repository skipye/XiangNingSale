using ModelProject;
using ServiceProject;
using System.Web.Mvc;

namespace XiangNingPhone.Controllers
{
    public class NewsController : Controller
    {
        private static readonly NewsService NSer = new NewsService();
        public ActionResult Index(SNewsModel SModel)
        {
            return View(SModel);
        }
        public ActionResult List(int? TypeId, int? PageIndex, int? PageSize)
        {
            SNewsModel SModel = new SNewsModel();
            SModel.PageSize = PageSize??20;
            SModel.PageIndex = PageIndex ?? 0;
            var models = NSer.GetNewsTypeList(SModel, 1);
            return View(models);
        }
        public ActionResult Detail(int Id)
        {
            var Models = NSer.GetDetailById(Id);
            return View(Models);
        }
    }
}
