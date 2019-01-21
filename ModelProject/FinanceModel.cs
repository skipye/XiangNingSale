using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelProject
{
    public class FinanceModel
    {
        public int Id { get; set; }
        public int HTId { get; set; }
        public string HTSN { get; set; }
        public string Customer { get; set; }
        public string PayModel { get; set; }
        public int? operator_id { get; set; }
        public string operator_name { get; set; }
        public decimal? Amount { get; set; }
        public string Remaks { get; set; }
    }
}
