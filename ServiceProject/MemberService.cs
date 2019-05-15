using DalProject;
using ModelProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceProject
{
    public class MemberService
    {
        private static readonly MemberDal UDal = new MemberDal();
        public UserIdOrNameModel GetUserIdOrName()
        {
            UserIdOrNameModel models = new UserIdOrNameModel();
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.User.Identity.Name))
            {
                models.MemberId = Guid.Parse(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
                models.Name = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];
            }
            return models;
        }
        public MemberModel GetUserDetail(Guid UserId)
        {
            try { return UDal.GetUserDetail(UserId); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool AddUser(MemberModel Models, out Guid UserId, out string MemNum)
        {
            try { UDal.AddUser(Models, out UserId, out MemNum); return true; }
            catch (Exception)
            {
                UserId = Guid.Empty;
                MemNum = "";
                return false;
            }
        }
        public bool ChangPassword(string NewPassword)
        {
            try
            {
                Guid MemberId = GetUserIdOrName().MemberId;
                UDal.ChangPassword(MemberId, NewPassword); return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool BackPassword(string Telphone, string NewPassword)
        {
            try
            {
                UDal.BackPassword(Telphone, NewPassword); return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public LoginModel IsWXLogin(string openId)
        {
            try { return UDal.IsWXLogin(openId); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public LoginModel IsLogin(string UserCode, string PassWord, string openId)
        {
            try { return UDal.IsLogin(UserCode, PassWord, openId); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool IsSameName(string Name)
        {
            try { return UDal.IsSameName(Name); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool IsSamePhone(string Phone)
        {
            try { return UDal.IsSamePhone(Phone); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool UpdatePhone(string Phone, string password, Guid UserId)
        {
            try { return UDal.UpdatePhone(Phone, password, UserId); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
