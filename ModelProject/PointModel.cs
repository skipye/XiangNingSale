using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelProject
{
    public class PointModel
    {
        public string UserName { get; set; }
        public string OrderNum { get; set; }
        public decimal? RequstPay { get; set; }
        public DateTime? CreateTime { get; set; }
        public decimal? OrderPrice { get; set; }

    }
}
