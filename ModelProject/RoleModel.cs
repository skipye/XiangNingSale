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
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string MenuList { get; set; }
        public DateTime? CreateTime { get; set; }
        public List<MenuItemModel> MenuItemList { get; set; }
        public List<SelectListItem> UserDroList { get; set; }
    }
    public class SRoleModel
    {
        public string Name { get; set; }
    }
    
}
