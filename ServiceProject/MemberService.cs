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
        public bool AddUser(MemberModel Models, out Guid UserId)
        {
            try { UDal.AddUser(Models, out UserId); return true; }
            catch (Exception)
            {
                UserId = Guid.Empty;
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
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="PassWord"></param>
        /// <param name="UserId"></param>
        /// <param name="IsUserInfo"></param>
        /// <returns></returns>
        public bool IsLogin(string UserCode, string PassWord, out Guid UserId, out bool IsUserInfo, string openId, out string UserName)
        {
            try { return UDal.IsLogin(UserCode, PassWord, out UserId, out IsUserInfo, openId, out UserName); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 微信登录
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="PassWord"></param>
        /// <param name="UserId"></param>
        /// <param name="IsUserInfo"></param>
        /// <returns></returns>
        public bool IsWXLogin(string openId, out Guid UserId, out bool IsUserInfo, out bool IsOpenId)
        {
            try { return UDal.IsWXLogin(openId, out UserId, out IsUserInfo, out IsOpenId); }
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
