using DalProject;
using ModelProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceProject
{
    public class UserService
    {
        private static readonly UserDal UDal = new UserDal();
        //用户登录
        public LoginModel IsLogin(LoginModel models)
        {
            try { return UDal.IsLogin(models); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public UserCurrentModel GetCurrentUserName()
        {
            try { return UDal.GetCurrentUserName(); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
