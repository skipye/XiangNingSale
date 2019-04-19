using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ModelProject
{
    public class MemberModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string RealName { get; set; }
        public string Telphone { get; set; }
        public string QQ { get; set; }
        public string Email { get; set; }
        public string ConvertImg { get; set; }
        public string Sex { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public int? LoginTimes { get; set; }
        public DateTime? QuitTime { get; set; }
        public string CheckCode { get; set; }
        public string APassword { get; set; }
        public int? Stock { get; set; }
        public int? AddStock { get; set; }
        public int? ZStock { get; set; }
        public int? Gold { get; set; }
        public int? Age { get; set; }
        public int? IndustryId { get; set; }
        public string Industry { get; set; }
        public int? AreaId { get; set; }
        public string Area { get; set; }
        public string MemberNumber { get; set; }
        public string RequestNumber { get; set; }
        public List<SelectListItem> AreaList { get; set; }
        public List<SelectListItem> IndustryList { get; set; }
        public int MessageCount { get; set; }
        public int UserOrderCount { get; set; }
        public int UserPMCount { get; set; }
        public int UserSodCount { get; set; }
        public string OpenId { get; set; }
        public int OneRequestUserCount { get; set; }
        public int TowRequestUserCount { get; set; }
        public decimal? Commission { get; set; }
    }
    public class UserIdOrNameModel
    {
        public Guid MemberId { get; set; }
        public string Name { get; set; }
        public string ReturnUrl { get; set; }
        public string strContent { get; set; }
        public int? TotalCount { get; set; }
    }
}
