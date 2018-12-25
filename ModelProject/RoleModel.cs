using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ModelProject
{
    public class RoleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? TypeId { get; set; }
        public int? ParentId { get; set; }
        public string TypeName { get; set; }
        public string ParentName { get; set; }
        public DateTime? CreateTime { get; set; }
        public List<SelectListItem> TypeDroList { get; set; }
        public int? Rank { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Remark { get; set; }
    }
    public class SRoleModel
    {
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string TypeName { get; set; }
        public string ParentName { get; set; }
        public int? TypeId { get; set; }
        public List<SelectListItem> TypeDroList { get; set; }
    }
}
