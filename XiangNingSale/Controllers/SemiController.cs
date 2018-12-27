using ModelProject;
using ServiceProject;
using System;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace XiangNingSale.Controllers
{
    public class SemiController : Controller
    {
        private static readonly INVService INSer = new INVService();
        public ActionResult Index()
        {
            SSemiModel Models = new SSemiModel();
            Models.XLDroList = INSer.GetXLDrolist(Models.product_SN_id);
            Models.AreaDroList = INSer.GetAreaDrolist(Models.product_area_id);
            Models.CKDroList = INSer.GetCKDrolist(Models.inv_id, 3);
            Models.MCDroList = INSer.GetWoodDrolist(Models.wood_id);
            return View(Models);
        }
        public ActionResult PageList(SSemiModel SModels)
        {
            var PageList = INSer.GetSemiList(SModels);
            return new ContentResult
            {
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(PageList),
                ContentType = "application/json"
            };
        }
    }
}
