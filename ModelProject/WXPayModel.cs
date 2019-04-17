using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelProject
{
    public class WXPayModel
    {
        public string OrderId { get; set; }//商品描述
        public string openid { get; set; }

    }
    public class ModelForOrder
    {
        public string appId { get; set; }
        public string timeStamp { get; set; }
        public string nonceStr { get; set; }
        public string packageValue { get; set; }
        public string paySign { get; set; }
        public string OrderId { get; set; }
        public string openid { get; set; }
        public string msg { get; set; }
        public string signature { get; set; }
    }
}
