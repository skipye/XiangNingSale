using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ModelProject
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Address { get; set; }
        public string Address_Delivery { get; set; }
        public string LinkMan { get; set; }
        public string LinkTel { get; set; }
        public string Email { get; set; }
        public int? BelongUserId { get; set; }
        public string BelongUserName { get; set; }
        public int? DepartmentId { get; set; }
        public string Department { get; set; }
        public DateTime? CreateTime { get; set; }
        public List<SelectListItem> DepartmentDroList { get; set; }
    }
    public class SCustomerModel
    {
        public string Name { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int? DepartmentId { get; set; }
        public int? BelongUserId { get; set; }
        public List<SelectListItem> DepartmentDroList { get; set; }
    }
    public class BelongModel
    {
        public int Id { get; set; }
        public int? BelongUserId { get; set; }
        public string BelongUserName { get; set; }
        public int? DepartmentId { get; set; }
        public string Department { get; set; }
        public List<SelectListItem> DepartmentDroList { get; set; }
        public List<SelectListItem> UserDroList { get; set; }
    }
}
