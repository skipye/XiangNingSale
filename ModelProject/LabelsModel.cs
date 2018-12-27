using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ModelProject
{
    public class LabelsModel
    {
        public int id { get; set; }
        public string SN { get; set; }//标签编码
        public string product_SN_Name { get; set; }//产品编号
        public string ProductName { get; set; }
        public string ProductXL { get; set; }
        public int pid { get; set; }
        public int product_id { get; set; }
        public int? wood_id { get; set; }
        public string woodname { get; set; }
        public int? inv_id { get; set; }
        public int? inv { get; set; }
        public string invname { get; set; }
        public DateTime? input_date { get; set; }
        public int? input_user_id { get; set; }
        public string customersName { get; set; }
        public int status { get; set; }
        public int? CRM_id { get; set; }
        public int? WIP_id { get; set; }
        public int? product_SN_id { get; set; }
        public int? product_area_id { get; set; }
        public int? product_SN { get; set; }
        public int? product_area { get; set; }
        public string ProductareaName { get; set; }
        public List<SelectListItem> XLDroList { get; set; }
        public List<SelectListItem> AreaDroList { get; set; }
        public List<SelectListItem> MCDroList { get; set; }
        public List<SelectListItem> SHDroList { get; set; }
        public List<SelectListItem> CKDroList { get; set; }
        public string color { get; set; }
        public int? color_id { get; set; }
        public int? flag { get; set; }//销售0，预投1
        public string style { get; set; }//规格
        public string Remark { get; set; }
        public int? length { get; set; }
        public int? width { get; set; }
        public int? height { get; set; }
        public string ListId { get; set; }
        public int qty { get; set; }
        public DateTime? check_date { get; set; }
        public string CRM_SN { get; set; }
        public int CRM_HTId { get; set; }
        public int? PageIndex { get; set; }
        public int? PagePSize { get; set; }
        public bool? delete_flag { get; set; }
        public decimal? W_BZ { get; set; }
        public decimal? volume { get; set; }
        public decimal? W_price { get; set; }
        public string OrderNum { get; set; }
        public decimal? price { get; set; }
        public decimal? PersonPrice { get; set; }
        public decimal? g_ccl { get; set; }
        public decimal? q_ccl { get; set; }
        public decimal? cc_prcie { get; set; }
        public bool? CW_checked { get; set; }
        public bool? CZ_checked { get; set; }
    }
    public class SLabelsModel
    {
        public int Id { get; set; }
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
