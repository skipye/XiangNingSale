using ServiceProject;
using ModelProject;
using System.Web.Mvc;

namespace XiangNingPhone.Controllers
{
    public class ProductsController : BaseController
    {
        private static readonly NewsService NSer = new NewsService();
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
        public ActionResult PageList(SNewsModel SModel,int? PageSize)
        {
            SModel.PageSize = PageSize??9;
            var models = NSer.GetWebPageList(SModel, 2);
            return View(models);
        }
        public ActionResult List(SNewsModel SModel, int? PageIndex, int? PageSize)
        {
            SModel.PageSize = PageSize ?? 10;
            SModel.PageIndex = PageIndex ?? 0;
            var Models = NSer.GetNewsTypeList(SModel,2);
            return View(Models);
        }
        public ActionResult Detail(int Id)
        {
            var Models = NSer.GetDetailById(Id);

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
    }
}
