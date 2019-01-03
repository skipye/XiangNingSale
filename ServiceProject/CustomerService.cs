using System;
using System.Collections.Generic;
using ModelProject;
using DalProject;
using System.Web.Mvc;

namespace ServiceProject
{
    public class CustomerService
    {
        private static readonly CustomerDal IDal = new CustomerDal();
        public List<CustomerModel> GetPageList(SCustomerModel SModel)
        {
            try { return IDal.GetPageList(SModel); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool AddOrUpdate(CustomerModel Models)
        {
            try {IDal.AddOrUpdate(Models); return true; }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public CustomerModel GetDetailById(int Id)
        {
            try { return IDal.GetDetailById(Id); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool DeleteMore(string ListId)
        {
            try { IDal.DeleteMore(ListId); return true; }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool UpdateBelong(BelongModel Models)
        {
            try { IDal.UpdateBelong(Models); return true; }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<SelectListItem> GetCustomerDrolist(int? pId, int? UserId, int? DepartmentId)
        {
            try { return IDal.GetCustomerDrolist(pId, UserId, DepartmentId); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
    }
}
