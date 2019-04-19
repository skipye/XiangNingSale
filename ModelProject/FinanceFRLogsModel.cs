using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelProject
{
    public class FinanceFRLogsModel
    {
        public string HTSN { get; set; }
        public string Customer { get; set; }
        public string Remaks { get; set; }
        public string PayModel { get; set; }
        public string operator_name { get; set; }
        public DateTime? CreateTime { get; set; }
        public decimal? Amount { get; set; }
    }
}
