using ModelProject;
using System;
using System.Web.Mvc;
using ServiceProject;
using System.Web.Script.Serialization;

namespace XiangNingSale.Controllers
{
    public class LabelsController : Controller
    {
        private static readonly INVService INSer = new INVService();
        public ActionResult Index()
        {
            SLabelsModel Models = new SLabelsModel();
            Models.XLDroList = INSer.GetXLDrolist(Models.product_SN_id);
            Models.AreaDroList = INSer.GetAreaDrolist(Models.product_area_id);
            Models.CKDroList = INSer.GetCKDrolist(Models.inv_id, 4);
            Models.MCDroList = INSer.GetWoodDrolist(Models.wood_id);
            return View(Models);
        }
        public ActionResult PageList(SLabelsModel SModels)
        {
            var PageList = INSer.GetLabelsList(SModels);
            return new ContentResult
            {
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(PageList),
                ContentType = "application/json"
            };
        }
    }
}
