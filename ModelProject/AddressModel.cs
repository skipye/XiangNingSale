using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelProject
{
    public class AddressModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Telphone { get; set; }
        public int? ProvinceId { get; set; }
        public string Province { get; set; }
        public int? CityId { get; set; }
        public string City { get; set; }
        public int? RegionId { get; set; }
        public string Region { get; set; }
        public string addressNo { get; set; }
        public Guid? MemberId { get; set; }
        public bool? IsTop { get; set; }
        public int? YunFei { get; set; }
        public string ReturnUrl { get; set; }
        public bool? IsPost { get; set; }
    }
}
