using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelProject
{
    public class LoginModel
    {
        public string UserName { get; set; }//昵称
        public string PassWord { get; set; }//用户密码
        public string valiCode { get; set; }
        public string Telephone { get; set; }//手机号码
        public int UserId { get; set; }
        public bool IsLogin { get; set; }
    }
    public class UsersModel
    {
        public int Id { get; set; }
        public string Name { get; set; }//昵称
        public string Password { get; set; }//用户密码
        public string Password2 { get; set; }
        public string Telphone { get; set; }
        public DateTime? LastTimes { get; set; }//手机号码
        public int? LoginTimes { get; set; }
        public DateTime? CreateTime { get; set; }
    }
    public class SUsersModel
    {
        public string Name { get; set; }
    }
    public class UserCurrentModel
    {
        public int UserId { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
    }
}
