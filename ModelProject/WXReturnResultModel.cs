using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelProject
{
    public class WXReturnResultModel
    {
        public string openid { get; set; }
        public string trade_type { get; set; }//交易类型
        public string bank_type { get; set; }//付款银行
        public string total_fee { get; set; }//订单金额
        public string fee_type { get; set; }//货币种类
        public string transaction_id { get; set; }
        public string out_trade_no { get; set; }//商户订单号
        public string is_subscribe { get; set; }//是否关注公众账号
        public string time_end { get; set; }//支付完成时间
    }
}
