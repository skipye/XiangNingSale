using DalProject;
using ModelProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceProject
{
    public class MessageService
    {
        private static readonly MessageDal MDal = new MessageDal();
        //获取会员回复列表
        public List<ReplyModel> GetMemberReplyList(int PageIndex, int PageSize, Guid? UserId)
        {
            try { return MDal.GetMemberReplyList(PageIndex, PageSize, UserId); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void UpdateMemberMessageState(Guid UserId)
        {
            try { MDal.UpdateMemberMessageState(UserId); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
