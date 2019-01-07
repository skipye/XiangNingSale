using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DalProject;
using ModelProject;

namespace ServiceProject
{
    public class PurchaseOrderService
    {
        private static readonly PurchaseOrderDal PODal = new PurchaseOrderDal();
        public List<PurchaseOrderModel> GetPageList(SPurchaseOrderModel SModel)
        {
            try { return PODal.GetPageList(SModel); }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public bool AddOrUpdate(PurchaseOrderModel Models)
        {
            try { PODal.AddOrUpdate(Models); return true; }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public PurchaseOrderModel GetDetailById(int Id)
        {
            try { return PODal.GetDetailById(Id); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool Delete(string ListId)
        {
            try { PODal.DeleteMore(ListId); return true; }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool Checked(string ListId, int CheckedId, int UserId, string UserName)
        {
            try { PODal.CheckedMore(ListId, CheckedId, UserId, UserName); return true; }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool CWCheckedMore(string ListId, int status, string Remarks)
        {
            try { PODal.CWCheckedMore(ListId, status, Remarks); return true; }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool CWAccounts(string ListId)
        {
            try { PODal.CWAccounts(ListId); return true; }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
