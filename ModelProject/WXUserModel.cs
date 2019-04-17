using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelProject
{
    public class WXUserModel
    {
        public string openid { get; set; }
        public string nickname { get; set; }//用户昵称
        public string sex { get; set; }//用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        public string province { get; set; }//用户个人资料填写的省份
        public string city { get; set; }//普通用户个人资料填写的城市
        public string country { get; set; }//国家，如中国为CN
    }
}
