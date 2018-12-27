using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ModelProject
{
    public class SemiModel
    {
        public int id { get; set; }
        public string productName { get; set; }
        public string ProductXL { get; set; }
        public int product_id { get; set; }
        public int wood_id { get; set; }
        public string woodname { get; set; }
        public int? inv_id { get; set; }
        public string invname { get; set; }
        public DateTime? input_date { get; set; }
        public int? product_SN_id { get; set; }
        public int? product_area_id { get; set; }
        public int? product_SN { get; set; }
        public int? product_area { get; set; }
        public string areaName { get; set; }
        public List<SelectListItem> XLDroList { get; set; }
        public List<SelectListItem> AreaDroList { get; set; }
        public List<SelectListItem> MCDroList { get; set; }
        public List<SelectListItem> CKDroList { get; set; }
        public int? length { get; set; }
        public int? width { get; set; }
        public int? height { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
    public class SSemiModel
    {
        public int product_id { get; set; }
        public string productName { get; set; }
        public int? wood_id { get; set; }
        public int? inv_id { get; set; }
        public int? product_SN_id { get; set; }
        public int? product_area_id { get; set; }
        public List<SelectListItem> XLDroList { get; set; }
        public List<SelectListItem> AreaDroList { get; set; }
        public List<SelectListItem> CKDroList { get; set; }
        public List<SelectListItem> MCDroList { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
