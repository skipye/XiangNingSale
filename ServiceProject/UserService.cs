using DalProject;
using ModelProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ServiceProject
{
    public class UserService
    {
        private static readonly UserDal UDal = new UserDal();
        public void AddWorkLogs(WorkLogsModel tables)
        {
            try { UDal.AddWorkLogs(tables); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<UsersModel> GetPageList(SUsersModel SModel)
        {
            try { return UDal.GetPageList(SModel); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool AddOrUpdate(UsersModel Models)
        {
            try { UDal.AddOrUpdate(Models); return true; }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public UsersModel GetDetailById(int Id)
        {
            try { return UDal.GetDetailById(Id); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool Delete(string ListId)
        {
            try { UDal.Delete(ListId); return true; }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
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
        public List<SelectListItem> GetUserDrolist(int? pId)
        {
            try { return UDal.GetUserDrolist(pId); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<SelectListItem> GetDepartmentDrolist(int? pId)
        {
            try { return UDal.GetDepartmentDrolist(pId); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
