using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ModelProject
{
    public class NewsModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? TypeId { get; set; }
        public string TypeName { get; set; }
        public string Remarks { get; set; }
        public string ConvertImg { get; set; }
        public DateTime? CreateTime { get; set; }
        public string StrContent { get; set; }
        public string KeyWord { get; set; }
        public List<SelectListItem> TypeDroList { get; set; }
        public string GalleryItems { get; set; }
        public int? HitTimes { get; set; }
        public string UploadName { get; set; }
        public int? CheckedStatus { get; set; }
        public int? UploadAuthorId { get; set; }
        public string EidtAuthorName { get; set; }
        public int? EidtAuthorId { get; set; }
        public int? AreaId { get; set; }
        public string AreaName { get; set; }
        public List<SelectListItem> AreaDroList { get; set; }
        
    }
    public class SNewsModel
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Name { get; set; }
        public int? TypeId { get; set; }
        public List<SelectListItem> TypeDroList { get; set; }
        public int? CheckedStatus { get; set; }
        public int? UploadAuthorId { get; set; }
        public int? AreaId { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public List<CRMItem> AreaList { get; set; }
        public List<CRMItem> TypeList { get; set; }
    }
    public class NewsItmeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TypeNav { get; set; }
        public string LinkUrl { get; set; }
    }
    public class GalleryModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public long? ProductId { get; set; }
        public string ThumbnailsUrl { get; set; }
        public string PublishUrl { get; set; }
    }
    public class NewsTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? PrId { get; set; }
    }
}
