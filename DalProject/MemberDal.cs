using DataBase;
using ModelProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WxPayAPI;

namespace DalProject
{
    public class MemberDal
    {
        public MemberModel GetUserDetail(Guid UserId)
        {
            using (var db = new XiangNingSaleEntities())
            {
                var tables = (from p in db.MemberInfo.Where(k => k.Id == UserId)
                              select new MemberModel
                              {
                                  Id = p.Id,
                                  Name = p.userName,
                                  RealName = p.RealName,
                                  Sex = p.Sex,
                                  Email = p.Email,
                                  Telphone = p.Telphone,
                                  Age = p.Age,
                                  AreaId = p.AreaId,
                                  IndustryId = p.IndustryId,
                                  Stock = p.Stock,
                                  AddStock = p.AddStock,
                                  Gold = p.Gold,
                                  MemberNumber = p.MemberNumber,
                                  ZStock = p.ZStock,
                                  Commission=p.Commission,
                              }).FirstOrDefault();
                tables.OneRequestUserCount = RequestUserCount(tables.MemberNumber, 1);
                tables.TowRequestUserCount = RequestUserCount(tables.MemberNumber, 2);
                return tables;
            }
        }
        
        //获取一级客户、二级客户个数
        public int RequestUserCount(string MemberNumber, int Request)
        {
            using (var db = new XiangNingSaleEntities())
            {
                if (Request == 1)
                { return db.MemberInfo.Where(k => k.RequestNumber_1 == MemberNumber && k.State == true).Count(); }
                else { return db.MemberInfo.Where(k => k.RequestNumber_2 == MemberNumber && k.State == true).Count(); }
            }
        }
        //获取未读信息
        public int UserMessageCount(Guid UserId)
        {
            using (var db = new XiangNingSaleEntities())
            {
                return db.MemberMessage.Where(k => k.State == true && k.IsRead == false && k.MemberId == UserId).Count();
            }
        }
        //添加和修改用户信息
        public void AddUser(MemberModel Models, out Guid UserId, out string MemberNumber,out string telphone)
        {
            string Mn = "";
            Guid UId = Guid.Empty;
            string tel = "";
            using (var db = new XiangNingSaleEntities())
            {
                int Stoct = 0;

                if (Models.Id != null && Models.Id != Guid.Empty)
                {
                    var Tabels = db.MemberInfo.Where(k => k.Id == Models.Id).FirstOrDefault();
                    Tabels.userName = Models.Name;
                    Tabels.RealName = Models.RealName;
                    Tabels.Telphone = Models.Telphone;
                    Tabels.Age = Models.Age;
                    Tabels.Email = Models.Email;
                    Tabels.Sex = Models.Sex;
                    Tabels.Stock = Tabels.Stock > 0 ? Tabels.Stock + Stoct : Stoct;
                    Tabels.ZStock = Tabels.ZStock > 0 ? Tabels.ZStock + Stoct : Stoct;
                }
                else
                {
                    var Tabels = new MemberInfo();
                    Tabels.Id = Guid.NewGuid();
                    Tabels.userName = Models.Name;
                    Tabels.RealName = Models.RealName;
                    Tabels.Password = Models.Password;
                    Tabels.Telphone = Models.Telphone;
                    Tabels.Email = Models.Email;
                    Tabels.CreateTime = DateTime.Now;
                    Tabels.LoginTimes = 1;
                    Tabels.LastLoginTime = DateTime.Now;
                    Tabels.Sex = Models.Sex;
                    Tabels.Stock = !string.IsNullOrEmpty(Models.RequestNumber) ? 100 : 20;
                    Tabels.ZStock = !string.IsNullOrEmpty(Models.RequestNumber) ? 100 : 20;
                    Tabels.AddStock = 0;
                    Tabels.Gold = 0;
                    Tabels.State = Models.RealName != null && Models.RealName == "微信注册用户" ? false : true;
                    Tabels.MemberNumber = "HT" + WxPayApi.GenerateTimeStamp();
                    Mn = Tabels.MemberNumber;
                    UId = Tabels.Id;
                    tel = Tabels.Telphone;
                    Tabels.OpenId = Models.OpenId;
                    if (!string.IsNullOrEmpty(Models.RequestNumber))
                    {
                        Tabels.RequestNumber_1 = Models.RequestNumber;

                        var Membertabel = db.MemberInfo.Where(k => k.MemberNumber == Models.RequestNumber).FirstOrDefault();
                        if (Membertabel != null)
                        {
                            if (Membertabel.RequestNumber_1 != null)
                            {
                                Tabels.RequestNumber_2 = Membertabel.RequestNumber_1;
                            }
                        }
                    }
                    db.MemberInfo.Add(Tabels);
                }
                UserId = UId;
                telphone = tel;
                MemberNumber = Mn;
                db.SaveChanges();
            }
        }
        public LoginModel IsWXLogin(string openId)
        {
            LoginModel UserModels = new LoginModel();
            using (var db = new XiangNingSaleEntities())
            {
                var Tables = (from p in db.MemberInfo.Where(k => k.State == true && k.OpenId == openId)
                              select p).FirstOrDefault();

                if (Tables != null)
                {
                    UserModels.UserName = Tables.userName;
                    UserModels.MemberId = Tables.Id;
                    UserModels.MemberNumber = Tables.MemberNumber;
                    UserModels.Telephone = Tables.Telphone;
                    UserModels.IsLogin = true;
                    Tables.LoginTimes = Tables.LoginTimes + 1;
                    Tables.LastLoginTime = DateTime.Now;
                    db.SaveChanges();
                }
            }
            return UserModels;
        }
        public LoginModel IsLogin(string UserCode, string PassWord, string openId)
        {
            LoginModel UserModels = new LoginModel();
            string TuserName = "";
            string telphone = "";
            if (IsCount(UserCode) == true)
            { telphone = UserCode; }
            else { TuserName = UserCode; }
            using (var db = new XiangNingSaleEntities())
            {

                var Tables = (from p in db.MemberInfo.Where(k => k.State == true && k.Password == PassWord)
                              where !string.IsNullOrEmpty(TuserName) ? p.userName == TuserName : true
                              where !string.IsNullOrEmpty(telphone) ? p.Telphone == telphone : true
                              select p).FirstOrDefault();

                if (Tables != null)
                {
                    UserModels.UserName = Tables.userName;
                    UserModels.MemberId = Tables.Id;
                    UserModels.MemberNumber = Tables.MemberNumber;
                    UserModels.Telephone = Tables.Telphone;
                    UserModels.IsLogin = true;
                    Tables.LoginTimes = Tables.LoginTimes + 1;
                    Tables.LastLoginTime = DateTime.Now;
                    if (string.IsNullOrEmpty(Tables.OpenId))
                    {
                        if (string.IsNullOrEmpty(openId))
                        {
                            Tables.OpenId = openId;
                        }
                    }
                    db.SaveChanges();

                }
            }
            return UserModels;
        }
        public bool IsCount(string keyword)
        {
            double result;
            bool isOK = double.TryParse(keyword, out result);
            if (isOK)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        /// <summary>
        /// 微信登录
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public bool IsWXLogin(string openId, out Guid UserId, out bool IsUserInfo, out bool IsOpenId)
        {
            using (var db = new XiangNingSaleEntities())
            {
                var Tables = db.MemberInfo.Where(k => k.OpenId == openId).FirstOrDefault();
                if (Tables != null)
                {
                    UserId = Tables.Id;
                    if (Tables.State == true)
                    {
                        Tables.LoginTimes = Tables.LoginTimes + 1;
                        Tables.LastLoginTime = DateTime.Now;
                        
                        db.SaveChanges();
                        IsOpenId = false;
                    }
                    else { IsOpenId = true; }

                    if (Tables.Telphone == null || Tables.AreaId == null || Tables.Email == null || Tables.IndustryId == null)
                    { IsUserInfo = true; }
                    else { IsUserInfo = false; }

                    return true;
                }
                else { UserId = Guid.Empty; IsUserInfo = false; IsOpenId = false; return false; }
            }
        }
       
        public void ChangPassword(Guid MemberId, string NewPassword)
        {
            using (var db = new XiangNingSaleEntities())
            {
                var tablse = db.MemberInfo.Where(k => k.Id == MemberId).SingleOrDefault();
                tablse.Password = NewPassword;
                db.SaveChanges();
            }
        }
        public bool IsSameName(string Name)
        {
            using (var db = new XiangNingSaleEntities())
            {
                var table = db.MemberInfo.Where(k => k.State == true && k.userName == Name).FirstOrDefault();
                if (table != null)
                {
                    return true;
                }
                else { return false; }
            }
        }
        public void BackPassword(string Telphone, string NewPassword)
        {
            using (var db = new XiangNingSaleEntities())
            {
                var tablse = db.MemberInfo.Where(k => k.Telphone == Telphone && k.State == true).FirstOrDefault();
                tablse.Password = NewPassword;
                db.SaveChanges();
            }
        }
        public bool IsSamePhone(string Phone)
        {
            using (var db = new XiangNingSaleEntities())
            {
                var table = db.MemberInfo.Where(k => k.State == true && k.Telphone == Phone).FirstOrDefault();
                if (table != null)
                {
                    return true;
                }
                else { return false; }
            }
        }
        public bool UpdatePhone(string Phone, string password, Guid UserId)
        {
            using (var db = new XiangNingSaleEntities())
            {
                var table = db.MemberInfo.Where(k => k.Id == UserId).FirstOrDefault();
                if (table != null)
                {
                    table.Telphone = Phone;
                    table.Password = password;
                    table.State = true;
                    db.SaveChanges(); return true;
                }
                else { return false; }
            }
        }
        //根据会员号获取手机号
        public string GetTelPhoneByMemberNum(string MemberNum)
        {
            using (var db = new XiangNingSaleEntities())
            {
                var dates = db.MemberInfo.Where(k => k.MemberNumber == MemberNum).Select(k => k.Telphone).FirstOrDefault();
                return dates;
            }
        }
    }
}
