using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelProject
{
    public class WX_Shar_ReturnModel
    {
        public string ToUserName { get; set; }
        public string FromUserName { get; set; }
        public string MsgType { get; set; }
        public string Event { get; set; }
        public string EventKey { get; set; }
        public string Ticket { get; set; }
        public string Remarks { get; set; }
    }
}
