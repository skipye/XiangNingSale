using DataBase;
using ModelProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DalProject
{
    public class MessageDal
    {
        
        //获取会员回复列表
        public List<ReplyModel> GetMemberReplyList(int PageIndex, int PageSize, Guid? UserId)
        {
            using (var db = new XiangNingSaleEntities())
            {
                var list = (from p in db.MemberMessage.Where(k => k.State == true && k.Checked == true)
                            where UserId != null && UserId != Guid.Empty ? p.MemberId == UserId.Value : true
                            orderby p.CreateTime descending
                            select new ReplyModel
                            {
                                Id = p.Id,
                                StrContent = p.StrContent,
                                CreateTime = p.CreateTime,
                                UserName = p.UserName,
                                IsRead = p.IsRead

                            }).Skip(PageIndex * PageSize).Take(PageSize).ToList();
                return list;
            }
        }
        //更新会员信息阅读状态
        public void UpdateMemberMessageState(Guid UserId)
        {
            using (var db = new XiangNingSaleEntities())
            {
                var list = db.MemberMessage.Where(k => k.State == true && k.MemberId == UserId).ToList();
                foreach (var item in list)
                {
                    item.IsRead = true;
                }
                db.SaveChanges();
            }
        }
    }
}
